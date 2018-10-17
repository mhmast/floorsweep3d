using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using AssetPrimitives;
using Floorsweep.Resources;
using Floorsweep.UI.Rendering.Core;
using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;

namespace Floorsweep.UI.Rendering
{
    public class Renderer
    {
        private readonly IDrawable _scene;
        private readonly Dictionary<Type, BinaryAssetSerializer> _serializers = DefaultSerializers.Get();

        private Pipeline _pipeline;

        private IApplicationWindow Window { get; }

        public Renderer(IApplicationWindow window, IDrawable scene)
        {
            _scene = scene;
            Window = window;
            Window.GraphicsDeviceCreated += OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed += OnDeviceDestroyed;

        }

        private void OnGraphicsDeviceCreated(GraphicsDevice gd, ResourceFactory factory, Swapchain sc)
        {
            var cl = factory.CreateCommandList();
            CreateResources(factory, sc, cl);

            Window.Rendering += deltaSeconds => Draw(cl, gd, sc, deltaSeconds);
        }

        private void OnDeviceDestroyed()
        {
            _pipeline.Dispose();
        }

        private Shader LoadShader(ResourceFactory factory, string set, Veldrid.ShaderStages stage, string entryPoint)
        {
            var name = $"{set}-{stage.ToString().ToLower()}.{GetExtension(factory.BackendType)}";
            return factory.CreateShader(new ShaderDescription(stage, ResourceImporter.ReadEmbeddedAssetBytes(name,GetType().Assembly), entryPoint));
        }

        private static string GetExtension(GraphicsBackend backendType)
        {
            var isMacOS = RuntimeInformation.OSDescription.Contains("Darwin");

            return backendType == GraphicsBackend.Direct3D11
                ? "hlsl.bytes"
                : backendType == GraphicsBackend.Vulkan
                    ? "450.glsl.spv"
                    : backendType == GraphicsBackend.Metal
                        ? isMacOS ? "metallib" : "ios.metallib"
                        : backendType == GraphicsBackend.OpenGL
                            ? "330.glsl"
                            : "300.glsles";
        }

        private ShaderSetDescription DescribeShader(ResourceFactory factory)
        {
            var shaderSet = new ShaderSetDescription(
                new[]
                {
                    new VertexLayoutDescription(
                        new VertexElementDescription("Position", VertexElementSemantic.Position, VertexElementFormat.Float3),
                        new VertexElementDescription("Normal", VertexElementSemantic.Normal, VertexElementFormat.Float3))
                },
                new[]
                {
                    LoadShader(factory, "Default", Veldrid.ShaderStages.Vertex, "VS"),
                    LoadShader(factory, "Default", Veldrid.ShaderStages.Fragment, "FS")
                });
            return shaderSet;
        }

        private void CreateResources(ResourceFactory factory, Swapchain sc, CommandList cl)
        {
            var shaderSet = DescribeShader(factory);
            var resourcefactory = new VeldridResourceFactory(cl, factory);
            _scene.CreateResources(resourcefactory);

            var graphicsPipelineDescription = new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                new DepthStencilStateDescription(
                    false,
                    true,
                    ComparisonKind.Always),
                new RasterizerStateDescription(
                    FaceCullMode.None,
                    PolygonFillMode.Solid,
                    FrontFace.CounterClockwise,
                    false,
                    false),
                PrimitiveTopology.TriangleStrip,
                shaderSet,
                resourcefactory.Layouts.ToArray(),
                sc.Framebuffer.OutputDescription);
            _pipeline = factory.CreateGraphicsPipeline(graphicsPipelineDescription);
        }

        private void Draw(CommandList cl, GraphicsDevice gd, Swapchain swapChain, float deltaSeconds)
        {
            cl.Begin();
            cl.SetFramebuffer(gd.SwapchainFramebuffer);
            cl.ClearColorTarget(0, RgbaFloat.Cyan);
            cl.SetPipeline(_pipeline);

            _scene.Draw(deltaSeconds, new GraphicsPipeline(cl));


            cl.End();
            gd.SubmitCommands(cl);
            //GraphicsDevice.WaitForIdle();

            // Once commands have been submitted, the rendered image can be presented to the application window.
            gd.SwapBuffers(swapChain);

        }
    }
}
