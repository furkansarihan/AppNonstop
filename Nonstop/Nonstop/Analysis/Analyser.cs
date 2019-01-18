using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream strea = assembly.GetManifestResourceStream("Nonstop.Forms.Analysis.Response." + trackId + ".json");

            using (var reader = new System.IO.StreamReader(strea))
            {
                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<All>(json);
                string toFile = JsonConvert.SerializeObject(data);
                // Write toFile -> "Nonstop.Forms.Analysis.Request." + trackId + ".json"
                // and setupXform() will find it.
                // and now we can delete trackId.json from Response folder
            }

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
        public bool setupXform(ref Xform runtimeData, String trackId)
        {
            // Control path for existing generated runtime data
            // return false if do not exist
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            // This check process now not work due to write to process file thing
            //Stream stream = assembly.GetManifestResourceStream("Nonstop.Forms.Analysis.Runtime." + trackId + ".json");
            Stream stream1 = assembly.GetManifestResourceStream("Nonstop.Forms.Analysis.Response." + trackId + ".json");

            Stream stream2 = assembly.GetManifestResourceStream("Nonstop.Forms.Analysis.Response." + trackId + "_xdata.json");

            if (stream1 == null || stream2 == null)
            {
               return false; // Track data not here
            }

            runtimeData = new Xform(stream1, stream2);
            return true;
        }
    }
}
