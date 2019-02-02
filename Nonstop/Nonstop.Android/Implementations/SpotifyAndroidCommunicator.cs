using System;
using Android.App;
using Android.Content;
using Android.Util;
using Com.Spotify.Android.Appremote.Api;
using Com.Spotify.Android.Appremote.Api.Error;
using Com.Spotify.Protocol.Client;
using Com.Spotify.Protocol.Types;
using Com.Spotify.Sdk.Android.Authentication;
using Java.Lang;
using Nonstop.Droid;
using Nonstop.Droid.Implementations;
using Nonstop.Forms.AppRemote;

[assembly: Xamarin.Forms.Dependency(typeof(SpotifyAndroidCommunicator))]
namespace Nonstop.Droid.Implementations
{
    public class SpotifyAndroidCommunicator : ISpotifyCommunicator
    {
        public static string CLIENT_ID = "90708631ebfb4fed93d3c0000c8c0eef";
        public static string REDIRECT_URI = "com.Nonstop://callback";

        const int REQUEST_CODE = 1337;

        public SpotifyAppRemote spotifyAppRemote;

        public Nonstop.Droid.MainActivity androidContext;

        public SpotifyAndroidCommunicator(System.Object ac)
        {
            this.androidContext = (Nonstop.Droid.MainActivity) ac;
        }
        // start
        public void start()
        {
            ConnectionParams connectionParams = new ConnectionParams.Builder(CLIENT_ID)
                                    .SetRedirectUri(REDIRECT_URI)
                                    .ShowAuthView(true)
                                    .Build();

            ConnectorConnectionListener listener = new ConnectorConnectionListener(this);

            /* Connect to SPT APP */
            SpotifyAppRemote.Connect(androidContext, connectionParams, listener);


            /* SPT get token request */
            AuthenticationRequest.Builder builder = new AuthenticationRequest.Builder(CLIENT_ID, AuthenticationResponse.Type.Token, REDIRECT_URI);

            builder.SetScopes(new string[] { "streaming" });
            AuthenticationRequest request = builder.Build();

            AuthenticationClient.OpenLoginActivity(androidContext, REQUEST_CODE, request);
        }
        // stop
        public void stop()
        {
            
        }
        public void destroy()
        {
            SpotifyAppRemote.Disconnect(spotifyAppRemote);
        }
        public void onActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            const int REQUEST_CODE = 1337;

            // Check if result comes from the correct activity
            if (requestCode == REQUEST_CODE)
            {
                int resultCode_ = resultCode == Result.Ok ? -1 : 0;
                AuthenticationResponse response = AuthenticationClient.GetResponse(resultCode_, intent);

                if (response.GetType() == AuthenticationResponse.Type.Token)
                {
                    Log.Debug("auth - token ", response.AccessToken);
                    string res = response.AccessToken;
                }
                else if (response.GetType() == AuthenticationResponse.Type.Error)
                {
                    if (response.Error == "NO_INTERNET_CONNECTION")
                    {
                        //androidContext.nonstopForms.launchNoInternetPage();
                    }
                }
                else
                {
                    Log.Debug("auth", "smth else");
                }

            }
        }
        public void connected()
        {
            
        }
        public void playTrack(System.String uri)
        {
            spotifyAppRemote.PlayerApi.Play("spotify:playlist:37i9dQZF1DX2sUQwD7tbmL");
        }
    }
    
    public class ConnectorConnectionListener : Java.Lang.Object, IConnectorConnectionListener
    {
        SpotifyAndroidCommunicator comm;
        public ConnectorConnectionListener(SpotifyAndroidCommunicator communicator)
        {
            this.comm = communicator;
        }

        public void OnConnected(SpotifyAppRemote remote)
        {
            comm.spotifyAppRemote = remote;
            comm.connected();
        }

        public void OnFailure(Throwable ex)
        {
            CouldNotFindSpotifyApp t = new CouldNotFindSpotifyApp();

            if (ex.GetType() == t.GetType())
            {
                comm.androidContext.nonstopForms.launchSpotifyConnectPage();
            }
        }
    }
}