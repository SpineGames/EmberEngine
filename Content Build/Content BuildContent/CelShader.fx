//--------------------------- BASIC PROPERTIES ------------------------------
float4x4 World;
 
float4x4 View;
 
float4x4 Projection;
 
float4x4 WorldInverseTranspose;
 
//--------------------------- DIFFUSE LIGHT PROPERTIES ------------------------------
float3 DiffuseLightDirection = float3(1, 1, -1);
 
float4 DiffuseColor = float4(1.0, 1.0, 1.0, 1.0);
 
float DiffuseIntensity = 1.0;
 
//--------------------------- TOON SHADER PROPERTIES ------------------------------
float4 LineColor = float4(0, 0, 0, 1);
 
float LineThickness = .03;

float ToonLevel = 4.0;
 
//--------------------------- TEXTURE PROPERTIES ------------------------------
texture Texture;
sampler2D textureSampler = sampler_state 
{
    Texture = Texture;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
 
//--------------------------- DATA STRUCTURES ------------------------------
struct AppToVertex
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;      
	float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
};
 
struct VertexToPixel
{
    float Intensity : TEXCOORD2;
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
    float3 Normal : TEXCOORD1;
};
 
//--------------------------- SHADERS ------------------------------
// The vertex shader that does cel shading.
VertexToPixel CelVertexShader(AppToVertex input)
{
    VertexToPixel output;
 
    // Transform the position
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
 
    // Transform the normal
    output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));
 
    // Copy over the texture coordinate
    output.TextureCoordinate = input.TextureCoordinate;
	
    // Calculate diffuse light amount
    float intensity = dot(normalize(DiffuseLightDirection), input.Normal);

    if(intensity < 0)
        intensity = 0;
    
	output.Intensity = intensity;
	 
    return output;
}
 
// The pixel shader that does cel shading.  Basically, it calculates
// the color like is should, and then it discretizes the color into
// one of four colors.
float4 CelPixelShader(VertexToPixel input) : COLOR0
{ 
    // Calculate what would normally be the final color, including texturing and diffuse lighting
    float4 color = tex2D(textureSampler, input.TextureCoordinate);
	color = color * DiffuseColor;
    color.a = 1;

    color.r = round(color.r * ToonLevel) / ToonLevel;
    color.g = round(color.g * ToonLevel) / ToonLevel;
    color.b = round(color.b * ToonLevel) / ToonLevel;
 
    return color;
}
 
// The vertex shader that does the outlines
VertexToPixel OutlineVertexShader(AppToVertex input)
{
    VertexToPixel output = (VertexToPixel)0;
 
    // Calculate where the vertex ought to be.  This line is equivalent
    // to the transformations in the CelVertexShader.
    float4 original = mul(mul(mul(input.Position, World), View), Projection);
 
    // Calculates the normal of the vertex like it ought to be.
    float4 normal = mul(mul(mul(input.Normal, World), View), Projection);
 
    // Take the correct "original" location and translate the vertex a little
    // bit in the direction of the normal to draw a slightly expanded object.
    // Later, we will draw over most of this with the right color, except the expanded
    // part, which will leave the outline that we want.
    output.Position = original + (mul(LineThickness, normal));
	 
    return output;
}
 
// The pixel shader for the outline.  It is pretty simple:  draw everything with the
// correct line color.
float4 OutlinePixelShader(VertexToPixel input) : COLOR0
{
    return LineColor;
}
 
//A simpler toon shader
technique SimpleToon
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 CelVertexShader();
        PixelShader = compile ps_2_0 CelPixelShader();
        CullMode = CCW;
    }
}

// The entire technique for doing toon shading
technique Toon
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 OutlineVertexShader();
        PixelShader = compile ps_2_0 OutlinePixelShader();
        CullMode = CW;
    }
 
    pass Pass1
    {
        VertexShader = compile vs_2_0 CelVertexShader();
        PixelShader = compile ps_2_0 CelPixelShader();
        CullMode = CCW;
    }
}