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
        Slider musicControl;
        Game urhoGame;
        GameManager gameManager;

        public MainPage()
        {
            InitializeComponent();

            //**************json serializer***********************
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Nonstop.Forms.Spotify.track.json");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                //System.Console.WriteLine(json);
                var data = JsonConvert.DeserializeObject<Track>(json);
            }
            //**************json serializer***********************

            //**************UrhoSurface implementation************
            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            musicControl = new Slider(0, 400000, 0);
            musicControl.ValueChanged += musicControlChanced;

            Content = new StackLayout
            {
                
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { urhoSurface }
            };
            //**************UrhoSurface implementation************

            //**************Spotify AppRemote Connection**********
            
            //**************Spotify AppRemote Connection**********
        }

        private void musicControlChanced(object sender, ValueChangedEventArgs e)
        {
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            urhoGame = await urhoSurface.Show<Game>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.Portrait });
            gameManager = new GameManager(urhoGame, "36YCdzT57us0LhDmCYtrNE"); // track_id for test data
        }
    }
}
