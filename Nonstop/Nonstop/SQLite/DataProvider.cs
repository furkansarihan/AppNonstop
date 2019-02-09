using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nonstop.Spotify;


namespace Nonstop.Forms.SQLite
{
    public class DataProvider
    {
        App app;

        public DataProvider()
        {

        }
        public void setAppReference(App appref)
        {
            app = appref;
        }
        public async Task<List<TrackList>> getAllPLaylists()
        {
            // if internet connection here
            // -- refresh getplaylists functions on network ---


            // if no internet get form local database
            // return
            return null;
        }
        public async Task<List<Track>> getTracks(String uri)
        {
            return null;
        }

        public bool getSpotifyConnection()
        {
            return false;
        }
    }
}
