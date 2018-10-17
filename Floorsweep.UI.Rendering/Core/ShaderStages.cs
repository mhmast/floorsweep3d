using System;

namespace Floorsweep.UI.Rendering.Core
{
    [Flags]
    public enum ShaderStages : byte
    {
        None = 0,
        Vertex = 1,
        Geometry = 2,
        TessellationControl = 4,
        TessellationEvaluation = 8,
        Fragment = 16, // 0x10
        Compute = 32, // 0x20
    }
}
