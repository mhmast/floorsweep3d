#version 330 core

struct SamplerDummy { int _dummyValue; };
struct SamplerComparisonDummy { int _dummyValue; };

struct Floorsweep_UI_Rendering_Shaders_Cube_VertexInput
{
    vec3 Position;
    vec2 TexCoords;
};

struct Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput
{
    vec4 SystemPosition;
    vec2 TexCoords;
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


Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput VS( Floorsweep_UI_Rendering_Shaders_Cube_VertexInput input_)
{
    Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput output_;
    vec4 worldPosition = field_World * vec4(input_.Position, 1);
    vec4 viewPosition = field_View * worldPosition;
    vec4 clipPosition = field_Projection * viewPosition;
    output_.SystemPosition = clipPosition;
    output_.TexCoords = input_.TexCoords;
    return output_;
}


in vec3 Position;
in vec2 TexCoords;
out vec2 fsin_0;

void main()
{
    Floorsweep_UI_Rendering_Shaders_Cube_VertexInput input_;
    input_.Position = Position;
    input_.TexCoords = TexCoords;
    Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput output_ = VS(input_);
    fsin_0 = output_.TexCoords;
    gl_Position = output_.SystemPosition;
        gl_Position.z = gl_Position.z * 2.0 - gl_Position.w;
}
