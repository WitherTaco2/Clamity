using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Commons.Particles
{
    public class ChromaticBurstParticle : Particle
    {
        public float Opacity = 1f;

        public float StartingScale = 1f;
        public float FinalScale = 1f;

        public override bool SetLifetime => true;

        public override bool UseCustomDraw => true;

        public override bool UseAdditiveBlend => true;

        public override string Texture => "CalRemix/Content/Particles/ChromaticBurst";

        public ChromaticBurstParticle(Vector2 position, Vector2 velocity, Color color, int lifetime, float startingScale, float finalScale)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            StartingScale = startingScale;
            FinalScale = finalScale;
            Lifetime = lifetime;
            Time = lifetime;
        }

        public override void Update()
        {
            Opacity = Utils.GetLerpValue(0f, 4f, Lifetime - Time, true);
            Scale = MathHelper.Lerp(StartingScale, FinalScale, LifetimeCompletion);
            //Scale += 0.8f;
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color * Opacity, Rotation, texture.Size() * 0.5f, Scale * 0.3f, 0, 0f);
        }
    }
}
