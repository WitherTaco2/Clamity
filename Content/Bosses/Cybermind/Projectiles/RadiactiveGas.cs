using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Cybermind.Projectiles
{
    public class RadiactiveGas : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override string Texture => ModContent.GetInstance<ApolloFireball>().Texture;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }
        public override void SetDefaults()
        {
            //Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.Opacity = 0f;
            CooldownSlot = ImmunityCooldownID.Bosses;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Type])
                {
                    Projectile.frame = 0;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity *= 0.9f;

            if (Projectile.velocity.Length() < 1f)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.ExpandHitboxBy(Projectile.width * 2);
                    Projectile.ai[1] = 1;
                }
                Projectile.Opacity += 0.05f;
                Projectile.ai[0]++;
                if (Projectile.ai[0] >= 4)
                {
                    Projectile.ai[0]++;
                    Particle mist = new MediumMistParticle(
                        Projectile.Center + Main.rand.NextVector2Circular(48, 48),
                        Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) / 2,
                        //Color.Lerp(new Color(234, 255, 97), new Color(161, 251, 70), Main.rand.NextFloat(0, 1f)),
                        new Color(161, 251, 70),
                        new Color(234, 255, 97),
                        2f, 220f, 0.1f
                        );
                    GeneralParticleHandler.SpawnParticle(mist);
                    Projectile.ai[0] = 0;
                }
            }
        }
    }
}
