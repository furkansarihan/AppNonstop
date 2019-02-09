using Newtonsoft.Json;
using Nonstop.Forms.Entity.Spotify;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Spotify
{
    public class TrackList
    {
        [PrimaryKey]
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [SQLite.Ignore]
        [JsonProperty("tracks")]
        public PagingObject<PlaylistTrack> tracks { get; set; }
        [JsonProperty("images")]
        public Image[] images { get; set; }
    }
}
