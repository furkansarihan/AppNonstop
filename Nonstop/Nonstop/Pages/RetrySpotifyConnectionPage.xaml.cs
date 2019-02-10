using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nonstop.Forms.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RetrySpotifyConnectionPage : ContentPage
	{
        App app;
		public RetrySpotifyConnectionPage (App appref)
		{
			InitializeComponent ();
            app = appref;
		}
	}
}