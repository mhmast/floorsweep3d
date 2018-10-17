#include <metal_stdlib>
using namespace metal;
struct Floorsweep_UI_Rendering_Shaders_Cube_VertexInput
{
    float3 Position [[ attribute(0) ]];
    float2 TexCoords [[ attribute(1) ]];
};

struct Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput
{
    float4 SystemPosition [[ position ]];
    float2 TexCoords [[ attribute(0) ]];
};

struct ShaderContainer {
constant float4x4& World;
constant float4x4& View;
constant float4x4& Projection;

ShaderContainer(
constant float4x4& World_param, constant float4x4& View_param, constant float4x4& Projection_param
)
:
World(World_param), View(View_param), Projection(Projection_param)
{}
Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput VS( Floorsweep_UI_Rendering_Shaders_Cube_VertexInput input)
{
    Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput output;
    float4 worldPosition = World * float4(float4(input.Position, 1));
    float4 viewPosition = View * float4(worldPosition);
    float4 clipPosition = Projection * float4(viewPosition);
    output.SystemPosition = clipPosition;
    output.TexCoords = input.TexCoords;
    return output;
}


};

vertex Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput VS(Floorsweep_UI_Rendering_Shaders_Cube_VertexInput input [[ stage_in ]], constant float4x4 &Projection [[ buffer(0) ]], constant float4x4 &View [[ buffer(1) ]], constant float4x4 &World [[ buffer(2) ]])
{
return ShaderContainer(World, View, Projection).VS(input);
}
