using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.Spotify.DatabaseObjects
{
    public class Track_db
    {
        [PrimaryKey]
        public string id { get; set; }
        public string name { get; set; }
        public string tracklistid { get; set; }
    }
}
