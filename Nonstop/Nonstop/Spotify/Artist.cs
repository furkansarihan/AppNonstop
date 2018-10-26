using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Spotify
{
    class Artist
    {
        [JsonProperty("id")]
        private string id { get; set; }
        [JsonProperty("name")]
        private string name { get; set; }
    }
}
