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
        private string addedAt { get; set; }
        [JsonProperty("added_by")]
        private User addedBy { get; set; }
        [JsonProperty("is_local")]
        private bool isLocal { get; set; }
        [JsonProperty("track")]
        private Track track { get; set; }        
    }
}
