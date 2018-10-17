using System;
using System.Collections.Generic;
using System.Linq;
using Floorsweep.UI.Rendering.Interfaces;
using FloorSweep.Engine.Interfaces;

namespace Floorsweep.Engine
{
    public class RenderMesh<TVertexType> : IRenderMesh where TVertexType : struct
    {
        private readonly uint _indexLength;
        private readonly Func<IResourceFactory, ILoadableDeviceBuffer> _createIndexBuffer;
        private readonly TVertexType[] _vertices;
        private ILoadableDeviceBuffer _indexBuffer;
        private ILoadableDeviceBuffer _vertexBuffer;

        public RenderMesh(IEnumerable<uint> indices, IEnumerable<TVertexType> vertices, int materialIndex)
        {
            MaterialIndex = materialIndex;
            var indicesArray = indices.ToArray();
            _indexLength = (ushort)indicesArray.Length;
            _createIndexBuffer = f => f.BuildIndexBufferUInt32(indicesArray);
            _vertices = vertices.ToArray();
        }

        public RenderMesh(IEnumerable<ushort> indices, IEnumerable<TVertexType> vertices, int materialIndex)
        {
            MaterialIndex = materialIndex;
            var indicesArray = indices.ToArray();
            _indexLength = (ushort)indicesArray.Length;
            _createIndexBuffer = f => f.BuildIndexBufferUInt16(indicesArray);
            _vertices = vertices.ToArray();
        }

        public int MaterialIndex { get; }

        public void Dispose()
        {
            _indexBuffer?.Dispose();
            _vertexBuffer?.Dispose();
        }

        public void CreateResources(IResourceFactory factory)
        {
            _indexBuffer = _createIndexBuffer(factory);
            _vertexBuffer = factory.BuildVertexBuffer(_vertices);
        }

        public void Draw(float deltaSeconds, IGraphicsPipeline pipeline)
        {
            _indexBuffer.Load();
            _vertexBuffer.Load();
            pipeline.DrawIndexed(_indexLength);
        }
    }
}