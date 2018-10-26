using System;
using System.Collections.Generic;
using System.Text;

using Urho;
using Urho.Resources;
using Urho.Gui;
using System.Threading.Tasks;

namespace Nonstop
{
    public class UrhoApp : Application
    {

        Scene scene;
        Text coinsText;

        public Viewport Viewport { get; private set; }

        [Preserve]
        protected UrhoApp() : base(new ApplicationOptions(assetsFolder: "Data")
        {
            Height = 1024, Width = 576, Orientation = ApplicationOptions.OrientationType.Portrait
        }) { }
        
        static UrhoApp()
        {

        }
        protected override void Start()
        {
            base.Start();
            CreateScene();
            Input.SubscribeToKeyDown(e =>
            {
                if (e.Key == Key.Esc) Exit();
            });
        }
        protected override void Stop()
        {
            base.Stop();
        }
        protected override void Setup()
        {
            base.Setup();
        }
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }
        protected override void OnDeleted()
        {
            base.OnDeleted();
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        async void CreateScene()
        {
            scene = new Scene();
            scene.CreateComponent<Octree>();

            var physics = scene.CreateComponent<Urho.Physics.PhysicsWorld>();
            physics.SetGravity(new Vector3(0, 0, 0));

            // Camera
            var cameraNode = scene.CreateChild();
            cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
            cameraNode.CreateComponent<Camera>();
            Viewport = new Viewport(Context, scene, cameraNode.GetComponent<Camera>(), null);

            Renderer.SetViewport(0, Viewport);

            var zoneNode = scene.CreateChild();
            var zone = zoneNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-300.0f, 300.0f));
            zone.AmbientColor = new Color(1f, 1f, 1f);
            
            // Game logic cycle
            bool firstCycle = true;
            while (true)
            {
                await StartGame();
                firstCycle = false;
            }
        }
        async Task StartGame()
        {
            //UpdateCoins(0);
            //Player = new Player();
            //var aircraftNode = scene.CreateChild(nameof(Aircraft));
            //aircraftNode.AddComponent(Player);
            //var playersLife = Player.Play();
            //Enemies enemies = new Enemies(Player);
            //scene.AddComponent(enemies);
            //SpawnCoins();
            //enemies.StartSpawning();
            //await playersLife;
            //enemies.KillAll();
            //aircraftNode.Remove();
        }
    }
}
