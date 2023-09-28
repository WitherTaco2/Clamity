/*
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Clamity.Content.Projectiles
{
    internal class ShortstrikeEffect : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Holy Aura");
        }
        public override void SetDefaults()
        {
            Projectile.width = 162;
            Projectile.height = 162;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 50;
            Projectile.alpha = 255;

            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += 0.05f * player.direction;
            Projectile.Center = player.Center;

            for (int i = 0; i < 30; i++)
            {
                float distance = Main.rand.Next(40) * 4;
                Vector2 dustRotation = new Vector2(distance, 0f).RotatedBy(MathHelper.ToRadians(i * 12));
                Vector2 dustPosition = Projectile.Center + dustRotation;
                Vector2 nextDustPosition = Projectile.Center + dustRotation.RotatedBy(MathHelper.ToRadians(-4));
                Vector2 dustVelocity = (dustPosition - nextDustPosition + Projectile.velocity) * player.direction;
                if (Main.rand.NextBool(8))
                {
                    Dust dust = Dust.NewDustPerfect(dustPosition, DustID.GoldFlame, dustVelocity, 0, Scale: 1.5f);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.alpha += 10;
                    dust.rotation = dustRotation.ToRotation();
                }
            }

            if (Projectile.timeLeft >= 40)
                Projectile.alpha -= 10;

            if (Projectile.timeLeft <= 20)
            {
                Projectile.alpha += 7;
                if (Projectile.alpha >= 255)
                    Projectile.Kill();
            }
        }
        private Vector2 PolarVector(float radius, float theta) =>
            new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            //Texture2D flare = Redemption.WhiteFlare.Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);
            //Vector2 flareOrigin = new(flare.Width / 2, flare.Height / 2);
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, drawOrigin, Projectile.scale * 1.6f, effects, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), -Projectile.rotation, drawOrigin, Projectile.scale * 1.6f, effects, 0);
            //Main.EntitySpriteDraw(flare, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.LightYellow) * 2f, Projectile.rotation, flareOrigin, Projectile.scale * 1.6f, effects, 0);
            //Main.EntitySpriteDraw(flare, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.LightYellow) * 2f, -Projectile.rotation, flareOrigin, Projectile.scale * 1.6f, effects, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
*/