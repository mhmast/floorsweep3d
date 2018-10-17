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


vec4 FS( Floorsweep_UI_Rendering_Shaders_Default_FragmentInput input_)
{
    return input_.Color;
}


layout(location = 0) in vec4 fsin_0;
layout(location = 0) out vec4 _outputColor_;

void main()
{
    Floorsweep_UI_Rendering_Shaders_Default_FragmentInput input_;
    input_.SystemPosition = gl_FragCoord;
    input_.Color = fsin_0;
    vec4 output_ = FS(input_);
    _outputColor_ = output_;
}
