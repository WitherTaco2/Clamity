sampler baseTexture : register(s0);
sampler noiseTexture : register(s1);

float globalTimer;
float4 backgroundColor1;
float4 backgroundColor2;

float4 TestShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float moveX = (globalTimer + coords.x * 30) % 60 / 60;
    float moveY = (globalTimer + coords.y * 30) % 60 / 60;
    float2 noicePos = float2(moveX, moveY);
    float4 noiceColor = tex2D(noiseTexture, noicePos);

    float4 backgroundColor = lerp(backgroundColor1, backgroundColor2, noiceColor.x);

    return backgroundColor;

    //return noiceColor;
}
    
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 TestShader();
    }
}