sampler baseTexture : register(s0);
sampler noiseTexture : register(s1);
sampler noiseTexture2 : register(s2);

float globalTimer;
float4 backgroundColor1;
float4 backgroundColor2;
float4 backgroundColor3;

float4 backgroundColorDarksun;
float darksunLerpValue;

float2 playerPos;
float2 screenSize;

float4 TestShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    //Making Purple must
    float moveX = (globalTimer + coords.x * 30 + playerPos.x / screenSize.x * 10) % 60 / 60;
    float moveY = (globalTimer + coords.y * 30 + playerPos.y / screenSize.y * 10) % 60 / 60;
    float2 noicePos = float2(moveX, moveY);
    float4 noiceColor = tex2D(noiseTexture, noicePos);
    float4 bg = lerp(backgroundColor1, lerp(backgroundColor2, backgroundColorDarksun, darksunLerpValue), noiceColor.x);
    
    //Making Cyan must
    float moveX2 = 1.0 - (globalTimer + coords.x * 60 + playerPos.x / screenSize.x * 10) % 60 / 60;
    float moveY2 = (globalTimer + coords.y * 60 + playerPos.y / screenSize.y * 10) % 60 / 60;
    float2 noicePos2 = float2(moveX2, moveY2);
    float4 noiceColor2 = tex2D(noiseTexture, noicePos2);
    float4 bg2 = lerp(backgroundColor1, lerp(backgroundColor3, backgroundColorDarksun, darksunLerpValue), noiceColor2.x);

    //Make a middle between must colors
    float4 finalBG = lerp(bg, bg2, 0.5);
    
    //Making darness "mist"
    float moveX3 = (globalTimer + coords.x * 90 + playerPos.x / screenSize.x * 10) % 60 / 60;
    float moveY3 = (globalTimer + coords.y * 90 + playerPos.y / screenSize.y * 10) % 60 / 60;
    float2 noicePos3 = float2(moveX3, moveY3);
    float4 noiceColor3 = tex2D(noiseTexture, noicePos3);

    //Lerping final color with darkness
    float4 veryFinalColor = lerp(backgroundColor1, finalBG, noiceColor3.x);

    return veryFinalColor;

    //return noiceColor;
}
    
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 TestShader();
    }
}