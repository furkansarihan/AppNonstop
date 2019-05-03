using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using Com.Spotify.Android.Appremote.Api;
//using Com.Spotify.Android.Appremote.Api.Error;
using Java.Lang;
using Android.Util;
//using Com.Spotify.Sdk.Android.Authentication;
using Android.Content;
using CarouselView.FormsPlugin.Android;
using Plugin.CurrentActivity;

namespace Nonstop.Droid
{
    public delegate void ActivityResultEventHandler(int requestCode, Result resultCode, Intent intent);

    [Activity(Label = "Nonstop", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public App nonstopForms; // Xamarin.Forms reference of our app
        public event ActivityResultEventHandler onActivityResult;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // FFimageloader spesific code
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: false);

            // comm = new SpotifyAndroidCommunicator(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            nonstopForms = new App(); 
            LoadApplication(nonstopForms);
                
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            
            //change status bar color
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            
            //===========================virtual keys 
            int uiOptions = (int)Window.DecorView.SystemUiVisibility;
            
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            //====================================
            
            CarouselViewRenderer.Init();
        }
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnActivityResult(int requestCode, Result result, Intent intent)
        {
            base.OnActivityResult(requestCode, result, intent);
            onActivityResult(requestCode, result, intent);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
