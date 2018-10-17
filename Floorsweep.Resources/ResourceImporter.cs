using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Assimp;
using Floorsweep.Resources.Interfaces;

namespace Floorsweep.Resources
{
    public class ResourceImporter
    {
        public IImportedScene ImportSceneFromObj(Stream stream) => ImportScene(stream, "obj");

        public IImportedScene ImportSceneFrom3mf1(Stream stream) => ImportScene(stream, "3mf1");
        
        private static IImportedScene ImportScene(Stream stream, string formatHint)
        {

            using (var context = new AssimpContext())
            {
                LogStream.IsVerboseLoggingEnabled = true;
                new LogStream((msg, data) => Console.WriteLine(msg)).Attach();
                //    var scene = new Scene();
                //scene.Open(stream,FileFormat.Detect(stream,formatHint));

                var scene = context.ImportFileFromStream(stream,
                    PostProcessSteps.GenerateNormals | PostProcessSteps.FlipUVs | PostProcessSteps.FlipWindingOrder,
                    formatHint);
                return new ImportedScene(scene);
            }
        }

        public static Stream OpenEmbeddedAssetStream(string name, Assembly assembly) => assembly.GetManifestResourceStream(name);

        public static byte[] ReadEmbeddedAssetBytes(string name, Assembly assembly)
        {
            using (var stream = OpenEmbeddedAssetStream(name, assembly))
            {
                var bytes = new byte[stream.Length];
                using (var ms = new MemoryStream(bytes))
                {
                    stream.CopyTo(ms);
                    return bytes;
                }
            }
        }
    }
}
