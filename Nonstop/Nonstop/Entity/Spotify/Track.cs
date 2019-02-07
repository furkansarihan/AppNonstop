using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Spotify
{
    public class Track
    {
        [PrimaryKey]
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [SQLite.Ignore]
        [JsonProperty("album")]
        public Album album { get; set; }
        [SQLite.Ignore]
        [JsonProperty("artists")]
        public Artist[] artists { get; set; }
    }
}
