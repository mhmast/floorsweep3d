#version 450
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
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

layout(set = 0, binding = 0) uniform Projection
{
    mat4 field_Projection;
};

layout(set = 0, binding = 1) uniform View
{
    mat4 field_View;
};

layout(set = 1, binding = 0) uniform World
{
    mat4 field_World;
};

layout(set = 1, binding = 1) uniform Material
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


layout(location = 0) in vec3 Position;
layout(location = 1) in vec3 Normal;
layout(location = 0) out vec4 fsin_0;

void main()
{
    Floorsweep_UI_Rendering_ShaderObjects_VertexWithNormal input_;
    input_.Position = Position;
    input_.Normal = Normal;
    Floorsweep_UI_Rendering_Shaders_Default_FragmentInput output_ = VS(input_);
    fsin_0 = output_.Color;
    gl_Position = output_.SystemPosition;
        gl_Position.y = -gl_Position.y; // Correct for Vulkan clip coordinates
}
