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
    public class Xform
    {
        public All data;
        public XData xdata;

        public Xform(Stream stream1, Stream stream2)
        {
            // Reads trackId.json from path and fills itself
            using (var reader = new System.IO.StreamReader(stream1))
            {
                var json = reader.ReadToEnd();
                this.data = JsonConvert.DeserializeObject<All>(json);
            }

            // Reading clasified data
            using (var reader = new System.IO.StreamReader(stream2))
            {
                var json = reader.ReadToEnd();
                this.xdata = JsonConvert.DeserializeObject<XData>(json);
            }

            fillSectionIndexes();
            setSegmentMillis();
        }

        public void fillSectionIndexes()
        {
            for (int i = 0; i < data.segments.Length; i++)
            {
                data.segments[i].index = xdata.xdatas[i].index;
            }
        }
        public void setSegmentMillis()
        {
            for (int i = 0; i < data.segments.Length - 1; i++)
            {
                data.segments[i].millis = data.segments[i + 1].getStartMillis() - data.segments[i].getStartMillis();
            }
        }
    }

    public class XData
    {
        [JsonProperty("xdatas")]
        public X[] xdatas { get; set; }
    }
    public class X
    {
        [JsonProperty("index")]
        public int index { get; set; }
    }

    public class All
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
    public class Beat
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
    public class Segment
    {
        [JsonProperty("start")]
        public double start { get; set; }
        [JsonProperty("pitches")]
        public double[] pitches { get; set; }
        [JsonProperty("index")]
        public int index { get; set; }
        [JsonProperty("millis")]
        public uint millis { get; set; }

        public uint getStartMillis()
        {
            return (uint)(start * (double)1000);
        }
        public int getIndex()
        {
            return index;
        }
    }
    public class Section
    {
        [JsonProperty("start")]
        private double start { get; set; }

        public uint getStartMillis()
        {
            return (uint)(start * (double)1000);
        }
    }
    public class Track
    {
        [JsonProperty("duration")]
        public double duration { get; set; }
        [JsonProperty("tempo")]
        public double tempo { get; set; }
    }
}
