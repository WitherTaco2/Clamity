using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.ItemBossRush.ItemBosses.DivineRetribution
{
    public class DivineSpear : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 72;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.ai[1] += Projectile.velocity.Length();
            if (Projectile.ai[1] >= Projectile.ai[0])
                Projectile.velocity.RotatedBy(0.1f);
            else
                Projectile.velocity *= 1.05f;
        }
    }
}
