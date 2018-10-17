using System.Collections.Generic;
using System.Linq;
using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;

namespace Floorsweep.UI.Rendering.Core
{
    internal class ResourceSet : IResourceSet
    {
        private readonly CommandList _cl;
        private readonly uint _slot;
        private readonly ResourceLayout _layout;
        private readonly Veldrid.ResourceSet _set;
        private readonly IEnumerable<IUpdateableDeviceBuffer> _buffers;

        public ResourceSet(CommandList cl,uint slot,ResourceLayout layout, Veldrid.ResourceSet set, IEnumerable<IUpdateableDeviceBuffer> buffers)
        {
            _cl = cl;
            _slot = slot;
            _layout = layout;
            _set = set;
            _buffers = buffers?.ToList() ?? new List<IUpdateableDeviceBuffer>();
        }

        public void Dispose()
        {
            _layout?.Dispose();
            _set?.Dispose();
            foreach (var buffer in _buffers)
            {
                buffer?.Dispose();
            }
        }

        public void Load()
        {
            _cl.SetGraphicsResourceSet(_slot,_set);
        }

        public IUpdateableDeviceBuffer this[string name] => _buffers.FirstOrDefault(b => b.Name == name);
    }
}