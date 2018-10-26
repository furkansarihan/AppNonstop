using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Spotify
{
    class Track
    {
        [JsonProperty("id")]
        private string id { get; set; }
        [JsonProperty("name")]
        private string name { get; set; }
        [JsonProperty("album")]
        private Album album { get; set; }
        [JsonProperty("artists")]
        private Artist[] artists { get; set; }

    }
}
