using System.IO;
using System.Numerics;
using Floorsweep.Engine;
using Floorsweep.Resources;
using Floorsweep.UI.Rendering;

namespace FloorSweep.UI
{
    static class Program
    {
        public static void Main()
        {
            var resource = ResourceImporter.ReadEmbeddedAssetBytes("FloorSweep.UI.Assets.Objects.robot.3mf", typeof(Program).Assembly);
            
            var importScene = new ResourceImporter().ImportSceneFrom3mf1(new MemoryStream(resource));
            var scene = Scene.FromImportedScene(importScene);

            var robot = scene.GameObjects[0];
            var window = new VeldridStartupWindow("Sample");
            var camera = scene.CreateCamera(window);
            camera.Position = new Vector3(robot.Position.X,robot.Position.Y,robot.Position.Z-100f);
            camera.LookAt(robot);
            var app = new Renderer(window, scene);
            window.Run();
        }
    }
}
