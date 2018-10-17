using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Floorsweep.Resources;
using Floorsweep.Resources.Interfaces;
using Floorsweep.UI.Rendering;
using Floorsweep.UI.Rendering.Core;
using Floorsweep.UI.Rendering.Interfaces;
using Floorsweep.UI.Rendering.ShaderObjects;
using Floorsweep.UI.Rendering.VertexTypes;
using FloorSweep.Engine;
using FloorSweep.Engine.Extensions;

namespace Floorsweep.Engine
{
    public class Scene : IDrawable
    {
        private readonly List<Material> _materials = new List<Material>();
        private IResourceSet _worldMaterialSet;
        private Camera _camera;
        private const string WorldBufferName = "World";
        private const string MaterialBufferName = "Material";
        public IList<GameObject> GameObjects {get;} = new List<GameObject>();
       
        public void AddGameObject(GameObject obj) => GameObjects.Add(obj);
        public Camera CreateCamera(IApplicationWindow window) => _camera??(_camera = new Camera(window));

        public static Scene FromImportedScene(IImportedScene importScene)
        {
            var scene = new Scene();
            scene._materials.Clear();
            scene._materials.AddRange(importScene.Materials.Select(CreateMaterial));

            foreach (var mesh in importScene.Meshes)
            {
                var obj = new GameObject
                {
                    RenderMesh =
                        new RenderMesh<VertexWithNormal>(mesh.Indices,
                            CreateVertices(mesh),mesh.Material.Index)
                };
                scene.AddGameObject(obj);
            }

            return scene;
        }

        private static IEnumerable<VertexWithNormal> CreateVertices(IImportedMesh mesh)
        {
            var verts = mesh.Vertices.ToList();
            var norms = mesh.Normals.ToList();
            for (var i = 0; i < verts.Count; i++)
            {
                yield return new VertexWithNormal(verts[i],norms[i]);
            }
        }

        private static Material CreateMaterial(IImportedMaterial material)
        {
            return new Material(
                0f,
                material.ColorAmbient,
                material.ColorDiffuse,
                material.ColorEmissive,
                material.ColorSpecular,
                material.ColorTransparent,
                material.Opacity,
                material.Reflectivity,
                0f,0f
            );
        }

        public void Dispose()
        {
            _worldMaterialSet?.Dispose();
            foreach (var gameObject in GameObjects)
            {
                gameObject.Dispose();
            }
        }

        public void CreateResources(IResourceFactory factory)
        {
            _camera?.CreateResources(factory);
            _worldMaterialSet = factory.BuildShaderResourceSet().AddShaderParam(
                WorldBufferName,
                ResourceKind.UniformBuffer,
                ShaderStages.Vertex,
                (uint)Unsafe.SizeOf<Matrix4x4>(),
                BufferUsage.UniformBuffer | BufferUsage.Dynamic)
                .AddShaderParam(MaterialBufferName,
                    ResourceKind.UniformBuffer,
                    ShaderStages.Vertex,
                    (uint)Unsafe.SizeOf<Material>(),
                    BufferUsage.UniformBuffer | BufferUsage.Dynamic)
                .Build();
            foreach (var gameObject in GameObjects)
            {
                gameObject.RenderMesh?.CreateResources(factory);
            }

        }

        public void Draw(float deltaSeconds, IGraphicsPipeline pipeline)
        {
            _camera?.Draw(deltaSeconds,pipeline);
            foreach (var gameObject in GameObjects)
            {
                _worldMaterialSet[WorldBufferName].Update(gameObject.WorldPositionMatrix);
                var materialBlittable = _materials[gameObject.RenderMesh.MaterialIndex];
                _worldMaterialSet[MaterialBufferName].Update(materialBlittable);
                _worldMaterialSet.Load();
                gameObject.RenderMesh.Draw(deltaSeconds,pipeline);
            }
        }
    }
}
