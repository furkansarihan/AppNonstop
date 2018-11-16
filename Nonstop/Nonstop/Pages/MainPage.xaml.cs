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
        App app; // Application reference

        public MainPage(App appref)
        {
            InitializeComponent();
            this.app = appref; // getting app reference

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

            //**************Content*******************************
            Content = new StackLayout
            {
                // Generate page here.
            };
            //**************Content*******************************
            
        }
        
        public void trackClickListener(){ // listener for custom cell view...
            // app.launchGame(trackCell.Text);
            app.launchGame("06AKEBrKUckW0KREUWRnvT");
        }

    }
}
