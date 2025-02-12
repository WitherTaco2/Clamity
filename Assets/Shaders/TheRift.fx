sampler noiseTexture : register(s0);

float globalTime;
float zoom;

float4 TestShader(float4 sampleColor : COLOR0) : COLOR0
{
    return sampleColor;
}
    
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 TestShader();
    }
}