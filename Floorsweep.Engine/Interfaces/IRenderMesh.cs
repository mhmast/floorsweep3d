using System;
using Floorsweep.UI.Rendering.Interfaces;

namespace FloorSweep.Engine.Interfaces
{
    public interface IRenderMesh : IDrawable
    {
        int MaterialIndex { get; }
    }
}
