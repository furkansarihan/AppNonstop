using System;
using System.Collections.Generic;
using System.Text;

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
        Scene scene;
        Node plotNode;
        Camera camera;
        Octree octree;

        Xform runtimeData;

        public Game(ApplicationOptions options = null) : base(options) { }

        protected override void Start()
        {
            base.Start();
            createScene();
        }
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }
        public void createScene()
        {
            
            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            plotNode = scene.CreateChild();
            var baseNode = plotNode.CreateChild().CreateChild();

            var plane = baseNode.CreateComponent<StaticModel>();
            plane.Model = CoreAssets.Models.Plane;
            
            var cameraNode = scene.CreateChild();
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3(10, 15, 10) / 1.75f;
            cameraNode.Rotation = new Quaternion(-0.121f, 0.878f, -0.305f, -0.35f);

            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 100;
            light.Brightness = 1.3f;
            
        }
        public void setXform(Xform data)
        {
            this.runtimeData = data;
        }
        
    }
}
