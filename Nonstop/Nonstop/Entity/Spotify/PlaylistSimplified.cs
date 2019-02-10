using Newtonsoft.Json;
using Nonstop.Spotify;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Entity.Spotify
{
    public class PlaylistSimplified : TrackList
    {
        [Ignore]
        [JsonProperty("owner")]
        private User owner { get; set; }
        [JsonProperty("tracks")]
        public PlaylistSimplifiedTrack tracks { get; set; }
        // SQLite indexes
        [Indexed]
        private string ownerID { get; set; }
    }
}
