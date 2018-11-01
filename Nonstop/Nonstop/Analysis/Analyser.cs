using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Analysis
{
    /*
     * This class generates and manages runtime data for Nonstop 
     * 
     **/
    class Analyser
    {
        /*
         * This function analyses and generates runtime data for game,
         * and writes this data to spesific path for next uses.
         * *Xform*: The name of runtime data format that readed in game
         * 
         * Input: track id for spesific Spotify Track
         * Output: bool value for result of operations.
         * 
         **/
        public bool generateXform(String trackId)
        {
            // Getting analyzer data from api.spotify request
            // Creating runtime data from api.spotify response json............
            // Writing data to path -> trackId.json
            return true;
        }

        /*
         * This function initilases a Xform object for given track.
         * Returnes false if trackId.json not exsist in path
         * 
         * Input1: Xform object pointer for caller *game* class, 
         * Input2: track id for spesific Spotify Track
         * Output: bool value for result of operations.
         **/
        public bool setupXform(Xform runtimeData, String trackId)
        {
            // Control path for existing generated runtime data
            // return false if do not exist
            
            runtimeData = new Xform(trackId);
            return true;
        }
    }
}
