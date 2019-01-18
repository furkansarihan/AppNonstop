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

namespace Nonstop.Forms.Game
{
    /*
     * This class includes all Urho3D code, NOT anything beyond that.
     * Everything about controlling syncronisation management will make on parent class
     * 
     **/
    class Game : Urho.Application
    {
        bool movementsEnabled;
        bool first = true;
        bool paused = false;
        bool hasXform = false;
        bool hasGameManager = false;

        GameManager gameManager;
        GameResult gameResult;

        Scene scene;
        Node plotNode;

        Node cameraNode;
        public static Camera camera;
        Viewport vp;

        Octree octree;
        Text timeText;
        Text durationText;
        Button pauseButton;
        Button resumeButton;
        Button endGameButton;
        List<Piece> pieces;

        Xform runtimeData;
        NonstopTime nonstopTime;
        
        public IEnumerable<Piece> Bars => pieces;
        float gameSpeed = 1.5f;

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
                var bar = results.Value.Node?.Parent?.GetComponent<Bar>();
            }
        }

        protected override void OnUpdate(float timeStep)
        {
            if (!this.paused) // not paused
            {
                base.OnUpdate(timeStep);
                // Update time 
                nonstopTime.updateTime();
                // Display time
                timeText.Value = nonstopTime.getDisplayableTime();
                // Move Camera
                cameraNode.SetWorldPosition(new Vector3(0, 0, ((nonstopTime.currentMillis) / 100) * gameSpeed));

                // Check for section change
                if (hasXform && runtimeData.data.isSectionChanged(nonstopTime.currentMillis))
                {
                    // Change background
                    vp.SetClearColor(new Color(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f)));
                }

                if (hasXform)
                {
                    double duration = runtimeData.data.getTrackDuration();
                    double currmil = nonstopTime.currentMillis;
                    if (currmil > duration)
                    {
                        this.endGame();
                    }
                }

                // This is for first launch...
                if (hasXform && first)
                {
                    this.setPieces();
                    this.first = false;
                }
            }
        }
        public void createUI()
        {
            // UI , Text
            timeText = new Text();
            timeText.SetPosition(250, 20);
            timeText.HorizontalAlignment = HorizontalAlignment.Center;
            timeText.SetFont(CoreAssets.Fonts.AnonymousPro, Graphics.Width / 10);
            timeText.SetColor(Color.White);
            
            // Pause Button
            pauseButton = new Button();
            //pauseButton.SetColor(new Color(0.2f, 0.2f, 0.7f));
            pauseButton.Texture = ResourceCache.GetTexture2D("UI/pause_button.png");
            pauseButton.SetPosition(20, 20);
            pauseButton.SetSize(128, 128);
            pauseButton.SubscribeToReleased(args => {
                this.inGamePause();
            });

            Text pauseText = new Text();
            pauseText.SetColor(Color.White);
            pauseButton.AddChild(pauseText);
            pauseText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            pauseText.Value = "Pause";

            // Resume Button
            resumeButton = new Button();
            resumeButton.SetColor(new Color(0.7f, 0.2f, 0.2f));
            resumeButton.SetPosition(250, 200);
            resumeButton.SetSize(300, 80); resumeButton.Visible = false;
            resumeButton.SubscribeToReleased(args => {
                this.inGameResume();
            });

            Text resumeText = new Text();
            resumeButton.AddChild(resumeText);
            resumeText.SetColor(Color.White);
            resumeText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            resumeText.Value = "Resume";

            // End Game Button
            endGameButton = new Button();
            endGameButton.SetColor(new Color(0.1f, 0.7f, 0.2f));
            endGameButton.SetPosition(250, 600);
            endGameButton.SetSize(100, 100); endGameButton.Visible = false;
            endGameButton.SubscribeToReleased(args => {
                this.endGame();
            });

            Text endGameButtonText = new Text();
            endGameButtonText.SetColor(Color.White);
            endGameButton.AddChild(endGameButtonText);
            endGameButtonText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            endGameButtonText.Value = "End Game";

            UI.Root.AddChild(resumeButton);
            UI.Root.AddChild(pauseButton);
            UI.Root.AddChild(endGameButton);
            UI.Root.AddChild(timeText);
        }

        public void createReferences()
        {
            var boxNode1 = this.cameraNode.CreateChild();
            boxNode1.Position = new Vector3(-(float)(Graphics.Width / 10) * 5 / (float) Graphics.Width, -0.2f, 3);
            boxNode1.SetScale(0.2f);
            boxNode1.SetWorldRotation(new Quaternion(0, 0, 45));
            Piece box1 = new Piece(new Color(1.0f, 1.0f, 1.0f, 0.4f));
            boxNode1.AddComponent(box1);

            var boxNode2 = this.cameraNode.CreateChild();
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
            boxNode4.AddComponent(box4);
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
                gameManager.end(this.gameResult);
            }
        }
        
        // This function includes a while loop that
        // finishes it's work when Xform is here
        void waitForXform(){
            while(!hasXform){
                this.setPieces();
            }
        }

        async void SetupViewport()
        {
            var renderer = Renderer;
            vp = new Viewport(Context, scene, camera, null);
            vp.SetClearColor(Color.Black);
            renderer.SetViewport(0, vp);
        }
        public async void setXform(Xform data)
        {
            this.runtimeData = data;
            this.hasXform = true;
        }
        public async void setGameManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.hasGameManager = true;
        }
        async void setPieces()
        {
            
            /*foreach (Beat b in runtimeData.data.beats)
            {
                var boxNode = plotNode.CreateChild();
                boxNode.Position = new Vector3(0, 2, b.getStartMillis() / 100);
                Piece box = new Piece(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f));
                boxNode.AddComponent(box);
            }*/

            // NEW BEATS
            var seg = new Segment();
            foreach (Beat b in runtimeData.data.beats)
            {
                foreach (var s in runtimeData.data.segments)
                {
                    seg = s;

                    var beat = b.getStartMillis() / 100;
                    var segm = s.getStartMillis() / 100;
                    if (beat <= segm)
                    {
                        break;
                    }
                }
                if ( seg.millis > 0)
                {
                    var boxNode = plotNode.CreateChild();
                    boxNode.Position = new Vector3((float)((seg.getIndex() - 2) * 0.5), -1, (seg.getStartMillis() / 100 ) * gameSpeed);
                    Piece box = new Piece(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f));
                    boxNode.AddComponent(box);
                }
                
            }
        }
    }
    public class Piece : Component
    {
        Node itself;
        StaticModel model;
        Color color;
        float speed;

        public Piece(Color color)
        {
            this.color = color;
            this.speed = 30;
            ReceiveSceneUpdates = true;
        }
        public override void OnAttachedToNode(Node node)
        {
            itself = node.CreateChild();
            itself.Scale = new Vector3(0.8f, 0.8f, 0.8f); //means zero height
            model = node.CreateComponent<StaticModel>();
            model.Model = Application.ResourceCache.GetModel("Models/Non.mdl");
            model.SetMaterial(Application.ResourceCache.GetMaterial("Materials/Non.xml"));
            model.CastShadows = true;
            
            base.OnAttachedToNode(node);
        }
        /*protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }*/
    }
}
