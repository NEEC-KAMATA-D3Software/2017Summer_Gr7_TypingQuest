float4x4 World;
float4x4 View;
float4x4 Projection;

texture Texture;

float Speed;
// TODO: add effect parameters here.

sampler MainSampler:register(s0) = sampler_state
{
	Texture = <Texture>;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TexUV : TEXCOORD0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexUV0: TEXCOORD0;
    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.Position = mul(input.Position, Projection);

	output.TexUV0 = input.TexUV;

    // TODO: add your vertex shader code here.

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.

	return tex2D(MainSampler, input.TexUV0 + float2(Speed, 0));
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
