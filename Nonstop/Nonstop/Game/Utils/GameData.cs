using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Game.Utils
{
    public class GameData
    {
        [JsonProperty("analyser_version")]
        public string analyser_version { get; set; }
        [JsonProperty("index_size")]
        public int index_size { get; set; }
        [JsonProperty("item_size")]
        public int item_size { get; set; }
        [JsonProperty("items")]
        public Item[] items { get; set; }
    }
    public class Item
    {
        [JsonProperty("start")]
        public float start { get; set; }
        [JsonProperty("index")]
        public int index { get; set; }
    }
}
