using Nonstop.Spotify;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms.SQLite.Spotify
{
    class TrackListDB : SPTBaseDB<TrackList>
    {
        protected override void delete(TrackList entity)
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

        protected override TrackList get(string id)
        {
            try
            {
                lock (locker)
                {
                    return _sqLiteConnection.Get<TrackList>(f => f.id == id);
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }
        }

        protected override List<TrackList> getAll()
        {
            try
            {
                lock (locker)
                {
                    return _sqLiteConnection.Table<TrackList>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }
        }

        protected override string getDatabaseName()
        {
            return "TrackList.db3";
        }

        protected override void initDatabase()
        {
            _sqLiteConnection.CreateTable<TrackList>();
        }

        protected override void insert(TrackList entity)
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

        protected override void update(TrackList entity)
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
