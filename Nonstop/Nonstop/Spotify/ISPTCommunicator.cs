using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Forms.Spotify
{
    /// <summary>
    /// Cross Spotify remote app connection class references
    /// to native appRemote Framework
    /// </summary>
    public interface ISPTCommunicator
    {
        event EventHandler<SPTGatewayEventArgs> connectionEventHandler;

        void connect();
        void disconnect();
        void playTrack(string uri);
        void pause();
        void resume();
        void seekTo(long positionMs);
        Task<long> playerPosition();
        void registerEventHandler(ISPTConnectionEventReceiver receiver);
    }
    public enum SPTConnectionResult
    {
        Success, SpotifyNotFound, UserNotAuthorized, Unknown
    }
    public class SPTGatewayEventArgs : EventArgs
    {
        public SPTConnectionResult result { get; set; }
    }

    public interface ISPTConnectionEventReceiver 
    {
        void onResult(object sender, SPTGatewayEventArgs args);
    }
}
