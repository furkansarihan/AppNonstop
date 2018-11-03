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
        Scene scene;
        Node plotNode;
        Camera camera;
        Octree octree;
        List<Piece> pieces;

        Xform runtimeData;

        public Bar SelectedBar { get; private set; }

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

            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            plotNode = scene.CreateChild();
            var baseNode = plotNode.CreateChild().CreateChild();
            var plane = baseNode.CreateComponent<StaticModel>();
            plane.Model = CoreAssets.Models.Plane;

            var cameraNode = scene.CreateChild();
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3(0, 0, -10);

            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 1000;
            light.Brightness = 1.3f;

            int size = 20;
            baseNode.Scale = new Vector3(size * 1.5f, 1, size * 1.5f);
            pieces = new List<Piece>(size * size);
            /*for (var i = 0f; i < size * 1.5f; i += 1.5f)
            {
                for (var j = 0f; j < size * 1.5f; j += 1.5f)
                {
                    var boxNode = plotNode.CreateChild();
                    boxNode.Position = new Vector3(size / 2f - i, 0, size / 2f - j);
                    var box = new Piece(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f));
                    boxNode.AddComponent(box);
                    pieces.Add(box);
                }
            }*/

            Node boxNode = plotNode.CreateChild();
            boxNode.Position = new Vector3(0, -2, 30);
            Piece box = new Piece(new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f));
            boxNode.AddComponent(box);
            //pieces.Add(box);

            //SelectedBar = bars.First();
            //SelectedBar.Select();

            try
            {
                await plotNode.RunActionsAsync(new EaseBackOut(new RotateBy(2f, 0, 360, 0)));
            }
            catch (OperationCanceledException) { }
            movementsEnabled = true;
        }

        void OnTouched(TouchEndEventArgs e)
        {
            Ray cameraRay = camera.GetScreenRay((float)e.X / Graphics.Width, (float)e.Y / Graphics.Height);
            var results = octree.RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry);
            if (results != null)
            {
                var bar = results.Value.Node?.Parent?.GetComponent<Bar>();
                if (SelectedBar != bar)
                {
                    SelectedBar?.Deselect();
                    SelectedBar = bar;
                    SelectedBar?.Select();
                }
            }
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }

        public void Rotate(float toValue)
        {
            plotNode.Rotate(new Quaternion(0, toValue, 0), TransformSpace.Local);
        }

        void SetupViewport()
        {
            var renderer = Renderer;
            var vp = new Viewport(Context, scene, camera, null);
            renderer.SetViewport(0, vp);
        }
        public void setXform(Xform data)
        {
            this.runtimeData = data;
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
        protected override void OnUpdate(float timeStep)
        {
            itself.SetWorldPosition(new Vector3(0,0,itself.WorldPosition.Z - timeStep * speed));
            if (itself.WorldPosition.Z < -5)
            {
                itself.SetWorldPosition(new Vector3(0, 0, 30));
                this.box.Color = new Color(RandomHelper.NextRandom(), RandomHelper.NextRandom(), RandomHelper.NextRandom(), 0.9f);
            }
        }
    }
    public class Bar : Component
    {
        Node barNode;
        Node textNode;
        Text3D text3D;
        Color color;
        float lastUpdateValue;

        public float Value
        {
            get { return barNode.Scale.Y; }
            set { barNode.Scale = new Vector3(1, value < 0.3f ? 0.3f : value, 1); }
        }

        public void SetValueWithAnimation(float value) => barNode.RunActionsAsync(new EaseBackOut(new ScaleTo(3f, 1, value, 1)));

        public Bar(Color color)
        {
            this.color = color;
            ReceiveSceneUpdates = true;
        }

        public override void OnAttachedToNode(Node node)
        {
            barNode = node.CreateChild();
            barNode.Scale = new Vector3(1, 0, 1); //means zero height
            var box = barNode.CreateComponent<Box>();
            box.Color = color;

            textNode = node.CreateChild();
            textNode.Rotate(new Quaternion(0, 180, 0), TransformSpace.World);
            textNode.Position = new Vector3(0, 10, 0);
            text3D = textNode.CreateComponent<Text3D>();
            text3D.SetFont(CoreAssets.Fonts.AnonymousPro, 60);
            text3D.TextEffect = TextEffect.Stroke;

            base.OnAttachedToNode(node);
        }

        protected override void OnUpdate(float timeStep)
        {
            var pos = barNode.Position;
            var scale = barNode.Scale;
            barNode.Position = new Vector3(pos.X, scale.Y / 2f, pos.Z);
            textNode.Position = new Vector3(0.5f, scale.Y + 0.2f, 0);
            var newValue = (float)Math.Round(scale.Y, 1);
            if (lastUpdateValue != newValue)
                text3D.Text = newValue.ToString("F01", CultureInfo.InvariantCulture);
            lastUpdateValue = newValue;
        }

        public void Deselect()
        {
            barNode.RemoveAllActions();//TODO: remove only "selection" action
            barNode.RunActions(new EaseBackOut(new TintTo(1f, color.R, color.G, color.B)));
        }

        public void Select()
        {
            Selected?.Invoke(this);
            // "blinking" animation
            barNode.RunActions(new RepeatForever(new TintTo(0.3f, 1f, 1f, 1f), new TintTo(0.3f, color.R, color.G, color.B)));
        }

        public event Action<Bar> Selected;
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
