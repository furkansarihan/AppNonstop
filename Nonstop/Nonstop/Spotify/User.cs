using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Spotify
{
    public class User
    {
        [JsonProperty("id")]
        private string id { get; set; }
        [JsonProperty("name")]
        private string name { get; set; }
        [JsonProperty("product")]
        private string product { get; set; }
    }
}
