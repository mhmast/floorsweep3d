using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;

namespace Floorsweep.UI.Rendering
{
    internal class VertexBuffer : ILoadableDeviceBuffer
    {
        private readonly CommandList _cl;
        private readonly DeviceBuffer _buffer;

        public VertexBuffer(CommandList cl, DeviceBuffer buffer)
        {
            _cl = cl;
            _buffer = buffer;
        }

        public void Dispose()
        {
            _buffer?.Dispose();
        }

        public string Name => "VertexBuffer";
        public void Load()
        {
            _cl.SetVertexBuffer(0,_buffer);
        }
    }
}