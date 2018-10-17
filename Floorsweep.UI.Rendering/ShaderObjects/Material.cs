using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Floorsweep.UI.Rendering.ShaderObjects
{
    [StructLayout(LayoutKind.Explicit,Size = 10*16)]
    public unsafe struct Material
    {
        [FieldOffset(16 * 0)] public readonly float BumpScaling;
        [FieldOffset(16 * 1)] public readonly Vector4 ColorAmbient;
        [FieldOffset(16 * 2)] public readonly Vector4 ColorDiffuse;
        [FieldOffset(16 * 3)] public readonly Vector4 ColorEmissive;
        [FieldOffset(16 * 4)] public readonly Vector4 ColorSpecular;
        [FieldOffset(16 * 5)] public readonly Vector4 ColorTransparent;
        [FieldOffset(16 * 6)] public readonly float Opacity;
        [FieldOffset(16 * 7)] public readonly float Reflectivity;
        [FieldOffset(16 * 8)] public readonly float Shininess;
        [FieldOffset(16 * 9)] public readonly float ShininessStrength;


        public Material(
            float bumpScaling,
            Vector4 colorAmbient,
            Vector4 colorDiffuse,
            Vector4 colorEmissive,
            Vector4 colorSpecular,
            Vector4 colorTransparent,
            float opacity,
            float reflectivity,
            float shininess,
            float shininessStrength)
        {
            BumpScaling = bumpScaling;
            ColorAmbient = colorAmbient;
            ColorDiffuse = colorDiffuse;
            ColorEmissive = colorEmissive;
            ColorSpecular = colorSpecular;
            ColorTransparent = colorTransparent;
            Opacity = opacity;
            Reflectivity = reflectivity;
            Shininess = shininess;
            ShininessStrength = shininessStrength;
        }

        //public MaterialBlittable GetBlittable()
        //{
        //    const int noProps = 10;
        //    var b = new MaterialBlittable
        //    {
        //        Data = new Vector4[noProps]
        //    };
        //    fixed (void* ptr = &BumpScaling)
        //    {
        //        fixed (void* destptr = &b.Data[0])
        //        {
        //            Unsafe.CopyBlock(destptr, ptr, (uint) Unsafe.SizeOf<Material>());
        //        }
        //    }


        //    return b;
        //}
    }

    //public struct MaterialBlittable
    //{
    //    public Vector4[] Data ;
    //}
}