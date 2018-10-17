#include <metal_stdlib>
using namespace metal;
struct Floorsweep_UI_Rendering_Shaders_Default_FragmentInput
{
    float4 SystemPosition [[ position ]];
    float4 Color [[ attribute(0) ]];
};

struct Floorsweep_UI_Rendering_ShaderObjects_Material
{
    float BumpScaling;
    packed_float4 ColorAmbient;
    packed_float4 ColorDiffuse;
    packed_float4 ColorEmissive;
    packed_float4 ColorSpecular;
    packed_float4 ColorTransparent;
    float Opacity;
    float Reflectivity;
    float Shininess;
    float ShininessStrength;
};

struct Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal
{
    float3 Position [[ attribute(0) ]];
    float3 Normal [[ attribute(1) ]];
};

struct ShaderContainer {

ShaderContainer(

)
{}
float4 FS( Floorsweep_UI_Rendering_Shaders_Default_FragmentInput input)
{
    return input.Color;
}


};

fragment float4 FS(Floorsweep_UI_Rendering_Shaders_Default_FragmentInput input [[ stage_in ]])
{
return ShaderContainer().FS(input);
}
