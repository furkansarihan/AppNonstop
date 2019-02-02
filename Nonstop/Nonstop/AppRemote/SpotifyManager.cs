using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Nonstop.Forms.AppRemote
{
    public class SpotifyManager
    {
        // Play track with spesified uri
        public void playTrack(String uri)
        {
            DependencyService.Get<ISpotifyCommunicator>().playTrack(uri);
        }
    }
}
