using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Entity.Spotify
{
    public class Image
    {
        [JsonProperty("href")]
        public int height { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
        [JsonProperty("width")]
        public int width { get; set; }
    }
}
