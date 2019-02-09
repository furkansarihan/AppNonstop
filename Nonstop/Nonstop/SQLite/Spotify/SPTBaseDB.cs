using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Nonstop.Forms.SQLite.Spotify
{
    abstract class SPTBaseDB<E> : BaseDispose
    {
        protected SQLiteConnection _sqLiteConnection;
        protected object locker = new object();

        public SPTBaseDB()
        {
            IFolder localFolder = FileSystem.Current.LocalStorage;
            string projectDirectory = Path.Combine(localFolder.Path, "Spotify_DB");

            _sqLiteConnection = DependencyService.Get<ISQLiteConnection>().getConnection(projectDirectory, getDatabaseName());
        }
        protected abstract void initDatabase();
        protected abstract void insert(E entity);
        protected abstract void update(E entity);
        protected abstract void delete(E entity);
        protected abstract E get(string id);
        protected abstract List<E> getAll();
        protected abstract string getDatabaseName();
    }
}
