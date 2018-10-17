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

uniform sampler2D SurfaceTexture;

SamplerDummy SurfaceSampler = SamplerDummy(0);


vec4 FS( Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput input_)
{
    return texture(SurfaceTexture, input_.TexCoords);
}


in vec2 fsin_0;
out vec4 _outputColor_;

void main()
{
    Floorsweep_UI_Rendering_Shaders_Cube_FragmentInput input_;
    input_.SystemPosition = gl_FragCoord;
    input_.TexCoords = fsin_0;
    vec4 output_ = FS(input_);
    _outputColor_ = output_;
}
