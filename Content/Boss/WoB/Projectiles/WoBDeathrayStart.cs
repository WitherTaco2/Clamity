using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.Projectiles
{
    public class WoBDeathrayStart : ThanatosBeamStart
    {
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("Clamity/Content/Boss/WoB/Projectiles/WoBDeathrayMiddle").Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>("Clamity/Content/Boss/WoB/Projectiles/WoBDeathrayEnd").Value;
        public override void PostAI()
        {
            if (!OwnerIsValid)
            {
                return;
            }

            int type = 235;
            Vector2 vector = base.Projectile.Center + base.Projectile.velocity * (base.LaserLength - 14f);
            for (int i = 0; i < 2; i++)
            {
                Vector2 vector2 = (base.Projectile.velocity.ToRotation() + (float)Main.rand.NextBool().ToDirectionInt() * (MathF.PI / 2f)).ToRotationVector2() * Main.rand.NextFloat(2f, 4f);
                Dust dust = Dust.NewDustDirect(vector, 0, 0, type, vector2.X, vector2.Y);
                dust.noGravity = true;
                dust.scale = 1.7f;
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 vector3 = base.Projectile.velocity.RotatedBy(1.5707963705062866) * Main.rand.NextFloatDirection() * base.Projectile.width * 0.5f;
                Dust dust2 = Dust.NewDustDirect(vector + vector3 - Vector2.One * 4f, 8, 8, type, 0f, 0f, 100, default(Color), 1.5f);
                dust2.velocity *= 0.5f;
                dust2.velocity.Y = 0f - Math.Abs(dust2.velocity.Y);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.getGoodWorld)
                target.AddBuff(BuffID.Frozen, 180);
        }
    }
}
