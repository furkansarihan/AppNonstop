using System;
using System.Collections.Generic;
using System.Text;

using Nonstop.Forms.Analysis;

namespace Nonstop.Forms.Game
{
    class Game
    {
        Xform runtimeData;
        Analyser analyser;
        // SpotifyRemoteApp remoteCommunicator;

        public Game(String trackId)
        {
            analyser = new Analyser(); // Generating analyser object
            if (!analyser.setupXform(runtimeData, trackId)) // Check if runtime data exist for spesified track
            {
                // Runtime data is not exist, sooo generate it..
                if (analyser.generateXform(trackId)) // Check for errors
                {
                    // Runtime data Succesfully generated and writed to file, so
                    analyser.setupXform(runtimeData, trackId); // runtimeData gets valid runtime data
                    // Create and warmup game assets..
                    // Make request to remoteApp object to play specified track
                    // syncronise the game time with time getting from spotify playerState
                    // then start GAMELOOP :)
                }
                else
                {
                    // Error happened when generating runtime data.
                }
            } 
        }
    }
}
