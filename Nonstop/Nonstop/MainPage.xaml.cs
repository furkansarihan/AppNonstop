using Newtonsoft.Json;
using Nonstop.Spotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nonstop
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Nonstop.Forms.Spotify.track.json");
            
            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                System.Console.WriteLine(json);
                var data = JsonConvert.DeserializeObject<Track>(json);
            }
            
            
        }
    }
}
