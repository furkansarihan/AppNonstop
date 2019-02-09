using Nonstop.Spotify;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.SQLite.Spotify
{
    class TrackDB : SPTBaseDB<Track>
    {
        protected override void delete(Track entity)
        {
            try
            {
                lock (locker)
                {
                    _sqLiteConnection.Delete(entity.id);
                }
            }
            catch (SQLiteException ex)
            {

            }
        }

        protected override Track get(string id)
        {
            try
            {
                lock (locker)
                {
                    return _sqLiteConnection.Get<Track>(f => f.id == id);
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }
        }

        protected override List<Track> getAll()
        {
            try
            {
                lock (locker)
                {
                    return _sqLiteConnection.Table<Track>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }
        }

        protected override string getDatabaseName()
        {
            return "Track.db3";
        }

        protected override void initDatabase()
        {
            _sqLiteConnection.CreateTable<Track>();
        }

        protected override void insert(Track entity)
        {
            try
            {
                lock (locker)
                {
                    _sqLiteConnection.Insert(entity);
                }
            }
            catch (SQLiteException ex)
            {

            }
        }

        protected override void update(Track entity)
        {
            try
            {
                lock (locker)
                {
                    _sqLiteConnection.Update(entity);
                }
            }
            catch (SQLiteException ex)
            {

            }
        }
    }
}
