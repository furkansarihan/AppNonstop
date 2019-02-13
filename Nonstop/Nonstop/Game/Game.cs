using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Shapes;

using Nonstop.Forms.Analysis;
using Nonstop.Forms.Game.Utils;
using Nonstop.Forms.Spotify;
using Xamarin.Forms;

namespace Nonstop.Forms.Game
{
    /*
     * This class includes all Urho3D code, NOT anything beyond that.
     * Everything about controlling syncronisation management will make on parent class
     * 
     **/
    public class Game : Urho.Application
    {
        bool first = true;
        bool paused = false;
        bool hasGameData = false;
        bool hasGameManager = false;
        bool hasSpotifyConnection = false;

        private string track_id;

        GameResult gameResult;
        ISPTCommunicator spotifyConnection;

        Scene scene;
        Node plotNode;

        Node cameraNode;
        public static Camera camera;
        Viewport vp;

        Octree octree;
        Text timeText;
        Text scoreText;
        Text nodeNumber;
        Urho.Gui.Button pauseButton;
        Urho.Gui.Button resumeButton;
        Urho.Gui.Button endGameButton;
        List<Piece> pieces;

        //Xform runtimeData;
        GameData runtimeData;
        NonstopTime nonstopTime;
        
        float gameSpeed = 1.0f;

        int screenHeight = (int)Xamarin.Forms.Device.Info.PixelScreenSize.Height;
        int screenWidth = (int)Xamarin.Forms.Device.Info.PixelScreenSize.Width;

        [Preserve]
        public Game() : base(new ApplicationOptions(assetsFolder: "Data") { Orientation = ApplicationOptions.OrientationType.Portrait }) { }

        [Preserve]
        public Game(ApplicationOptions opts) : base(opts) { }

        static Game()
        {
            UnhandledException += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                e.Handled = true;
            };
        }
        
        protected override void Start()
        {
            base.Start();
            CreateScene();
            SetupViewport();
        }
        
        async void CreateScene()
        {
            Input.SubscribeToTouchEnd(OnTouched);

            nonstopTime = new NonstopTime(Urho.Time.SystemTime);
            gameResult = new GameResult();
            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            plotNode = scene.CreateChild();
            var baseNode = plotNode.CreateChild().CreateChild();
            var plane = baseNode.CreateComponent<StaticModel>();
            plane.Model = CoreAssets.Models.Plane;

            // Camera
            cameraNode = scene.CreateChild();
            camera = cameraNode.CreateComponent<Camera>();
            camera.FarClip = 30.0f;
            cameraNode.Position = new Vector3(0, 0, 0);
            //cameraNode.Rotation = new Quaternion(0.0f, 0.0f, 0.0f);
            //cameraNode.Rotate(new Quaternion(15.0f, 0.0f, 0.0f));

            /*Node table = cameraNode.CreateChild();
            var obj = table.CreateComponent<Box>();
            table.AddComponent(obj);*/

            // Light
            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            light.Range = 50;
            light.Brightness = 1f;

            // Create skybox. The Skybox component is used like StaticModel, but it will be always located at the camera, giving the
            // illusion of the box planes being far away. Use just the ordinary Box model and a suitable material, whose shader will
            // generate the necessary 3D texture coordinates for cube mapping
            
            /*Node skyNode = scene.CreateChild("Sky");
            skyNode.SetScale(500.0f); // The scale actually does not matter
            Skybox skybox = skyNode.CreateComponent<Skybox>();
            var a = ResourceCache.GetResourceFileName("Models/Non.mdl");
            skybox.Model = ResourceCache.GetModel("Models/Non.mdl");
            skybox.SetMaterial(ResourceCache.GetMaterial("Materials/Skybox.xml"));*/

            int size = 20;
            baseNode.Scale = new Vector3(size * 1.5f, 1, size * 1.5f);
            pieces = new List<Piece>(size * size);
            this.createUI(); // Create User Interface
            //this.createReferences(); // Clickable references

            Input.SetMouseVisible(true, false);
        }

        void OnTouched(TouchEndEventArgs e)
        {
            Ray cameraRay = camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
            var results = octree.RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry);
            if (results != null)
            {
                if (results.Value.Node?.Position.Z - cameraNode.Position.Z < 150)
                {
                    results.Value.Node?.Remove();
                    System.Console.WriteLine("Node tapped.");
                }
            }
        }

        protected override async void OnUpdate(float timeStep)
        {
            if (!this.paused) // not paused
            {
                base.OnUpdate(timeStep);
                // Update time 
                nonstopTime.updateTime();
                // Display time
                timeText.Value = nonstopTime.getDisplayableTime();
                // Display Score
                scoreText.Value = gameResult.getUIScore();
                // Number of nodes
                nodeNumber.Value = plotNode.GetNumChildren().ToString();

                float f = 0;
                if (this.first == false)
                    f = Convert.ToSingle(await spotifyConnection.playerPosition()) / 100.0f * gameSpeed - 4.0f;

                // Move Camera
                if (this.first == false)
                    cameraNode.SetWorldPosition(new Vector3(0.0f, 0.0f, f));
                
                //cameraNode.SetWorldPosition(new Vector3(0, 3.4f, (loc * gameSpeed) - 1.0f));
                //loc += 0.3f;
                // Check for section change
                /*if (hasXform && runtimeData.data.isSectionChanged(nonstopTime.currentMillis))
                {
                    // Change background
                    vp.SetClearColor(new Color(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f)));
                }*/

                /*if (hasXform)
                {
                    double duration = runtimeData.data.getTrackDuration();
                    double currmil = nonstopTime.currentMillis;
                    if (currmil > duration)
                    {
                        this.endGame();
                    }
                }*/

                // This is for first launch...
                if (hasSpotifyConnection && hasGameData && first)
                {
                    this.first = false;
                    this.setPieces();
                    spotifyConnection.playTrack("spotify:track:" + track_id);
                    nonstopTime.refreshTime(Urho.Time.SystemTime);
                }
            }
        }
        public void createUI()
        {
            // UI , Text
            timeText = new Text();
            timeText.SetPosition(0, 0);
            timeText.HorizontalAlignment = HorizontalAlignment.Right;
            timeText.VerticalAlignment = VerticalAlignment.Bottom;
            timeText.SetFont(CoreAssets.Fonts.AnonymousPro, Graphics.Width / 10);
            timeText.SetColor(Urho.Color.White);

            // Score text
            scoreText = new Text();
            scoreText.SetPosition(0, 0);
            scoreText.HorizontalAlignment = HorizontalAlignment.Right;
            scoreText.VerticalAlignment = VerticalAlignment.Top;
            scoreText.SetFont(CoreAssets.Fonts.AnonymousPro, Graphics.Width / 10);
            scoreText.SetColor(Urho.Color.White);

            // Number of nodes displayed here
            nodeNumber = new Text();
            nodeNumber.SetPosition(0, 0);
            nodeNumber.HorizontalAlignment = HorizontalAlignment.Left;
            nodeNumber.VerticalAlignment = VerticalAlignment.Bottom;
            nodeNumber.SetFont(CoreAssets.Fonts.AnonymousPro, Graphics.Width / 10);
            nodeNumber.SetColor(Urho.Color.White);

            // Pause Button
            pauseButton = new Urho.Gui.Button();
            //pauseButton.SetColor(new Color(0.2f, 0.2f, 0.7f));
            pauseButton.Texture = ResourceCache.GetTexture2D("UI/pause_button.png");
            pauseButton.SetPosition(20, 20);
            pauseButton.SetSize(128, 128);
            pauseButton.SubscribeToReleased(args => {
                this.inGamePause();
            });

            Text pauseText = new Text();
            pauseText.SetColor(Urho.Color.White);
            pauseButton.AddChild(pauseText);
            pauseText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            pauseText.Value = "Pause";

            // Resume Button
            resumeButton = new Urho.Gui.Button();
            resumeButton.SetColor(new Urho.Color(0.7f, 0.2f, 0.2f));
            resumeButton.SetPosition(0, -(screenHeight / 4));
            resumeButton.HorizontalAlignment = HorizontalAlignment.Center;
            resumeButton.VerticalAlignment = VerticalAlignment.Center;
            resumeButton.SetSize(300, 80); resumeButton.Visible = false;
            resumeButton.SubscribeToReleased(args => {
                this.inGameResume();
            });

            Text resumeText = new Text();
            resumeButton.AddChild(resumeText);
            resumeText.SetColor(Urho.Color.White);
            resumeText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            resumeText.Value = "Resume";

            // End Game Button
            endGameButton = new Urho.Gui.Button();
            endGameButton.SetColor(new Urho.Color(0.1f, 0.7f, 0.2f));
            endGameButton.SetPosition(250, 600);
            endGameButton.SetSize(100, 100); endGameButton.Visible = false;
            endGameButton.SubscribeToReleased(args => {
                this.endGame();
            });

            Text endGameButtonText = new Text();
            endGameButtonText.SetColor(Urho.Color.White);
            endGameButton.AddChild(endGameButtonText);
            endGameButtonText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            endGameButtonText.Value = "End Game";

            UI.Root.AddChild(resumeButton);
            UI.Root.AddChild(pauseButton);
            UI.Root.AddChild(endGameButton);
            UI.Root.AddChild(timeText);
            UI.Root.AddChild(scoreText);
            UI.Root.AddChild(nodeNumber);
        }

        public void createReferences()
        {
            var boxNode1 = this.cameraNode.CreateChild();
            boxNode1.Position = new Vector3(0, 0, 0);
            boxNode1.Scale = new Vector3(0.8f, 0.8f, 0.8f);
            boxNode1.SetWorldRotation(new Quaternion(0, 0, 0));
            Piece box1 = new Piece(new Urho.Color(1.0f, 1.0f, 1.0f, 0.4f), gameResult, cameraNode , "Reference");
            boxNode1.AddComponent(box1);

            /*var boxNode1 = this.cameraNode.CreateChild();
            boxNode1.Position = new Vector3(-(float)(Graphics.Width / 10) * 5 / (float) Graphics.Width, -0.2f, 3);
            boxNode1.SetScale(0.2f);
            boxNode1.SetWorldRotation(new Quaternion(0, 0, 45));
            Piece box1 = new Piece(new Color(1.0f, 1.0f, 1.0f, 0.4f), gameResult, cameraNode);
            boxNode1.AddComponent(box1);

            /*var boxNode2 = this.cameraNode.CreateChild();
            boxNode2.Position = new Vector3(-(float)(Graphics.Width / 10) * 2 / (float)Graphics.Width, -0.4f, 3);
            boxNode2.SetScale(0.2f);
            Piece box2 = new Piece(new Color(1.0f, 1.0f, 1.0f, 0.4f));
            boxNode2.AddComponent(box2);

            var boxNode3 = this.cameraNode.CreateChild();
            boxNode3.Position = new Vector3((float)(Graphics.Width / 10) * 2 / (float)Graphics.Width, -0.4f, 3);
            boxNode3.SetScale(0.2f);
            Piece box3 = new Piece(new Color(1.0f, 1.0f, 1.0f, 0.4f));
            boxNode3.AddComponent(box3);

            var boxNode4 = this.cameraNode.CreateChild();
            boxNode4.Position = new Vector3((float)(Graphics.Width / 10) * 5 / (float)Graphics.Width, -0.2f, 3);
            boxNode4.SetScale(0.2f);
            boxNode4.SetWorldRotation(new Quaternion(0, 0, -45));
            Piece box4 = new Piece(new Color(1.0f, 1.0f, 1.0f, 0.4f));
            boxNode4.AddComponent(box4);*/
        }
        // Game paused outside of class
        // AppRemote or native app invokers will causes this function to run.
        public void inGamePause()
        {
            this.paused = true;
            nonstopTime.pauseStart(); // Stop Time

            resumeButton.Visible = true;
            endGameButton.Visible = true;
            pauseButton.Visible = false;
        }
        // Game will continue to play with start animation
        public void inGameResume()
        {
            endGameButton.Visible = false;
            resumeButton.Visible = false;
            pauseButton.Visible = true;

            nonstopTime.pauseEnd();
            this.paused = false; // continue 
        }
        // this function invokes a function outside of a class
        // and sends game result information to Nonstop
        public async void endGame()
        {
            // gameManager.end(Result gameresult);
            if (hasGameManager)
            {
                // gameManager.end(this.gameResult);
            }
        }

        void SetupViewport()
        {
            var renderer = Renderer;
            vp = new Viewport(Context, scene, camera, null);
            vp.SetClearColor(Urho.Color.Black);
            renderer.SetViewport(0, vp);
        }
        public async void setGameData(GameData data, ISPTCommunicator conn, string track_id)
        {
            this.runtimeData = data;
            this.spotifyConnection = conn;
            this.track_id = track_id;
            this.hasGameData = true;
        }
        public void setHasConnection(bool b)
        {
            this.hasSpotifyConnection = b;
        }
        async void setPieces()
        {
            int totalTap = 0;

            foreach (Item i in runtimeData.items)
            {
                var boxNode = plotNode.CreateChild();
                boxNode.Position = new Vector3((float)((i.index - 2) * 0.5f), -1, (i.start * 10.0f) * gameSpeed);
                Piece box = new Piece(new Urho.Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f), gameResult, cameraNode, "Non");
                boxNode.Scale = new Vector3(0.4f, 0.4f, 0.4f);
                boxNode.AddComponent(box); totalTap++;
            }

            this.gameResult.setTotalTap(totalTap);
        }
    }
    public class Piece : Component
    {
        Node _base;
        Node itself;
        StaticModel model;
        Urho.Color color;
        string modelName;
        float scaleFactor = 0.6f;

        GameResult gameResult;
        Node cameraNode;
        
        public Piece(Urho.Color color, GameResult gr, Node cn, string modelName)
        {
            this.color = color;
            this.gameResult = gr;
            this.cameraNode = cn;
            this.modelName = modelName;

            ReceiveSceneUpdates = true;
        }
        public override void OnAttachedToNode(Node node)
        {
            this._base = node;
            itself = node.CreateChild();
            itself.Scale = new Vector3(1.0f, 1.0f, 1.0f); //means zero height
            model = node.CreateComponent<StaticModel>();
            model.Model = Application.ResourceCache.GetModel("Models/" + modelName + ".mdl");
            model.SetMaterial(Application.ResourceCache.GetMaterial("Materials/" + modelName + ".xml"));
            
            base.OnAttachedToNode(node);
        }
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
            
            if (itself.Parent.Position.Z < cameraNode.Position.Z + 14.0f)
            {
                scaleFactor += 0.006f;
                _base.Scale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }

            /*if (itself.Parent.Position.Z < cameraNode.Position.Z)
            {
                itself.Parent.Remove();
                gameResult.incraseCurrentMiss();
                //Console.WriteLine("Node Removed");
            }*/
        }
    }
}
