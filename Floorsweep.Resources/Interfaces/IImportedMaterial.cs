using System.Numerics;

namespace Floorsweep.Resources.Interfaces
{
    public interface IImportedMaterial
    {
        int Index { get; }
        string Name { get; }
        float Opacity { get; }
        float Shininess { get; }
        float Reflectivity { get; }
        Vector4 ColorDiffuse { get; }
        Vector4 ColorAmbient { get; }
        Vector4 ColorSpecular { get; }
        Vector4 ColorEmissive { get; }
        Vector4 ColorTransparent { get; }
        Vector4 ColorReflective { get; }
    }
}
