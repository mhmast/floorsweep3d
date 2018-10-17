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

cbuffer ProjectionBuffer : register(b0)
{
    float4x4 Projection;
}

cbuffer ViewBuffer : register(b1)
{
    float4x4 View;
}

cbuffer WorldBuffer : register(b2)
{
    float4x4 World;
}

cbuffer MaterialBuffer : register(b3)
{
    Floorsweep_UI_Rendering_ShaderObjects_Material Material;
}


Floorsweep_UI_Rendering_Shaders_Default_FragmentInput VS( Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal input)
{
    Floorsweep_UI_Rendering_Shaders_Default_FragmentInput output;
    float4 worldPosition = mul(World, float4(input.Position, 1));
    float4 viewPosition = mul(View, worldPosition);
    float4 clipPosition = mul(Projection, viewPosition);
    output.SystemPosition = clipPosition;
    output.Color = Material.ColorDiffuse;
    return output;
}


