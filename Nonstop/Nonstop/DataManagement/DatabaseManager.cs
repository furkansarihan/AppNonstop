using SQLite;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System;
using Nonstop.Spotify;
using System.Threading.Tasks;
using System.Diagnostics;
using PCLStorage;
using System.IO;

namespace Nonstop.Forms.DataManagement
{
    public class DatabaseManager
    {
        public App app;
        static SQLiteConnection _sqLiteConnection;
        string dbName = "Nonstop.db3";

        public DatabaseManager()
        {
            IFolder localFolder = FileSystem.Current.LocalStorage;
            string projectDirectory = Path.Combine(localFolder.Path, "debug");

            _sqLiteConnection = DependencyService.Get<ISQLite>().getConnection(projectDirectory, dbName);

            setupDatabase();
            insertData();
        }
        public void setupDatabase()
        {
            _sqLiteConnection.CreateTable<TrackList>();
            _sqLiteConnection.CreateTable<Track>();
        }
        void insertData()
        {            
            // insert data
            TrackList t = new TrackList();
            t.id = "playlist1";
            t.name = "playplay";

            Track t1 = new Track();
            t1.id = "track1";
            t1.name = "Nonstop1";

            Track t2 = new Track();
            t2.id = "track2";
            t2.name = "Nonstop2";

            List<TrackList> trackLists = getAllPlaylists();
            if(trackLists.Count == 0)
            {
                _sqLiteConnection.Insert(t);
                _sqLiteConnection.Insert(t1);
                _sqLiteConnection.Insert(t2);
            }
        }
        public void setAppReference(App appref)
        {
            app = appref;
        }
        public List<TrackList> getAllPlaylists()
        {
            // users id defined in app staticly
            // but in this method we just query all
            // playlists on the playlist table
            // ...
            //return await sqliteconnection.QueryAsync<TrackList_db>("select * from TrackList_db");
            
            List<TrackList> liste = _sqLiteConnection.Table<TrackList>().ToList();

            return liste;
        }
        public bool getSpotifyConnection()
        {
            return false;
        }
        public List<Track> getTracks(String uri)
        {
            //List<Track_db> li = await sqliteconnection.QueryAsync<Track_db>("select * from Track_db where tracklistid=?", uri);
            List<Track> li = _sqLiteConnection.Query<Track>("select * from Track");
            List<Track> ret = new List<Track>();

            foreach (var l in li)
            {
                if (l.id == uri)
                {
                    ret.Add(l);
                }
            }

            return ret;
        }
    }
}
