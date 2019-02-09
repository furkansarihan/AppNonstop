using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Entity.Spotify
{
    public class PagingObject<E>
    {
        [JsonProperty("href")]
        private string href { get; set; }
        [JsonProperty("items")]
        private E[] items { get; set; }
        [JsonProperty("limit")]
        private int limit { get; set; }
        [JsonProperty("next")]
        private string next { get; set; }
        [JsonProperty("offset")]
        private int offset { get; set; }
        [JsonProperty("previous")]
        private string previous { get; set; }
        [JsonProperty("total")]
        private int total { get; set; }
    }
}
