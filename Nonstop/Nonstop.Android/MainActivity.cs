using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace Nonstop.Droid
{
    [Activity(Label = "Nonstop", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        App nonstopForms; // Xamarin.Forms reference of our app.
        // SpotifyAndroidCommunicator comm; // Spotify communication object
        
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // comm = new SpotifyAndroidCommunicator(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            nonstopForms = new App(); 
            LoadApplication(nonstopForms);
        }
        protected override void OnStart()
        {
            base.OnStart();
            // comm.start();
        }
        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}