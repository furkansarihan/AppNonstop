using System;
using System.Collections.Generic;
using System.Text;

using Nonstop.Forms.Analysis;
using Nonstop.Forms.Game.Utils;
using Nonstop.Forms.Spotify;
using Nonstop.Forms.Service.Nonstop;
using Xamarin.Forms;

namespace Nonstop.Forms.Game
{
    public class GameManager
    {
        public App app; // Main application reference
        GameData gameData;
        Game urhoGame;

        public GameManager(App appref, ref Game game, String trackId)
        {
            this.app = appref; // Getting main application reference
            this.urhoGame = game; // this.game points to Game object created in Main.xaml.cs

            

            /*
             
             runtimeData = app.dataProvider.getData(trackId, token);
             this.urhoGame.setXform(runtimeData);
             this.urhoGame.setGameManager(this);
             */
        }

        // public void end(Result gameresult);
        public async void end(Nonstop.Forms.Game.Utils.GameResult result)
        {
            app.launchResultPage(result);
        }
    }
}
