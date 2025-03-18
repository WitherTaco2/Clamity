using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Particles
{
    public class NightmareSpark : BaseClamityParticle
    {
        public override string Texture => $"CalamityMod/Projectiles/StarProj";
        public override bool UseCustomDraw => true;
        public override bool SetLifetime => true;
        public NightmareSpark(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
            Color = Color.White;
            Lifetime = 600;
            Time = 600;

        }
        public override void Update()
        {
            //Velocity.X -= 0.02f;
            //Velocity.Y *= 0.9999f;
        }
        public override void CustomDraw(SpriteBatch spriteBatch, Vector2 basePosition)
        {
            Texture2D t = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(t, basePosition - Main.screenPosition, null, Color.Orange, 0, t.Size() / 2, new Vector2(1, 0.5f) /* 0.1f*/, SpriteEffects.None, 1);
            spriteBatch.Draw(t, basePosition - Main.screenPosition, null, Color.Orange, MathHelper.PiOver2, t.Size() / 2, new Vector2(1, 0.5f) /* 0.1f*/, SpriteEffects.None, 1);
        }
    }
}
