struct Floorsweep_UI_Rendering_Shaders_Default_FragmentInput
{
    float4 SystemPosition : SV_Position;
    float4 Color : COLOR0;
};

struct Floorsweep_UI_Rendering_ShaderObjects_Material
{
    float BumpScaling;
    float4 ColorAmbient;
    float4 ColorDiffuse;
    float4 ColorEmissive;
    float4 ColorSpecular;
    float4 ColorTransparent;
    float Opacity;
    float Reflectivity;
    float Shininess;
    float ShininessStrength;
};

struct Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal
{
    float3 Position : POSITION0;
    float3 Normal : NORMAL0;
};


float4 FS( Floorsweep_UI_Rendering_Shaders_Default_FragmentInput input) : SV_Target
{
    return input.Color;
}


