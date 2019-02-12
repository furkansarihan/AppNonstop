using Nonstop.Forms.Game;
using Nonstop.Forms.Game.Utils;
using Nonstop.Forms.Service.Nonstop;
using Nonstop.Forms.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Urho;
using Urho.Forms;
using Xamarin.Forms;

namespace Nonstop.Forms.Pages
{
	public class GamePage : ContentPage
	{
        App app;
        UrhoSurface urhoSurface;
        Nonstop.Forms.Game.Game urhoGame;
        GameManager gameManager;
        GameData gameData;

        ISPTCommunicator spotifyConnection;
        string track_id;

        public GamePage (App application, string track_id)
		{
            this.app = application;
            this.track_id = track_id;
            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { urhoSurface }
            };

            this.launchGame();
        }

        public async void launchGame()
        {
            urhoGame = await urhoSurface.Show<Nonstop.Forms.Game.Game>(new ApplicationOptions("Data"));

            spotifyConnection = DependencyService.Get<ISPTCommunicator>();
            spotifyConnection.connectionEventHandler += connectionEventHandler;

            ISPTAuthentication auth = DependencyService.Get<ISPTAuthentication>();
            auth.tokenReady += async (object sender, TokenReceiverEventArgs args) =>
            {
                gameData = await NonstopService.getGameData(track_id, args.token);
                urhoGame.setGameData(gameData, spotifyConnection, track_id);
            };

            auth.authRequest(new string[] { });
            spotifyConnection.connect();
        }
        private void connectionEventHandler(object sender, SPTGatewayEventArgs e)
        {
            if (e.result == SPTConnectionResult.Success)
            {
                urhoGame.setHasConnection(true);
            }
        }

    }
}