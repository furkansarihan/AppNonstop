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
        bool hasXform = false;
        bool first = true;
        Scene scene;
        Node plotNode;
        Node cameraNode;
        Camera camera;
        Octree octree;
        Text timeText;
        List<Piece> pieces;

        Xform runtimeData;
        NonstopTime nonstopTime;
        
        public IEnumerable<Piece> Bars => pieces;

        [Preserve]
        public Game(ApplicationOptions options = null) : base(options) { }

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

            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            plotNode = scene.CreateChild();
            var baseNode = plotNode.CreateChild().CreateChild();
            var plane = baseNode.CreateComponent<StaticModel>();
            plane.Model = CoreAssets.Models.Plane;

            // Camera
            cameraNode = scene.CreateChild();
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3(0, 0, 0);

            // Light
            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 5000;
            light.Brightness = 1.3f;

            int size = 20;
            baseNode.Scale = new Vector3(size * 1.5f, 1, size * 1.5f);
            pieces = new List<Piece>(size * size);
            
            // UI
            timeText = new Text();
            timeText.HorizontalAlignment = HorizontalAlignment.Center;
            timeText.SetFont(CoreAssets.Fonts.AnonymousPro, Graphics.Width / 10);
            timeText.SetColor(Color.White);
            UI.Root.AddChild(timeText);
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
            base.OnUpdate(timeStep);
            // Refresh time 
            nonstopTime.refreshTime();
            // Display time
            timeText.Value = nonstopTime.getDisplayableTime();
            // Move Camera
            cameraNode.SetWorldPosition(new Vector3(0,0,nonstopTime.currentMillis / 100));

            if (hasXform && this.first)
            {
                this.setPieces();
                first = false;
            }
        }
        public void Rotate(float toValue)
        {
            plotNode.Rotate(new Quaternion(0, toValue, 0), TransformSpace.Local);
        }

        async void SetupViewport()
        {
            var renderer = Renderer;
            var vp = new Viewport(Context, scene, camera, null);
            renderer.SetViewport(0, vp);
        }
        public async void setXform(Xform data)
        {
            this.runtimeData = data;
            this.hasXform = true;
        }
        async void setPieces()
        {
            foreach (Beat b in runtimeData.data.beats)
            {
                var boxNode = plotNode.CreateChild();
                boxNode.Position = new Vector3(0, -2, b.getStartMillis() / 100);
                Piece box = new Piece(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f));
                boxNode.AddComponent(box);
            }
        }
    }
    public class Piece : Component
    {
        Node itself;
        Box box;
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
            itself.Scale = new Vector3(1, 1, 1); //means zero height
            this.box = itself.CreateComponent<Box>();
            this.box.Color = color;

            base.OnAttachedToNode(node);
        }
    }
    public class NonstopTime
    {
        uint startTime; // init position of NonstopTime object
        public uint currentMillis   { get; set; } // for calculate
        public string currentSecond { get; set; } // for display
        public string currentMinute { get; set; } // for display

        public NonstopTime(uint start)
        {
            this.startTime = start;
            refreshTime();
        }
        public void refreshTime()
        {
            this.currentMillis = Urho.Time.SystemTime - startTime;
            this.currentSecond = ((this.currentMillis / 1000) % 60).ToString();
            this.currentMinute = (this.currentMillis / 60000).ToString();
        }
        public string getDisplayableTime()
        {
            return this.currentMinute + ":" + this.currentSecond;
        }
    }
    public static class RandomHelper
    {
        static readonly Random random = new Random();

        /// <summary>
        /// Return a random float between 0.0 (inclusive) and 1.0 (exclusive.)
        /// </summary>
        public static float NextRandom() { return (float)random.NextDouble(); }

        /// <summary>
        /// Return a random float between 0.0 and range, inclusive from both ends.
        /// </summary>
        public static float NextRandom(float range) { return (float)random.NextDouble() * range; }

        /// <summary>
        /// Return a random float between min and max, inclusive from both ends.
        /// </summary>
        public static float NextRandom(float min, float max) { return (float)((random.NextDouble() * (max - min)) + min); }

        /// <summary>
        /// Return a random integer between min and max - 1.
        /// </summary>
        public static int NextRandom(int min, int max) { return random.Next(min, max); }
    }
}
