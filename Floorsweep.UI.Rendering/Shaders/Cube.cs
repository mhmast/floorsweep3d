using System.Numerics;
using ShaderGen;

[assembly: ShaderSet("Cube", "Floorsweep.UI.Rendering.Shaders.Cube.VS", "Floorsweep.UI.Rendering.Shaders.Cube.FS")]

namespace Floorsweep.UI.Rendering.Shaders
{
    public class Cube
    {
        [ResourceSet(0)]
        public Matrix4x4 Projection;
        [ResourceSet(0)]
        public Matrix4x4 View;

        [ResourceSet(1)]
        public Matrix4x4 World;
        [ResourceSet(1)]
        public ShaderGen.Texture2DResource SurfaceTexture;
        [ResourceSet(1)]
        public SamplerResource SurfaceSampler;

        [VertexShader]
        public FragmentInput VS(VertexInput input)
        {
            FragmentInput output;
            var worldPosition = ShaderBuiltins.Mul(World, new Vector4(input.Position, 1));
            var viewPosition = ShaderBuiltins.Mul(View, worldPosition);
            var clipPosition = ShaderBuiltins.Mul(Projection, viewPosition);
            output.SystemPosition = clipPosition;
            output.TexCoords = input.TexCoords;

            return output;
        }

        [FragmentShader]
        public Vector4 FS(FragmentInput input)
        {
            return ShaderBuiltins.Sample(SurfaceTexture, SurfaceSampler, input.TexCoords);
        }

        public struct VertexInput
        {
            [PositionSemantic] public Vector3 Position;
            [TextureCoordinateSemantic] public Vector2 TexCoords;
        }

        public struct FragmentInput
        {
            [SystemPositionSemantic] public Vector4 SystemPosition;
            [TextureCoordinateSemantic] public Vector2 TexCoords;
        }
    }
}
