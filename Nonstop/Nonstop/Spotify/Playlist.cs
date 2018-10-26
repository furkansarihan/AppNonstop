using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Spotify
{
    class Playlist
    {
        [JsonProperty("id")]
        private string id { get; set; }
        [JsonProperty("tracks")]
        private Track[] tracks { get; set; }
        [JsonProperty("owner")]
        private User owner { get; set; }
    }
}
