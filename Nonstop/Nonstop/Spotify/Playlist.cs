using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Spotify
{
    class Playlist : TrackList
    {
        [JsonProperty("owner")]
        private User owner { get; set; }
    }
}
