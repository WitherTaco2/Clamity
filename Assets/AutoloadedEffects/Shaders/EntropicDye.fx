sampler uImage0 : register(s0); // The texture that you are currently drawing.
sampler uImage1 : register(s1); // A secondary texture that you can use for various purposes. This is usually a noise map.
sampler noiseTexture : register(s1);

float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect; // The position and size of the currently drawn frame.
float2 uWorldPosition;
float uDirection;
float3 uLightSource; // Used for reflective dyes.
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;
    
float4 ArmorBasic(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 noiseCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;

    //Making Purple must
    float moveX = (uTime + noiseCoords.x * 30) % 15 / 15;
    float moveY = (uTime + noiseCoords.y * 30) % 15 / 15;
    float2 noicePos = float2(moveX, moveY);
    float4 noiceColor = tex2D(noiseTexture, noicePos);
    float4 bg = lerp(float4(0, 0, 0, 1), float4(0.5, 0, 0.5, 1), noiceColor.x);
    
    //Making Cyan must
    float moveX2 = 1.0 - (uTime + noiseCoords.x * 60) % 15 / 15;
    float moveY2 = (uTime + noiseCoords.y * 60) % 15 / 15;
    float2 noicePos2 = float2(moveX2, moveY2);
    float4 noiceColor2 = tex2D(noiseTexture, noicePos2);
    float4 bg2 = lerp(float4(0, 0, 0, 1), float4(0, 0, 0.54, 1), noiceColor2.x);

    //Make a middle between must colors
    float4 finalBG = lerp(bg, bg2, 0.5);
    
    //Making darness "mist"
    float moveX3 = (uTime + noiseCoords.x * 90) % 15 / 15;
    float moveY3 = (uTime + noiseCoords.y * 90) % 15 / 15;
    float2 noicePos3 = float2(moveX3, moveY3);
    float4 noiceColor3 = tex2D(noiseTexture, noicePos3);

    //Lerping final color with darkness
    float4 color = tex2D(uImage0, coords);
    color.rgb = finalBG.rgb;

    return sampleColor * color;
}
    
technique Technique1
{
    pass ArmorBasic
    {
        PixelShader = compile ps_2_0 ArmorBasic();
    }
}