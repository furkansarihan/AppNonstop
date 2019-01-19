using SQLite;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System;
using Nonstop.Spotify;
using Nonstop.Spotify.DatabaseObjects;
using System.Threading.Tasks;

namespace Nonstop.Forms.DataManagement
{
    public class DatabaseManager
    {
        public App app;
        static SQLiteAsyncConnection sqliteconnection;
        public const string DbFileName = "Nonstop.db";

        public DatabaseManager()
        {
            sqliteconnection = DependencyService.Get<ISQLite>().GetConnection();
            sqliteconnection.CreateTableAsync<TrackList_db>();

            // insert data
            TrackList_db t = new TrackList_db();
            t.id = "idididid";
            t.name = "name name";

            sqliteconnection.InsertAsync(t);
            //sqliteconnection.UpdateAsync(t);
        }
        public void setAppReference(App appref)
        {
            app = appref;
        }
        public async Task<List<TrackList_db>> getAllPlaylists()
        {
            // users id defined in app staticly
            // but in this method we just query all
            // playlists on the playlist table
            // ...
            var query = sqliteconnection.Table<TrackList_db>().Where(t => t.name.StartsWith("t"));
            

            var result = await query.ToListAsync();

            return result;

            //return await sqliteconnection.ExecuteScalarAsync<List<TrackList>>("select * from [TrackList]");
        }
        public async Task<List<Track>> getAllTracks()
        {
            return await sqliteconnection.ExecuteScalarAsync<List<Track>>("select * from Track");
        }
    }
}
