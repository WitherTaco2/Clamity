using Luminance.Common.Easings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Clamity.Content.Particles
{
    public class ChromaticBurstParticle : BaseClamityParticle
    {
        public float StartingScale;
        public float FinalScale;
        public Color StartingColor;
        public Color FinalColor;
        public EasingType ScaleEasingType;

        public EasingCurves.Curve ScaleEasingCurve
        {
            get;
            set;
        }

        public override bool SetLifetime => true;
        public override bool UseCustomDraw => true;
        public override bool UseAdditiveBlend => true;

        public override string Texture => $"{BaseTexturePath}/ChromaticBurst";

        public ChromaticBurstParticle(Vector2 position, Vector2 velocity, Color startingColor, Color finalColor, int lifetime, float startingScale, float finalScale, EasingCurves.Curve scaleEasingCurve, EasingType scaleEasingType)
        {
            Position = position;
            Velocity = velocity;
            //Color = Color.White;
            StartingColor = startingColor;
            FinalColor = finalColor;
            StartingScale = startingScale;
            FinalScale = finalScale;
            Lifetime = lifetime;
            Time = lifetime;
            ScaleEasingCurve = scaleEasingCurve;
            ScaleEasingType = scaleEasingType;
        }

        public override void Update()
        {
            //Opacity = Utils.GetLerpValue(0f, 4f, Lifetime - Time, true);
            Color = Color.Lerp(StartingColor, FinalColor, LifetimeRatio);
            Scale = ScaleEasingCurve.Evaluate(ScaleEasingType, StartingScale, FinalScale, LifetimeRatio);
            //Scale += 0.8f;
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureFile, Position - Main.screenPosition, null, Color * Opacity, Rotation, TextureFile.Size() * 0.5f, Scale * 0.3f, 0, 0f);
        }
    }
}
