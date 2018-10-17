using System.Numerics;
using Veldrid;

namespace Floorsweep.UI.Rendering.VertexTypes
{
    public struct VertexPositionColor2
    {
        public readonly Vector2 _position;
        public readonly RgbaFloat _color;

        public VertexPositionColor2(Vector2 position, RgbaFloat color)
        {
            _position = position;
            _color = color;
        }
    }
}