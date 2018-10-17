using System;

namespace Floorsweep.UI.Rendering.Interfaces
{
    public interface IDeviceBuffer : IDisposable
    {
        string Name { get; }
    }
}