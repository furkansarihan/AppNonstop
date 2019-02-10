using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Nonstop.Forms.Spotify;

namespace Nonstop.Forms.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SpotifyConnectionPage : ContentPage , ISPTConnectionEventReceiver
	{
        App app;
		public SpotifyConnectionPage (App appref)
		{
			InitializeComponent ();
            app = appref;


		}
        protected override void OnAppearing()
        {
            base.OnAppearing();

            app.spotifyCommunicator.connect();
        }

        public void onResult(object sender, SPTGatewayEventArgs args)
        {
            if (args.result == SPTConnectionResult.SpotifyNotFound)
            {
                Navigation.PushAsync(new SpotifyDownloadPage(app));
            }
            else if (args.result == SPTConnectionResult.Success)
            {
                Navigation.PushAsync(new TrackListsPage(app));
            }
            else if(args.result == SPTConnectionResult.UserNotAuthorized)
            {
                Navigation.PushAsync(new RetrySpotifyConnectionPage(app));
            }
        }
    }
}