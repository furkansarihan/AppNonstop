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
        public static async Task<PagingObject<PlaylistSimplified>> getUserPlaylists(string token, int limit = 20, int offset = 0)
        {
            string path = "me/playlists";
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add("limit", limit.ToString());
            queries.Add("offset", offset.ToString());
            return await SPTBaseService<PagingObject<PlaylistSimplified>>.getAsync(path, queries, token);
        }

        public static async Task<Playlist> getPlaylist(string token, string playlistID)
        {
            string path = String.Format("playlists/{0}", playlistID);

            return await SPTBaseService<Playlist>.getAsync(path, token);
        }

        public static async Task<PagingObject<PlaylistTrack>> getPlaylistTracks(string token, string playlistID, int limit = 20, int offset = 0)
        {
            string path = String.Format("playlists/{0}/tracks", playlistID);
            Dictionary<string, string> queries = new Dictionary<string, string>();
            queries.Add("limit", limit.ToString());
            queries.Add("offset", offset.ToString());
            return await SPTBaseService<PagingObject<PlaylistTrack>>.getAsync(path, queries, token);
        }

        public static async Task<List<Image>> getPlaylistCoverImage(string token, string playlistID)
        {
            string path = String.Format("playlists/{0}/images", playlistID);
            return await SPTBaseService<List<Image>>.getAsync(path, token);
        }

    }
}
