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
        int currentSection = 0;

        [JsonProperty("track")]
        public Track track { get; set; }
        [JsonProperty("beats")]
        public Beat[] beats { get; set; }
        [JsonProperty("segments")]
        public Segment[] segments { get; set; }
        [JsonProperty("sections")]
        public Section[] sections { get; set; }

        public bool isSectionChanged(uint millis)
        {
            for (int i = currentSection; i < sections.Length; i++)
            {
                if (sections[i].getStartMillis() <= millis)
                {
                    if (i != currentSection)
                    {
                        currentSection++;
                        return true;
                    }
                }
            }

            return false;
        }
        public uint getTrackDuration()
        {
            return (uint)(track.duration * (double)1000);
        }
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
    class Segment
    {
        [JsonProperty("start")]
        private double start { get; set; }
        [JsonProperty("pitches")]
        public double[] pitches { get; set; }

        public uint getStartMillis()
        {
            return (uint)(start * (double)1000);
        }
        public int getIndex()
        {
            int biggestI = -1;
            double biggestN = 0.99;

            for (int i = 0; i < pitches.Length; i++)
            {
                if (pitches[i] > biggestN)
                {
                    biggestN = pitches[i];
                    biggestI = i;
                }
            }

            return biggestI;
        }
    }
    class Section
    {
        [JsonProperty("start")]
        private double start { get; set; }

        public uint getStartMillis()
        {
            return (uint)(start * (double)1000);
        }
    }
    class Track
    {
        [JsonProperty("duration")]
        public double duration { get; set; }
        [JsonProperty("tempo")]
        public double tempo { get; set; }
    }
}
