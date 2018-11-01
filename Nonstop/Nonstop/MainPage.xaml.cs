using Newtonsoft.Json;
using Nonstop.Spotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Urho;
using Urho.Forms;

using Nonstop.Forms.Game;

namespace Nonstop
{
    public partial class MainPage : ContentPage
    {
        UrhoSurface urhoSurface;
        Game urhoGame;
        GameManager gameManager;

        public MainPage()
        {
            InitializeComponent();

            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Nonstop.Forms.Spotify.track.json");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                System.Console.WriteLine(json);
                var data = JsonConvert.DeserializeObject<Track>(json);
            }

            // Urho Surface
            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = new StackLayout
            {
                Padding = new Thickness(1, 1, 1, 1),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { urhoSurface }
            };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            urhoGame = await urhoSurface.Show<Game>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.Portrait });
            gameManager = new GameManager(urhoGame, "sampleTrackId");
        }
    }
}
