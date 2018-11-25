using Nonstop.Forms.Game;
using Nonstop.Forms.Pages;
using System;
using System.Collections.Generic;
using Urho;
using Urho.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Nonstop
{
    public partial class App : Xamarin.Forms.Application
    {
        MainPage mainPageObject;
        NavigationPage navigationPage;
        GamePage gamePage;

        public App()
        {
            InitializeComponent();
            mainPageObject = new MainPage(this); // send reference of App object
            navigationPage = new NavigationPage(mainPageObject);
            MainPage = navigationPage;
        }
        public async void launchGame(string track_uri) {
            // launching game with track_uri
            gamePage = new GamePage(this, track_uri);
            MainPage = gamePage;
        }

        public async void launchResultPage(Nonstop.Forms.Game.GameResult result)
        {
            // mainPageObject.launchResultPage(result);
            mainPageObject = new MainPage(this); // send reference of App object
            navigationPage = new NavigationPage(mainPageObject);
            MainPage = navigationPage;
        }
        protected override void OnStart()
        {
            // Handle when your app starts
            // Making connection to appRemote
            // Generating response objects.
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            // Control in game thing...
            // urhoGame.inGamePause();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
