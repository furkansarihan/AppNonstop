using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Util;
using Com.Spotify.Android.Appremote.Api;
using Com.Spotify.Android.Appremote.Api.Error;
using Com.Spotify.Sdk.Android.Authentication;
using Java.Lang;
using Nonstop.Droid;
using Nonstop.Droid.Spotify;
using Nonstop.Forms.Spotify;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidSPTCommunicator))]
namespace Nonstop.Droid.Spotify
{
    public delegate void SPTConnectedEventHandler(SpotifyAppRemote appRemote);
    public delegate void SPTFailedEventHandler(Throwable exception);

    public class AndroidSPTCommunicator : ISPTCommunicator
    {
        const int REQUEST_CODE = 1337;        
        public SpotifyAppRemote spotifyAppRemote;

        public event EventHandler<SPTGatewayEventArgs> connectionEventHandler;

        public AndroidSPTCommunicator()
        {
            
        }
        // start
        public void connect()
        {
            ConnectionParams connectionParams = new ConnectionParams.Builder(SPTCredentials.CLIENT_ID)
                                        .SetRedirectUri(SPTCredentials.REDIRECT_URI)
                                        .ShowAuthView(true)
                                        .Build();

            ConnectorConnectionListener listener = new ConnectorConnectionListener();
            listener.onSPTConnected += new SPTConnectedEventHandler(onConnected);
            listener.onSPTFailed += new SPTFailedEventHandler(onFailed);
            /* Connect to SPT APP */
            Context currentAndroidContext = CrossCurrentActivity.Current.Activity;
            SpotifyAppRemote.Connect(currentAndroidContext, connectionParams, listener);
        }
        // stop
        public void disconnect()
        {
            SpotifyAppRemote.Disconnect(spotifyAppRemote);
        }

        public void playTrack(string uri)
        {
            //"spotify:playlist:37i9dQZF1DX2sUQwD7tbmL"
            spotifyAppRemote.PlayerApi.Play(uri);
        }
        public void onConnected(SpotifyAppRemote appRemote)
        {
            spotifyAppRemote = appRemote;
            connectionEventHandler(this, new SPTGatewayEventArgs { result = SPTConnectionResult.Success });

            playTrack("spotify:playlist:37i9dQZF1DX2sUQwD7tbmL");
        }
        public void onFailed(Throwable exception)
        {
            Console.WriteLine(exception.Message);

            SPTGatewayEventArgs args = new SPTGatewayEventArgs();
            if(exception is CouldNotFindSpotifyApp)
            {
                args.result = SPTConnectionResult.SpotifyNotFound;
            }else if(exception is UserNotAuthorizedException)
            {
                args.result = SPTConnectionResult.UserNotAuthorized;
            } else
            {
                args.result = SPTConnectionResult.Unknown;
            }

            connectionEventHandler(this, args);
        }

        public void registerEventHandler(ISPTConnectionEventReceiver receiver)
        {
            connectionEventHandler += receiver.onResult;
        }
    }
    
    public class ConnectorConnectionListener : Java.Lang.Object, IConnectorConnectionListener
    { 
        public event SPTConnectedEventHandler onSPTConnected;
        public event SPTFailedEventHandler onSPTFailed;
        public ConnectorConnectionListener()
        {

        }

        public void OnConnected(SpotifyAppRemote remote)
        {
            onSPTConnected(remote);
        }

        public void OnFailure(Throwable ex)
        {
            onSPTFailed(ex);
        }
    }
}