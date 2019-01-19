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
using Nonstop.Droid.Implementations;
using Nonstop.Forms.DataManagement;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidSQLite))]
namespace Nonstop.Droid.Implementations
{
    class AndroidSQLite : ISQLite
    {
        public SQLiteAsyncConnection GetConnection()
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            // Documents folder  
            var path = Path.Combine(documentsPath, DatabaseManager.DbFileName);
            //var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();

            // This is where we copy in our pre-created database
            if (File.Exists(path))
            {
                using (var binaryReader = new BinaryReader(Android.App.Application.Context.Assets.Open("Nonstop.db")))
                {
                    using (var binaryWriter = new BinaryWriter(new FileStream(path, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            binaryWriter.Write(buffer, 0, length);
                        }
                    }
                }
            }

            var conn = new SQLiteAsyncConnection(path);

            // Return the database connection  
            return conn;
        }

        void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            while (bytesRead >= 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }

    }
}