using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Spotify
{
    public class TrackList
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("tracks")]
        public Track[] tracks { get; set; }
    }
}
