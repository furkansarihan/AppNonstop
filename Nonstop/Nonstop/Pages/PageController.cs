using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Nonstop.Forms.Pages
{
    class PageController
    {   

        private void bindData()
        {
            tracks.Add(new Spotify.Track
            {
                name = "deneme"
            });

        }

        public PageController()
        {
            bindData();
        }

        private ObservableCollection<Spotify.Track> _tracks;

        public ObservableCollection<Spotify.Track> tracks
        {
            get
            {
                if (_tracks == null)
                    _tracks = new ObservableCollection<Spotify.Track>();
                return _tracks;
            }

            set
            {
                _tracks = value;
            }
        }

    }
}
