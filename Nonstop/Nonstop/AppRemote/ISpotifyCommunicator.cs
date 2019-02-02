using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.AppRemote
{
    /// <summary>
    /// Cross Spotify remote app connection class references
    /// to native appRemote Framework
    /// </summary>
    public interface ISpotifyCommunicator
    {
        //void start();
        //void stop();
        //void connected();
        void playTrack(String uri);
    }
}
