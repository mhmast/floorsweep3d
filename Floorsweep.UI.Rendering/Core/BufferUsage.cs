using System;

namespace Floorsweep.UI.Rendering.Core
{
    [Flags]
    public enum BufferUsage : byte
    {
        VertexBuffer = 1,
        IndexBuffer = 2,
        UniformBuffer = 4,
        StructuredBufferReadOnly = 8,
        StructuredBufferReadWrite = 16, // 0x10
        IndirectBuffer = 32, // 0x20
        Dynamic = 64, // 0x40
        Staging = 128, // 0x80
    }
}
