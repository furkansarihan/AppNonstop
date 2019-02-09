using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Nonstop.Droid.SQLite;
using Nonstop.Forms.SQLite;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidSQLiteConnection))]
namespace Nonstop.Droid.SQLite
{
    class AndroidSQLiteConnection : ISQLiteConnection
    {
        public SQLiteConnection getConnection(string path, string db)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var connectionPath = System.IO.Path.Combine(path, db);
            var connection = new SQLiteConnection(connectionPath);
            return connection;
        }
    }
}