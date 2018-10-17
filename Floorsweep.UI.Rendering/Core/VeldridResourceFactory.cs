using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;

namespace Floorsweep.UI.Rendering.Core
{
    internal class VeldridResourceFactory : IResourceFactory
    {
        private readonly CommandList _cl;
        private readonly ResourceFactory _factory;
        private readonly IList<ShaderResourceSetBuilder> _buildersCreated = new List<ShaderResourceSetBuilder>();

        public VeldridResourceFactory(CommandList cl, ResourceFactory factory)
        {
            _cl = cl;
            _factory = factory;
        }

        public ResourceLayout[] Layouts => _buildersCreated.Select(b => b.Layout).ToArray();

        public IShaderResourceSetBuilder BuildShaderResourceSet()
        {
            var shaderResourceSetBuilder = new ShaderResourceSetBuilder((uint)_buildersCreated.Count,_cl,_factory);
            _buildersCreated.Add(shaderResourceSetBuilder);
            return shaderResourceSetBuilder;
        }

        public ILoadableDeviceBuffer BuildIndexBufferUInt16(IEnumerable<ushort> indices)
        {
            var indicesArray = indices.ToArray();
            var buffer = _factory.CreateBuffer(new BufferDescription((uint)Unsafe.SizeOf<ushort>() * (uint)indicesArray.Length, Veldrid.BufferUsage.IndexBuffer));
            _cl.UpdateBuffer(buffer,0,indicesArray);
            return new IndexBuffer(_cl, buffer, IndexFormat.UInt16);
        }

        public ILoadableDeviceBuffer BuildIndexBufferUInt32(IEnumerable<uint> indices)
        {
            var indicesArray = indices.ToArray();
            var buffer = _factory.CreateBuffer(new BufferDescription((uint)Unsafe.SizeOf<uint>() * (uint)indicesArray.Length, Veldrid.BufferUsage.IndexBuffer));
            _cl.UpdateBuffer(buffer, 0, indicesArray);
            return new IndexBuffer(_cl, buffer, IndexFormat.UInt32);
        }

        public ILoadableDeviceBuffer BuildVertexBuffer<T>(IEnumerable<T> vertices) where T : struct
        {
            var verticesArray = vertices.ToArray();
            var buffer = _factory.CreateBuffer(new BufferDescription((uint)Unsafe.SizeOf<T>() * (uint)verticesArray.Length, Veldrid.BufferUsage.VertexBuffer));
            _cl.UpdateBuffer(buffer,0,verticesArray);
            return new VertexBuffer(_cl, buffer);
        }
    }
}