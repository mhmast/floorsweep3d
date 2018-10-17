using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;

namespace Floorsweep.UI.Rendering.Core
{
    internal class GraphicsPipeline : IGraphicsPipeline
    {
        private readonly CommandList _cl;

        public GraphicsPipeline(CommandList cl)
        {
            _cl = cl;
        }

        public void DrawIndexed(uint indexCount)
        {
            _cl.DrawIndexed(indexCount);
        }
    }
}