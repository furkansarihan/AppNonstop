using Nonstop.Forms.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Nonstop.Forms;
using Nonstop.Forms.SQLite;
using Nonstop.Forms.Network;
using Nonstop.Forms.Spotify;
using Color = Xamarin.Forms.Color;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Nonstop
{
    public partial class App : Xamarin.Forms.Application, ISPTConnectionEventReceiver
    {
        //public MainPage mainPageObject;
        public ContentPage mainPageObject;
        GamePage gamePage;
        NavigationPage navigationPage;
        public DataProvider dataProvider;
        public NetworkManager networkManager;
        public ISPTCommunicator spotifyCommunicator;

        bool gameActive = false;

        public App()
        {
            InitializeComponent();

            // Generating Objects and setting App references
            dataProvider = new DataProvider();
            networkManager = new NetworkManager();
            dataProvider.setAppReference(this);
            networkManager.setAppReference(this);

            spotifyCommunicator = DependencyService.Get<ISPTCommunicator>();
            spotifyCommunicator.registerEventHandler(this);
            
            initApplication();
        }
        public void initApplication()
        {
            /*bool connection = databaseManager.getSpotifyConnection().Result;
            if (connection)
            {
                
            }
            else
            {
                //
            }*/
            //MainPage = new NavigationPage(new SpotifyConnectionPage(this));
            MainPage = new LoadingPage("Connecting to Spotify");
            //MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex("#FFFFFF"));
            MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.FromHex("#000000"));

        }
        public void launchPlaylistsPage()
        {
            mainPageObject = new Forms.TrackListsPage(this); // send reference of App objectC:\Users\enesk\Documents\GIT\AppNonstop\Nonstop\Nonstop\App.xaml.cs
            MainPage= new NavigationPage(mainPageObject);
           // MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex("#FFFFFF"));
            MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.FromHex("#000000"));
            
        }

        public void launchGame(string track_uri)
        {
            // launching game with track_uri
            gamePage = new GamePage(this, track_uri);
            MainPage = gamePage;
            gameActive = true;
        }

        public void launchSpotifyConnectPage()
        {
            MainPage = new SpotifyDownloadPage(this);
        }
        public async void launchResultPage(Nonstop.Forms.Game.Utils.GameResult result)
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

        public void onResult(object sender, SPTGatewayEventArgs args)
        {
           if( args.result == SPTConnectionResult.SpotifyNotFound)
            {

            }
           else if(args.result == SPTConnectionResult.Success)
            {
                if (gameActive)
                {

                }
                else
                {
                    launchPlaylistsPage();
                }
            }
        }
    }
}
