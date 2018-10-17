using System;
using System.Collections.Generic;
using System.IO;
using AssetPrimitives;

namespace AssetProcessor
{
    class Program
    {
        private static readonly Dictionary<string, BinaryAssetProcessor> s_assetProcessors = GetAssetProcessors();
        private static readonly Dictionary<Type, BinaryAssetSerializer> s_assetSerializers = DefaultSerializers.Get();

        private static Dictionary<string, BinaryAssetProcessor> GetAssetProcessors()
        {
            var texProcessor = new ImageSharpProcessor();
            var assimpProcessor = new AssimpProcessor();

            return new Dictionary<string, BinaryAssetProcessor>()
            {
                { ".png", texProcessor },
                { ".ktx", new KtxFileProcessor() },
                { ".dae", assimpProcessor },
                //{ ".obj", assimpProcessor },
            };
        }

        static int Main(string[] args)
        {
            var outputDirectory = args[0];
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            for (var i = 1; i < args.Length; i++)
            {
                var arg = args[i];
                Console.WriteLine($"Processing {arg}");

                var extension = Path.GetExtension(arg);
                if (string.IsNullOrEmpty(extension))
                {
                    Console.Error.WriteLine($"Invalid path: {arg}");
                    return -1;
                }

                if (!s_assetProcessors.TryGetValue(extension, out var processor))
                {
                    Console.Error.WriteLine($"Unable to process asset with extension {extension}.");
                    return -1;
                }

                object processedAsset;
                using (var fs = File.OpenRead(arg))
                {
                    processedAsset = processor.Process(fs, extension);
                }

                var assetType = processedAsset.GetType();
                if (!s_assetSerializers.TryGetValue(assetType, out var serializer))
                {
                    Console.Error.WriteLine($"Unable to serialize asset of type {assetType}.");
                    return -1;
                }

                var fileName = Path.GetFileNameWithoutExtension(arg);
                var outputFileName = Path.Combine(outputDirectory, fileName + ".binary");
                using (var outFS = File.Create(outputFileName))
                {
                    var writer = new BinaryWriter(outFS);
                    serializer.Write(writer, processedAsset);
                }
                Console.WriteLine($"Processed asset: {arg} => {outputFileName}");
            }

            return 0;
        }
    }
}
