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

            List<Spotify.Track> list = new List<Spotify.Track>();
            Track t;
            
            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                //System.Console.WriteLine(json);
                t = JsonConvert.DeserializeObject<Track>(json);
            }

            for (int i = 0; i < 100; i++)
                list.Add(t);

            lst.ItemsSource = list;

            //**************json serializer***********************

            //**************Content*******************************
            //Content = new StackLayout
            //{
                // Generate page here.
            //};
            //**************Content*******************************
            
        }
        
        public void trackClickListener(object sender, ItemTappedEventArgs e)
        { 
            // listener for custom cell view...
            var myListView = (ListView)sender;
            var track = (Spotify.Track) myListView.SelectedItem;
            app.launchGame(track.id);
        }

    }
}
