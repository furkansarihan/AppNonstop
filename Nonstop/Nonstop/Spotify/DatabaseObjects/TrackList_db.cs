using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Spotify.DatabaseObjects
{
    public class TrackList_db
    {
        [PrimaryKey]
        public string id { get; set; }
        public string name { get; set; }
    }
}