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
            Projectile.velocity *= 0.9f;
            Projectile.ai[0]++;
            if (Projectile.velocity.Length() < 1f && Projectile.ai[0] >= 4)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.ExpandHitboxBy(2);
                    Projectile.ai[1] = 1;
                }
                Particle mist = new MediumMistParticle(
                    Projectile.Center + Main.rand.NextVector2Circular(48, 48),
                    Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi),
                    //Color.Lerp(new Color(234, 255, 97), new Color(161, 251, 70), Main.rand.NextFloat(0, 1f)),
                    new Color(161, 251, 70),
                    new Color(234, 255, 97),
                    1f, Projectile.Opacity
                    );
                GeneralParticleHandler.SpawnParticle(mist);
                Projectile.ai[0] = 0;
            }
        }
    }
}
