float4x4 World;
float4x4 View;
float4x4 Projection;
//float4x4 WorldInverseTranspose;
 
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;
 
float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 0.5;
  
texture Texture0;
sampler2D textureSampler0 = sampler_state {
    Texture = Texture0;
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture Texture1;
sampler2D textureSampler1 = sampler_state {
    Texture = Texture1;
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture Texture2;
sampler2D textureSampler2 = sampler_state {
    Texture = Texture2;
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture Texture3;
sampler2D textureSampler3 = sampler_state {
    Texture = Texture3;
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct ColoredVertexShaderInput
{
    float4 Position : POSITION0;
    float4 Normal : NORMAL0;
	float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
	float4 TexWeights : TEXCOORD1;
};
 
struct VertexShaderOutput
{
    float4 Position : POSITION;
    float4 Color : COLOR0;
    float3 Normal : TEXCOORD0;
    float2 TextureCoordinate : TEXCOORD1;
	float4 TexWeights : TEXCOORD2;
};
 
VertexShaderOutput VertexShaderFunction(ColoredVertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position= mul(viewPosition, Projection);

    float4 ambient = (AmbientColor * AmbientIntensity);
    ambient.a = 1;

	float4 col = input.Color * ambient;
	col.a = 1;

	float4 normal = normalize(mul(input.Normal, World));
	float lightIntensity = dot(normal, DiffuseLightDirection);
	float4 diffuse = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
	diffuse.a = 1;

	float4 c = saturate(input.Color * ambient + diffuse);
	c.a = 1;

	output.Color = c;

    output.TextureCoordinate = input.TextureCoordinate;
	output.Normal = mul(input.Normal, World);
	output.TexWeights = input.TexWeights;

    return output;
}
 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 textureColor = 
	(tex2D(textureSampler0, input.TextureCoordinate) * input.TexWeights.x) +
	(tex2D(textureSampler1, input.TextureCoordinate) * input.TexWeights.y) +
	(tex2D(textureSampler2, input.TextureCoordinate) * input.TexWeights.z) +
	(tex2D(textureSampler3, input.TextureCoordinate) * input.TexWeights.w);

    return saturate(textureColor * input.Color);
}
 
technique MultiTextured
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}