sampler baseTexture : register(s0);
sampler noiseTexture : register(s1);

float globalTimer;
float4 backgroundColor1;
float4 backgroundColor2;
float4 backgroundColor3;

float4 backgroundColorDarksun;
float darksunLerpValue;

float4 TestShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float moveX = (globalTimer + coords.x * 30) % 60 / 60;
    float moveY = (globalTimer + coords.y * 30) % 60 / 60;
    float2 noicePos = float2(moveX, moveY);
    float4 noiceColor = tex2D(noiseTexture, noicePos);
    float4 bg = lerp(backgroundColor1, backgroundColor2, noiceColor.x);

    
    float moveX2 = (globalTimer + coords.x * 45) % 60 / 60;
    float moveY2 = (globalTimer + coords.y * 45) % 60 / 60;
    float2 noicePos2 = float2(moveX2, moveY2);
    float4 noiceColor2 = tex2D(noiseTexture, noicePos2);
    float4 bg2 = lerp(backgroundColor1, backgroundColor3, noiceColor2.x);

    float finalBG = lerp(bg, bg2, 0.5);

    return lerp(finalBG, backgroundColorDarksun, darksunLerpValue);

    //return noiceColor;
}
    
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 TestShader();
    }
}