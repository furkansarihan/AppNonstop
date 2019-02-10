using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Entity.Spotify
{
    public class PlaylistSimplifiedTrack
    {
        [JsonProperty("href")]
        public string href { get; set; }
        [JsonProperty("total")]
        public string total { get; set; }
    }
}
