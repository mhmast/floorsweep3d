using System;
using System.Numerics;
using Floorsweep.UI.Rendering.ShaderObjects;
using ShaderGen;

[assembly: ShaderSet("Default", "Floorsweep.UI.Rendering.Shaders.Default.VS", "Floorsweep.UI.Rendering.Shaders.Default.FS")]
namespace Floorsweep.UI.Rendering.Shaders
{
    public class Default
    {
        [ResourceSet(0)]
        public Matrix4x4 Projection;
        [ResourceSet(0)]
        public Matrix4x4 View;
        [ResourceSet(1)]
        public Matrix4x4 World;
        [ResourceSet(1)]
        public Material Material;


        [VertexShader]
        public FragmentInput VS(VertexWithNormal input)
        {
            FragmentInput output;
            var worldPosition = ShaderBuiltins.Mul(World, new Vector4(input.Position, 1));
            var viewPosition = ShaderBuiltins.Mul(View, worldPosition);
            var clipPosition = ShaderBuiltins.Mul(Projection, viewPosition);
            output.SystemPosition = clipPosition;
            output.Color = Material.ColorDiffuse;//new Vector4(0,0,0,0);

            return output;
        }

        [FragmentShader]
        public Vector4 FS(FragmentInput input)
        {
            return input.Color;
        }

        public struct FragmentInput
        {
            [SystemPositionSemantic] public Vector4 SystemPosition;
            [ColorSemantic] public Vector4 Color;
        }
    }
}
