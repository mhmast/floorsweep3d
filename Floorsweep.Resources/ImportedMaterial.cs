using System.Numerics;

using Assimp;
using Floorsweep.Resources.Extensions;
using Floorsweep.Resources.Interfaces;

namespace Floorsweep.Resources
{
    public class ImportedMaterial : IImportedMaterial
    {
        private readonly Material _material;
        public int Index { get; }
        public string Name => _material.Name;

        //public ShadingMode ShadingMode => _material.ShadingMode;

        //public BlendMode BlendMode

        //{
        public ImportedMaterial(Material material, int index)
        {
            _material = material;
            Index = index;
        }

        //    get => _material.BlendMode;

        //    set => _material.BlendMode = value;

        //}


        public float Opacity => _material.Opacity;

        public float Shininess => _material.Shininess;

        public float Reflectivity => _material.Reflectivity;

        public Vector4 ColorDiffuse => _material.ColorDiffuse.ToVector4();

        public Vector4 ColorAmbient => _material.ColorAmbient.ToVector4();

        public Vector4 ColorSpecular => _material.ColorSpecular.ToVector4();

        public Vector4 ColorEmissive => _material.ColorEmissive.ToVector4();

        public Vector4 ColorTransparent => _material.ColorTransparent.ToVector4();

        public Vector4 ColorReflective => _material.ColorReflective.ToVector4();

        //public TextureSlot TextureDiffuse

        //{

        //    get => _material.t;

        //}


        //public TextureSlot TextureSpecular

        //{

        //    get => _material.TextureSpecular;

        //    set => _material.TextureSpecular = value;

        //}


        //public TextureSlot TextureAmbient

        //{

        //    get => _material.TextureAmbient;

        //    set => _material.TextureAmbient = value;

        //}


        //public TextureSlot TextureEmissive

        //{

        //    get => _material.TextureEmissive;

        //    set => _material.TextureEmissive = value;

        //}


        //public TextureSlot TextureHeight

        //{

        //    get => _material.TextureHeight;

        //    set => _material.TextureHeight = value;

        //}


        //public TextureSlot TextureNormal

        //{

        //    get => _material.TextureNormal;

        //    set => _material.TextureNormal = value;

        //}


        //public TextureSlot TextureOpacity

        //{

        //    get => _material.TextureOpacity;

        //    set => _material.TextureOpacity = value;

        //}
    }
}