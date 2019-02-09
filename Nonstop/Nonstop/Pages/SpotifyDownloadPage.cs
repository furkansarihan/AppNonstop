using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarouselView.FormsPlugin.Abstractions;
using Nonstop;
using Nonstop.Forms.ViewModels;
using Xamarin.Forms;
using Nonstop.Spotify;

namespace Nonstop.Forms
{
    public partial class SpotifyDownloadPage : ContentPage
    {
        
        App app; // Application reference

        public SpotifyDownloadPage(App appref)
        {
            InitializeComponent();
            app = appref;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }
        
    }
}
