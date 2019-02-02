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
using Nonstop.Forms.Pages;

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
            List<TrackList> albumList = new List<TrackList>();
            Track t;
            
            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                //System.Console.WriteLine(json);
                t = JsonConvert.DeserializeObject<Track>(json);
            }
            lst.RowHeight = 300;

            Spotify.Album a = new Spotify.Album(); //create an album for testing
            a.artists = null;
            a.id = "deneme album";

            a.tracks = new Track[10]; // initialize the track list of the album

            for (int i = 0; i < 10; i++)
                a.tracks[i] = t;

            albumList.Add(a);
            lst.ItemsSource = albumList; // add albums to listView

            //**************json serializer***********************

            //**************Content*******************************
            //Content = new StackLayout
            //{
                // Generate page here.
            //};
            //**************Content*******************************
            
        }
        
        /*public async void trackClickListener(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            if (myListView.SelectedItem is Album) {
                //open tracsPage
                // listener for custom cell view...
                var selectedAlbum = (Album)myListView.SelectedItem;
                await Navigation.PushAsync(new Forms.Pages.TrackListsPage(app, selectedAlbum)); //open tracksPage(add it navigation stack) and send it app ref and selected Album
            }
            else if(myListView.SelectedItem is Track)
            {
                //openTracPage
                var selectedTrack = (Track)myListView.SelectedItem;
                await Navigation.PushAsync(new Forms.Pages.TracksPage(app, selectedTrack));
            }
        }*/
    }
}
