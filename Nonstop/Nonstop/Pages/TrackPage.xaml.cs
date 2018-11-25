using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Nonstop.Spotify;

namespace Nonstop.Forms.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TrackPage : ContentPage
	{
        App app;
        Track track;


        public TrackPage (App appref,Track selectedTrack)
		{
			InitializeComponent ();
            app = appref;
            track = selectedTrack;
            label.Text = track.name;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            app.launchGame(track.id);
        }
    }
}