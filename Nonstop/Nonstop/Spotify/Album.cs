using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Spotify
{
    public class Album : TrackList
    {
        [JsonProperty("artists")]
        public Artist[] artists { get; set; }
    }
}
