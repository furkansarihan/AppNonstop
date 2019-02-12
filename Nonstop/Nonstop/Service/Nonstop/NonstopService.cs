using Nonstop.Forms.Game.Utils;
using Nonstop.Forms.Service.Spotify;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nonstop.Forms.Service.Nonstop
{
    class NonstopService
    {
        public static async Task<GameData> getGameData(string track_id, string token)
        {
            return await NonstopBaseService<GameData>.getAsync(track_id, token);
        }
    }
}
