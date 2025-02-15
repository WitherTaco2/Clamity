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

    
    float moveX2 = (globalTimer + coords.x * 45) % 60 / 60;
    float moveY2 = (globalTimer + coords.y * 45) % 60 / 60;
    float2 noicePos2 = float2(moveX2, moveY2);
    float4 noiceColor2 = tex2D(noiseTexture, noicePos2);

    float4 finalNoiceColor = lerp(noiceColor, noiceColor2, 0.5);

    float4 backgroundColor = lerp(backgroundColor1, backgroundColor2, finalNoiceColor.x);

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