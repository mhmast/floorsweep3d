using Floorsweep.UI.Rendering.Core;

namespace Floorsweep.UI.Rendering.Interfaces
{
    public interface IShaderResourceSetBuilder
    {
        IShaderResourceSetBuilder AddShaderParam(string name, ResourceKind resourceKind, ShaderStages shaderStages,
            uint size, BufferUsage usage);

        IResourceSet Build();
    }
}