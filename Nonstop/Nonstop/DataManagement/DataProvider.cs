using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nonstop.Forms.Spotify.DatabaseObjects;
using Nonstop.Spotify;
using Nonstop.Spotify.DatabaseObjects;


namespace Nonstop.Forms.DataManagement
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
        public async Task<List<TrackList_db>> getAllPLaylists()
        {
            // if internet connection here
            // -- refresh getplaylists functions on network ---
            

            // if no internet get form local database
            // return
            return await app.databaseManager.getAllPlaylists();
        }
        public async Task<List<Track_db>> getTracks(String uri)
        {
            return await app.databaseManager.getAllTracks(uri);
        }
    }
}
