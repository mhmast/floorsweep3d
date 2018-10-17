using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;

namespace Floorsweep.UI.Rendering.Core
{

    internal class ShaderParameterBuffer : IUpdateableDeviceBuffer
    {
        private readonly CommandList _cl;
        private readonly DeviceBuffer _buffer;

        public ShaderParameterBuffer(CommandList cl, DeviceBuffer buffer)
        {
            _cl = cl;
            _buffer = buffer;
        }

        public void Dispose()
        {
            _buffer?.Dispose();
        }

        public void Update<T>(T resource) where T : struct
        {
            _cl.UpdateBuffer(_buffer, 0, resource);
        }

        public string Name => _buffer.Name;
    }
}