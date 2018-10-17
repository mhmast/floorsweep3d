using System.Numerics;
using Assimp;

//using Assimp;

namespace Floorsweep.Resources.Extensions
{
    internal static class PrimitiveExtensions
    {
        public static Vector3 ToVector3(this Vector3D vec)
        {
            return new Vector3(vec.X, vec.Y, vec.Z);
        }

        public static Vector4 ToVector4(this Color4D vec)
        {
            return new Vector4(vec.R, vec.G, vec.B, vec.A);
        }

        //public static Vector4 ToVector4(this Aspose.ThreeD.Utilities.Vector4 vec)
        //{
        //    return new Vector4((float)vec.x, (float)vec.y, (float)vec.z, (float)vec.w);
        //}

        //public static Vector4 ToVector4(this Aspose.ThreeD.Utilities.Vector3 vec)
        //{
        //    return new Vector4((float)vec.x, (float)vec.y, (float)vec.z, 1f);
        //}

        //public static Vector3 ToVector3(this Aspose.ThreeD.Utilities.Vector4 vec)
        //{
        //    return new Vector3((float)vec.x, (float)vec.y, (float)vec.z);
        //}
    }
}
