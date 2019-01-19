using Nonstop.Forms.Game;
using Nonstop.Forms.Pages;
using System;
using System.Collections.Generic;
using Urho;
using Urho.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Nonstop.Forms;
using Nonstop.Forms.DataManagement;
using Nonstop.Forms.Network;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Nonstop
{
    public partial class App : Xamarin.Forms.Application
    {
        //public MainPage mainPageObject;
        public ContentPage mainPageObject;
        GamePage gamePage;
        NavigationPage navigationPage;
        public DatabaseManager databaseManager;
        public DataProvider dataProvider;
        public NetworkManager networkManager;

        public App()
        {
            InitializeComponent();
            
            // Generating Objects and setting App references
            databaseManager = new DatabaseManager();
            dataProvider = new DataProvider();
            networkManager = new NetworkManager();
            databaseManager.setAppReference(this);
            dataProvider.setAppReference(this);
            networkManager.setAppReference(this);

            x();

            launchPlaylistsPage();
        }

        public async void x()
        {
            List<Nonstop.Spotify.DatabaseObjects.TrackList_db> l = await dataProvider.getAllPLaylists();

            var a = l;
        }
        public async void launchPlaylistsPage()
        {
            mainPageObject = new Forms.PlaylistsPage(this); // send reference of App object
            MainPage = new NavigationPage(mainPageObject);
        }
        public async void launchGame(string track_uri) {
            // launching game with track_uri
            gamePage = new GamePage(this, track_uri);
            MainPage = gamePage;
        }

        public async void launchResultPage(Nonstop.Forms.Game.GameResult result)
        {
            // mainPageObject.launchResultPage(result);
            //mainPageObject = new CarPage(this); // send reference of App object
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
