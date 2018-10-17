using System.Numerics;
using ShaderGen;

namespace Floorsweep.UI.Rendering.ShaderObjects
{
    public struct VertexWithNormal
    {
        [PositionSemantic] public readonly Vector3 Position;
        [NormalSemantic]public readonly Vector3 Normal;

        public VertexWithNormal(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }
    }
}