using Nonstop.Forms.Entity.Spotify;
using Nonstop.Spotify;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Forms.Service.Spotify
{
    class PlaylistService
    {
        static async Task<PagingObject<Playlist>> getUserPlaylists(string token, int limit = 20, int offset = 0)
        {
            string path = "me/playlists";
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add("limit", limit.ToString());
            queries.Add("offset", offset.ToString());
            return await SPTBaseService<PagingObject<Playlist>>.getAsync(path, queries, token);
        }

        static async Task<Playlist> getPlaylist(string token, string playlistID)
        {
            string path = String.Format("playlists/{0}", playlistID);

            return await SPTBaseService<Playlist>.getAsync(path, token);
        }

        static async Task<PagingObject<PlaylistTrack>> getPlaylistTracks(string token, string playlistID, int limit = 20, int offset = 0)
        {
            string path = String.Format("playlists/{0}/tracks", playlistID);
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add("limit", limit.ToString());
            queries.Add("offset", offset.ToString());
            return await SPTBaseService<PagingObject<PlaylistTrack>>.getAsync(path, queries, token);
        }
    }
}
