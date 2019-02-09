using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Spotify.Sdk.Android.Authentication;
using Nonstop.Droid.Spotify;
using Nonstop.Forms.Spotify;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidSPTAuthentication))]
namespace Nonstop.Droid.Spotify
{
    public class AndroidSPTAuthentication : ISPTAuthentication
    {
        const int REQUEST_CODE = 1337;

        private string accessToken;
        private long tokenExpireAt;

        private EventHandler<TokenReceiverEventArgs>  _tokenReady;
        public event EventHandler<TokenReceiverEventArgs> tokenReady
        {
            add
            {
                if (_tokenReady == null || !_tokenReady.GetInvocationList().Contains(value))
                {
                    _tokenReady += value;
                }
                else
                {
                    // Do here some logging
                }
            }
            remove
            {
                _tokenReady -= value;
            }
        }

        public void authRequest(string[] scopes)
        {
            if(isTokenExpired())
            {
                /* SPT get token request */
                AuthenticationRequest.Builder builder = new AuthenticationRequest.Builder(
                    SPTCredentials.CLIENT_ID, AuthenticationResponse.Type.Token, SPTCredentials.REDIRECT_URI);

                builder.SetScopes(scopes);
                AuthenticationRequest request = builder.Build();

                Activity currentAndroidContext = CrossCurrentActivity.Current.Activity;
                if (currentAndroidContext is MainActivity)
                {
                    ((MainActivity)currentAndroidContext).onActivityResult += new ActivityResultEventHandler(onActivityResult);
                }
                AuthenticationClient.OpenLoginActivity(currentAndroidContext, REQUEST_CODE, request);
            } else
            {
                _tokenReady(this, new TokenReceiverEventArgs { token = accessToken });
            }
            
        }

        private void updateExpireTime(int expireInSec)
        {
            long timestamp = DateTime.Now.Ticks;
            tokenExpireAt = timestamp + expireInSec;
        }

        private bool isTokenExpired()
        {
            long timestamp = DateTime.Now.Ticks;
            return timestamp <= tokenExpireAt;
        }

        public void registerToken(ISPTTokenReceiver receiver)
        {
            tokenReady += receiver.onTokenReady;
        }

        public void onActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            // Check if result comes from the correct activity
            if (requestCode == REQUEST_CODE)
            {
                Console.WriteLine("activity resulted");

                int resultCode_ = resultCode == Result.Ok ? -1 : 0;
                AuthenticationResponse response = AuthenticationClient.GetResponse(resultCode_, intent);

                if (response.GetType() == AuthenticationResponse.Type.Token)
                {
                    string accessToken = response.AccessToken;
                    _tokenReady(this, new TokenReceiverEventArgs { token = accessToken });
                }
                else if (response.GetType() == AuthenticationResponse.Type.Error)
                {
                    if (response.Error == "NO_INTERNET_CONNECTION")
                    {

                    }
                    Console.WriteLine(response.Error);
                }
                else
                {

                }
            }
        }
    }
}