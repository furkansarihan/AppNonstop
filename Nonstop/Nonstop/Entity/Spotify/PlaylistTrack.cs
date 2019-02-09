using Newtonsoft.Json;
using Nonstop.Spotify;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Entity.Spotify
{
    public class PlaylistTrack
    {
        [JsonProperty("added_at")]
        public string addedAt { get; set; }
        [JsonProperty("added_by")]
        public User addedBy { get; set; }
        [JsonProperty("is_local")]
        public bool isLocal { get; set; }
        [JsonProperty("track")]
        public Track track { get; set; }        
    }
}
