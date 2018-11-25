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
    public partial class Page : ContentPage
    {
        public Page()
        {
            InitializeComponent();
        }
    }

    public class TrackCell: ViewCell
    {
        Label trackNameLabel, trackArtistNameLabel;

        public static readonly BindableProperty trackNameProperty =
            BindableProperty.Create("trackName", typeof(string), typeof(TrackCell), "name");

        public string trackName
        {
            get
            {
                return (string)GetValue(trackNameProperty);
            }
            set
            {
                SetValue(trackNameProperty, value);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext != null)
            {
                trackNameLabel.Text = trackName;
            }
        }


    }
}