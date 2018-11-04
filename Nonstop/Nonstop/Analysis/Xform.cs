using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Nonstop.Forms.Analysis
{
    /*
     * This class includes playable runtime data and It's specified format 
     * 
     **/
    class Xform
    {
        public All data;
        public Xform(Stream stream)
        {
            // Reads trackId.json from path and fills itself
            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                this.data = JsonConvert.DeserializeObject<All>(json);
            }
        }
    }
    class All
    {
        [JsonProperty("beats")]
        public Beat[] beats { get; set; }
    }
    class Beat
    {
        [JsonProperty("start")]
        private double start { get; set; }
        [JsonProperty("duration")]
        private double duration { get; set; }
        [JsonProperty("confidence")]
        private double confidence { get; set; }

        public uint getStartMillis()
        {
            return (uint)(start * (double)1000);
        }
    }
}
