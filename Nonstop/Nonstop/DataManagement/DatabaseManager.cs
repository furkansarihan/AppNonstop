using SQLite;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System;
using Nonstop.Spotify;
using Nonstop.Spotify.DatabaseObjects;
using System.Threading.Tasks;
using System.Diagnostics;
using Nonstop.Forms.Spotify.DatabaseObjects;

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
            setupDatabase();
            insertData();
        }
        public void setupDatabase()
        {
            sqliteconnection.CreateTableAsync<TrackList_db>();
            sqliteconnection.CreateTableAsync<Track_db>();
        }
        async void insertData()
        {
            //List<TrackList_db> li = await sqliteconnection.QueryAsync<TrackList_db>("select * from TrackList_db");
            
            // insert data
            TrackList_db t = new TrackList_db();
            t.id = "playlist1";
            t.name = "playplay";

            Track_db t1 = new Track_db();
            t1.id = "track1";
            t1.name = "Nonstop1";
            t1.tracklistid = "playlist1";

            Track_db t2 = new Track_db();
            t2.id = "track2";
            t2.name = "Nonstop2";
            t2.tracklistid = "playlist1";

            sqliteconnection.InsertAsync(t);
            sqliteconnection.InsertAsync(t1);
            sqliteconnection.InsertAsync(t2);

            Debug.WriteLine("asdfasdf");
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
            //return await sqliteconnection.QueryAsync<TrackList_db>("select * from TrackList_db");
            List<TrackList_db> li = sqliteconnection.QueryAsync<TrackList_db>("select * from TrackList_db").Result;
            return li;
        }
        public async Task<bool> getSpotifyConnection()
        {
            return false;
        }
        public async Task<List<Track_db>> getTracks(String uri)
        {
            //List<Track_db> li = await sqliteconnection.QueryAsync<Track_db>("select * from Track_db where tracklistid=?", uri);
            List<Track_db> li = sqliteconnection.QueryAsync<Track_db>("select * from Track_db").Result;
            List<Track_db> ret = new List<Track_db>();

            foreach (var l in li)
            {
                if (l.tracklistid == uri)
                {
                    ret.Add(l);
                }
            }

            return ret;
        }
    }
}
