using System;

namespace Floorsweep.UI.Rendering.Interfaces
{
    public interface IResourceSet : ILoadableResource, IDisposable
    {
        IUpdateableDeviceBuffer this[string name] { get; }
    }
}