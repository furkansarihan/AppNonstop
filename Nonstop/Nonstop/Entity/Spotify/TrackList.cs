using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Spotify
{
    public class TrackList
    {
        [PrimaryKey]
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [SQLite.Ignore]
        [JsonProperty("tracks")]
        public Track[] tracks { get; set; }
    }
}
