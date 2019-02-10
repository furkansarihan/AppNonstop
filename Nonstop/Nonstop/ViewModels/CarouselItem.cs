using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Nonstop.Spotify;

namespace Nonstop.Forms.ViewModels
{
    public class CarouselItem : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public Color BackgroundColor { get; set; }
        public string Type { get; set; }
        public string ImageSrc { get; set; }
        public int Rotation { get; set; }

        double _position;
        public event PropertyChangedEventHandler PropertyChanged;

        public double Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CarouselPlaylistlItem : CarouselItem, INotifyPropertyChanged
    {
        public TrackList trackList { get; set; }
        public int trackCount { get; set; }
    }
    public class CarouselTracklistlItem : CarouselItem, INotifyPropertyChanged
    {
        public string artistName { get; set; }
        public Track track { get; set; }
    }
}
