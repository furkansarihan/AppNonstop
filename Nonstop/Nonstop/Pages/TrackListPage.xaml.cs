using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Nonstop.Spotify;
using Nonstop;

namespace Nonstop.Forms.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TrackListPage : ContentPage
	{
        App app;
        Album album;

		public TrackListPage(App appref, Album selectedAlbum)
        {
			InitializeComponent ();
            this.app = appref;  // getting app reference
            this.album = selectedAlbum; //getting selected Album from mainpage album list

            List<Track> list = new List<Track>();

            foreach(Track t in album.tracks)
            {
                list.Add(t);
            }

            lst.ItemsSource = list;
        }

        private async void lst_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            var selectedTrack = (Spotify.Track)myListView.SelectedItem;
            await Navigation.PushAsync(new Forms.Pages.TrackPage(app, selectedTrack));
            //app.contentUpdateGame();
            //await Navigation.PopAsync(); // go back mainpage from TracsPage for Urho Content
            //app.launchGame(track.id);
        }
    }
}