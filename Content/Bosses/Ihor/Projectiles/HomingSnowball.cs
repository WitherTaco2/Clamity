using CalamityMod;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Ihor.Projectiles
{
    public class HomingSnowball : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.scale = 0.5f;
        }
        public float homingSpeed => 15;
        public override void OnSpawn(IEntitySource source)
        {
            //Current homing speed
            Projectile.ai[0] = homingSpeed;
            //Base homing speed
            Projectile.ai[1] = homingSpeed + Main.rand.NextFloat(-2.5f, 2.5f);

        }
        public override void AI()
        {
            float inertia = 50f;

            int playerTracker = Player.FindClosest(Projectile.Center, 1, 1);
            Player player = Main.player[playerTracker];

            Vector2 targetVector = Projectile.SafeDirectionTo(player.Center);
            Projectile.velocity = (Projectile.velocity * (inertia - 1f) + (targetVector /*+ player.velocity / 10f*/) * Projectile.ai[0]) / inertia;

            float angle = Utilities.AngleBetween(targetVector, Projectile.velocity);
            if (angle < MathHelper.PiOver4 /*&& angle > -MathHelper.PiOver4*/)
                Projectile.ai[0] = MathHelper.Clamp(Projectile.ai[0] + Projectile.ai[1] / 50f, Projectile.ai[1], Projectile.ai[1] * 2);
            else
                Projectile.ai[0] = MathHelper.Clamp(Projectile.ai[0] - Projectile.ai[1] / 25f, Projectile.ai[1], Projectile.ai[1] * 2);

            Projectile.scale += 0.0015f;
            Projectile.rotation += Projectile.velocity.X;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = (int)Utils.Lerp(1, 30, Projectile.scale);
            hitbox.Inflate(size, size);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor);
            return false;
        }
    }
}
