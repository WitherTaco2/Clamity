using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Clamitas.Projectiles
{
    public class BrimstoneSpiritsSpawner : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 48;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;

        }
        public override void AI()
        {
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }
            Projectile.ai[2] += Power;
        }
        public ref float Width => ref Projectile.ai[0];
        public ref float Power => ref Projectile.ai[1];
        public override void OnKill(int timeLeft)
        {
            for (float i = -Width / 2f; i < Width / 2f; i += 20)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * i, new Vector2(Main.rand.NextFloat(10f, 12f) * Power, 0).RotatedBy(Projectile.rotation), ModContent.ProjectileType<BrimstoneSpirits>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D line = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomLine").Value;
            Main.spriteBatch.Draw(line, Projectile.Center + new Vector2(Projectile.ai[2], Width / 2f).RotatedBy(Projectile.rotation) - Main.screenPosition, null, Color.OrangeRed, Projectile.rotation, new Vector2(line.Width / 2f, line.Height), new Vector2(2f, Width), SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }


        public override bool CanHitPlayer(Player target) => false;
        public override bool? CanHitNPC(NPC target) => false;

    }
}
