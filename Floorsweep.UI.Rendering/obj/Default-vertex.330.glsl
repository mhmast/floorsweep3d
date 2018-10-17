#version 330 core

struct SamplerDummy { int _dummyValue; };
struct SamplerComparisonDummy { int _dummyValue; };

struct Floorsweep_UI_Rendering_Shaders_Default_FragmentInput
{
    vec4 SystemPosition;
    vec4 Color;
};

struct Floorsweep_UI_Rendering_ShaderObjects_Material
{
    float BumpScaling;
    vec4 ColorAmbient;
    vec4 ColorDiffuse;
    vec4 ColorEmissive;
    vec4 ColorSpecular;
    vec4 ColorTransparent;
    float Opacity;
    float Reflectivity;
    float Shininess;
    float ShininessStrength;
};

struct Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal
{
    vec3 Position;
    vec3 Normal;
};

layout(std140) uniform Projection
{
    mat4 field_Projection;
};

layout(std140) uniform View
{
    mat4 field_View;
};

layout(std140) uniform World
{
    mat4 field_World;
};

layout(std140) uniform Material
{
    Floorsweep_UI_Rendering_ShaderObjects_Material field_Material;
};


Floorsweep_UI_Rendering_Shaders_Default_FragmentInput VS( Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal input_)
{
    Floorsweep_UI_Rendering_Shaders_Default_FragmentInput output_;
    vec4 worldPosition = field_World * vec4(input_.Position, 1);
    vec4 viewPosition = field_View * worldPosition;
    vec4 clipPosition = field_Projection * viewPosition;
    output_.SystemPosition = clipPosition;
    output_.Color = field_Material.ColorDiffuse;
    return output_;
}


in vec3 Position;
in vec3 Normal;
out vec4 fsin_0;

void main()
{
    Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal input_;
    input_.Position = Position;
    input_.Normal = Normal;
    Floorsweep_UI_Rendering_Shaders_Default_FragmentInput output_ = VS(input_);
    fsin_0 = output_.Color;
    gl_Position = output_.SystemPosition;
        gl_Position.z = gl_Position.z * 2.0 - gl_Position.w;
}
