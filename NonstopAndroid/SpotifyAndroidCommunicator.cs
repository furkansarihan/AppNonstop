﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Com.Spotify.Android.Appremote.Api;
using Com.Spotify.Protocol.Client;
using Com.Spotify.Protocol.Types;
using Java.Lang;

namespace NonstopAndroid
{
    class SpotifyAndroidCommunicator
    {
        public static System.String CLIENT_ID = "2c769d49a6154b23847770985bb19957";
        public static System.String REDIRECT_URI = "com.yourdomain.yourapp://callback";
        public SpotifyAppRemote mSpotifyAppRemote;
        public Android.Content.Context androidContext;

        public SpotifyAndroidCommunicator(System.Object ac)
        {
            this.androidContext = (Android.Content.Context) ac;
        }
        // start
        public void start()
        {
            ConnectionParams connectionParams =
                new ConnectionParams.Builder(CLIENT_ID)
                        .SetRedirectUri(REDIRECT_URI)
                        .ShowAuthView(true)
                        .Build();

            ConnectorConnectionListener cclistener = new ConnectorConnectionListener(this);
            
            // this function needs android context
            SpotifyAppRemote.Connect(androidContext, connectionParams, cclistener);
        }
        // stop
        public void stop()
        {
            SpotifyAppRemote.Disconnect(mSpotifyAppRemote);
        }

        public void connected()
        {
            Subscription.IEventCallback eventCallback = new EventCallback();

            mSpotifyAppRemote.PlayerApi.Play("spotify:user:spotify:playlist:37i9dQZF1DX2sUQwD7tbmL");

            // Event object implemeted in class
            mSpotifyAppRemote.PlayerApi.SubscribeToPlayerState().SetEventCallback(eventCallback);
        }
    }
    class EventCallback : Java.Lang.Object, Subscription.IEventCallback
    {

        //public IntPtr Handle => throw new NotImplementedException();
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnEvent(Java.Lang.Object p0)
        {
            Track track = ((PlayerState)p0).Track;
            throw new NotImplementedException();
        }
    }
    class ConnectorConnectionListener : Java.Lang.Object, IConnectorConnectionListener
    {
        private SpotifyAndroidCommunicator spotifyCommunicator;

        //public IntPtr Handle => throw new NotImplementedException();
        
        public ConnectorConnectionListener(SpotifyAndroidCommunicator sc)
        {
            this.spotifyCommunicator = sc;
        }
        public void Dispose() { }

        public void OnConnected(SpotifyAppRemote sar)
        {
            spotifyCommunicator.mSpotifyAppRemote = sar;
            spotifyCommunicator.connected();
        }

        public void OnFailure(Throwable p0)
        {
            throw new NotImplementedException();
        }
    }
}