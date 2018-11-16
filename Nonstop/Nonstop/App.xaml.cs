using Nonstop.Forms.Game;
using System;
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
        UrhoSurface urhoSurface;
        Game urhoGame;
        GameManager gameManager;

        public App()
        {
            InitializeComponent();
            mainPageObject = new MainPage(this); // send reference of App object
            MainPage = mainPageObject;
            setupGame();
        }

        public void setupGame()
        {
            //**************UrhoSurface implementation************
            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            //**************UrhoSurface implementation************
        }

        public async void launchGame(string track_uri){
            // launching game with track_uri
            urhoGame = await urhoSurface.Show<Game>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.Portrait });
            gameManager = new GameManager(this, ref urhoGame, track_uri); // track_id for test data
        }
        
        public void contentUpdateGame(){
            // Changing context for launching game
            mainPageObject.Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { urhoSurface }
            };
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
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
