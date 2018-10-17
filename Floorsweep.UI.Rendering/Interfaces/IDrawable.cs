using System;

namespace Floorsweep.UI.Rendering.Interfaces
{
    public interface IDrawable : IDisposable
    {
        void CreateResources(IResourceFactory factory);
        void Draw(float deltaSeconds, IGraphicsPipeline pipeline);
    }
}
