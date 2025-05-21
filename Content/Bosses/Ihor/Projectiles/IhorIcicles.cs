using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Ihor.Projectiles
{
    public class IhorIcicles : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(3);
        }
        public override void AI()
        {
            //Projectile.velocity *= 1.02f;
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.timeLeft > 500)
            {
                if (Projectile.velocity.Length() > 0.1f)
                    Projectile.velocity *= 0.95f;
            }
            else
            {
                //Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
                Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.rotation - Projectile.velocity.ToRotation());
                if (Projectile.velocity.Length() < 15f)
                    Projectile.velocity *= 1.05f;
            }
            Projectile.rotation += Projectile.ai[0];
            Projectile.ai[0] *= 0.99f;

            if (Projectile.timeLeft % 2 == 0 && Projectile.timeLeft < 550 && Projectile.velocity.Length() > 1f)
            {
                SparkParticle spark = new SparkParticle(Projectile.Center - Projectile.velocity * 2f, -Projectile.velocity * 0.1f, false, 9, 1.5f, Color.White * 0.2f);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileWithBackglow(Color.Cyan, lightColor, 2f);
            return base.PreDraw(ref lightColor);
        }*/
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawProjectileWithBackglow(Color.Cyan, lightColor, 2f);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor);
            return false;
        }
    }
}
