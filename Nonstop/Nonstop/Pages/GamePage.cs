using Nonstop.Forms.Game;
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

        string track_uri;

        public GamePage (App application, string track_uri)
		{
            this.app = application;
            this.track_uri = track_uri;
            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { urhoSurface }
            };

            this.launchGame();
        }

        public async void launchGame() {
            urhoGame = await urhoSurface.Show<Nonstop.Forms.Game.Game>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.Portrait });
            gameManager = new GameManager(this.app, ref urhoGame, this.track_uri); // track_id for test data
        }
        
    }
}