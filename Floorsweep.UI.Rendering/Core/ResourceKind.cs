namespace Floorsweep.UI.Rendering.Core
{

    /// <summary>
    /// The kind of a <see cref="T:Veldrid.BindableResource" /> object.
    /// </summary>
    public enum ResourceKind : byte
    {
        UniformBuffer,
        StructuredBufferReadOnly,
        StructuredBufferReadWrite,
        TextureReadOnly,
        TextureReadWrite,
        Sampler,
    }
}
