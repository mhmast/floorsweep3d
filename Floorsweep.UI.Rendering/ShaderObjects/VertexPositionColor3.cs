using System.Numerics;

namespace Floorsweep.UI.Rendering.ShaderObjects
{
    public struct VertexPositionColor3
    {

        public readonly Vector3 _position;
        public readonly Vector4 _color;

        public VertexPositionColor3(Vector3 position, Vector4 color)
        {
            _position = position;
            _color = color;
        }
    }
}