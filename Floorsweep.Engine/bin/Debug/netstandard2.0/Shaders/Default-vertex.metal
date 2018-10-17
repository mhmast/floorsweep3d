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
constant float4x4& World;
constant float4x4& View;
constant float4x4& Projection;
constant Floorsweep_UI_Rendering_ShaderObjects_Material& Material;

ShaderContainer(
constant float4x4& World_param, constant float4x4& View_param, constant float4x4& Projection_param, constant Floorsweep_UI_Rendering_ShaderObjects_Material& Material_param
)
:
World(World_param), View(View_param), Projection(Projection_param), Material(Material_param)
{}
Floorsweep_UI_Rendering_Shaders_Default_FragmentInput VS( Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal input)
{
    Floorsweep_UI_Rendering_Shaders_Default_FragmentInput output;
    float4 worldPosition = World * float4(float4(input.Position, 1));
    float4 viewPosition = View * float4(worldPosition);
    float4 clipPosition = Projection * float4(viewPosition);
    output.SystemPosition = clipPosition;
    output.Color = Material.ColorDiffuse;
    return output;
}


};

vertex Floorsweep_UI_Rendering_Shaders_Default_FragmentInput VS(Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal input [[ stage_in ]], constant float4x4 &Projection [[ buffer(0) ]], constant float4x4 &View [[ buffer(1) ]], constant float4x4 &World [[ buffer(2) ]], constant Floorsweep_UI_Rendering_ShaderObjects_Material &Material [[ buffer(3) ]])
{
return ShaderContainer(World, View, Projection, Material).VS(input);
}
