using System;
using System.Collections.Generic;

namespace Floorsweep.UI.Rendering.Interfaces
{
    public interface IResourceFactory
    {
        IShaderResourceSetBuilder BuildShaderResourceSet();
        ILoadableDeviceBuffer BuildIndexBufferUInt16(IEnumerable<ushort> indices);
        ILoadableDeviceBuffer BuildIndexBufferUInt32(IEnumerable<uint> indices);
        ILoadableDeviceBuffer BuildVertexBuffer<T>(IEnumerable<T> size) where T : struct;
    }
}
