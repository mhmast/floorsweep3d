using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;

namespace Floorsweep.UI.Rendering.Core
{
    internal class IndexBuffer : ILoadableDeviceBuffer
    {
        private readonly CommandList _cl;
        private readonly DeviceBuffer _buffer;
        private readonly IndexFormat _format;

        public IndexBuffer(CommandList cl, DeviceBuffer buffer, IndexFormat format)
        {
            _cl = cl;
            _buffer = buffer;
            _format = format;
        }

        public void Dispose()
        {
            _buffer?.Dispose();
        }

        public string Name => $"IndexBuffer ({_format})";
        public void Load()
        {
            _cl.SetIndexBuffer(_buffer, _format);
        }
    }
}