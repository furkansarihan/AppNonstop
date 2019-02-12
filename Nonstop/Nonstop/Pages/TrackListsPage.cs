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
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Nonstop.Forms.Controls;
using Nonstop.Forms.Spotify;
using Nonstop.Forms.Service.Spotify;
using Nonstop.Forms.Entity.Spotify;

namespace Nonstop.Forms
{
    public partial class TrackListsPage : ContentPage 
    {
        private int _currentIndex;
        private List<Color> _backgroundColors = new List<Color>();
        ISPTAuthentication authentication;

        public Wrapper Wrapper { get; set; }
        App app; // Application reference

        public TrackListsPage(App appref)
        {
            InitializeComponent();
            app = appref;

            Wrapper = new Wrapper
            {
                Items = new List<CarouselItem>()
            };          
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            
            // Need to start somewhere...
            if (_backgroundColors.GetCount() > 0)
            {
                page.BackgroundColor = _backgroundColors.First();
            }

            authentication = DependencyService.Get<ISPTAuthentication>();
            authentication.tokenReady += tokenReceived;
            authentication.authRequest(new string[] { "streaming" });
        }

        private async void tokenReceived(object sender, TokenReceiverEventArgs e)
        {
            string token = e.token;

            PagingObject<PlaylistSimplified> playlists = await PlaylistService.getUserPlaylists(token);
            TrackList[] list = playlists.items;
            addTrackListsToCards(list);
            authentication.tokenReady -= tokenReceived;
        }

        public void Handle_PositionSelected(object sender, PositionSelectedEventArgs e)
        {
            _currentIndex = e.NewValue;
            Wrapper.SlidePosition = 0;
        }

        public void Handle_Scrolled(object sender, CarouselView.FormsPlugin.Abstractions.ScrolledEventArgs e)
        {
            int position = 0;

            if (e.Direction == ScrollDirection.Right)
                position = (int)((_currentIndex * 100) + e.NewValue);
            
            else if (e.Direction == ScrollDirection.Left)
                position = (int)((_currentIndex * 100) - e.NewValue);

            // Set the background color of our page to the item in the color gradient
            // array, matching the current scrollindex.
            if (position > _backgroundColors.Count - 1)
                page.BackgroundColor = _backgroundColors.Last();
            else if (position < 0)
                page.BackgroundColor = _backgroundColors.First();
            else
                page.BackgroundColor = _backgroundColors[position];

            // Save the current scroll position
            Wrapper.SlidePosition = e.NewValue;

            if (e.Direction == ScrollDirection.Right)
            {
                // When scrolling right, we offset the current item and the next one.
                Wrapper.Items[_currentIndex].Position = -Wrapper.SlidePosition;

                if (_currentIndex < Wrapper.Items.Count - 1)
                {
                    Wrapper.Items[_currentIndex + 1].Position = 100 - Wrapper.SlidePosition;
                }
            }
            else if (e.Direction == ScrollDirection.Left)
            {
                // When scrolling left, we offset the current item and the previous one.
                Wrapper.Items[_currentIndex].Position = Wrapper.SlidePosition;

                if (_currentIndex > 0)
                {
                    Wrapper.Items[_currentIndex - 1].Position = -100 + Wrapper.SlidePosition;
                }
            }
        }

        // Create a list of all the colors in between our start and end color.
        public static IEnumerable<Color> SetGradients(Color start, Color end, int steps)
        {
            var colorList = new List<Color>();

            double aStep = ((end.A * 255) - (start.A * 255)) / steps;
            double rStep = ((end.R * 255) - (start.R * 255)) / steps;
            double gStep = ((end.G * 255) - (start.G * 255)) / steps;
            double bStep = ((end.B * 255) - (start.B * 255)) / steps;

            for (int i = 0; i < 100; i++)
            {
                var a = (start.A * 255) + (int)(aStep * i);
                var r = (start.R * 255) + (int)(rStep * i);
                var g = (start.G * 255) + (int)(gStep * i);
                var b = (start.B * 255) + (int)(bStep * i);

                colorList.Add(Color.FromRgba(r / 255, g / 255, b / 255, a / 255));
            }

            return colorList;
        }
        private void itemTapped(object sender, EventArgs e)
        {
            CarouselPlaylistlItem selectedCorouselItem = (CarouselPlaylistlItem)Wrapper.Items[_currentIndex];
            TrackList selectedPlaylist = selectedCorouselItem.trackList;
            Navigation.PushAsync(new TracksPage(app, selectedPlaylist.id));
        }
        private void search_clicked(object sender, EventArgs e)
        {
            SearchBar bar = new SearchBar { };
            bar.VerticalOptions = LayoutOptions.Start;
            bar.BackgroundColor = Color.White;

            
            page.Children.Add(bar);
            bar.TranslateTo(0, -50,0);
            bar.TranslateTo(0, 0,350,Easing.SinOut);
            
        }
        private void addTrackListsToCards(TrackList[] tracklist)
        {
            if (Wrapper.Items == null)
            {
                Wrapper.Items = new List<CarouselItem>();
            }
            foreach (var t in tracklist)
            {
                CarouselPlaylistlItem card = new CarouselPlaylistlItem();
                card.Title = t.name;
                card.Name = t.name;
                card.ImageSrc = t.images[0].url;
               // card.trackCount = t.tracks.items.Length;
                card.trackList = t;
                
                card.Position = 0;
                card.BackgroundColor = Color.FromHex("#F5F5F5");
                card.StartColor = Color.FromHex("#7B1FA2");
                card.EndColor = Color.FromHex("#4A148C");

                Wrapper.Items.Add(card);
            }
            
            this.BindingContext = Wrapper;

            // Create out a list of background colors based on our items colors so we can do a gradient on scroll.
            for (int i = 0; i < Wrapper.Items.Count; i++)
            {
                var current = Wrapper.Items[i];
                var next = Wrapper.Items.Count > i + 1 ? Wrapper.Items[i + 1] : null;

                if (next != null)
                    _backgroundColors.AddRange(SetGradients(current.BackgroundColor, next.BackgroundColor, 100));
                else
                    _backgroundColors.Add(current.BackgroundColor);
            }
        }
        
    }
}
