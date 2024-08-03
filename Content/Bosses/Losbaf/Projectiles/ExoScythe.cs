using CalamityMod;
using Clamity.Content.Bosses.Losbaf.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf.Projectiles
{
    public class ExoScythe : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 48;
            Projectile.hostile = true;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
            Projectile.alpha = 100;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] == 2 && Projectile.ai[1] != 2)
                Projectile.timeLeft = (int)(LosbafSuperboss.DistanceOnRotationAttack / LosbafSuperboss.VelocityOnRotationAttack);
        }
        public override void AI()
        {
            Projectile.rotation += 0.25f;
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    if (Projectile.timeLeft == 1000 - 60)
                    {
                        //Projectile.ai[1] = Projectile.velocity.Length();
                        Projectile.velocity = Vector2.Zero;
                    }
                    if (Projectile.timeLeft == 1000 - 180)
                    {
                        int target = Player.FindClosest(Projectile.Center, 100, 100);
                        Projectile.velocity = (Main.player[target].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
                    }
                    break;
                case 1:
                    if (Projectile.timeLeft == 1000 - 60)
                        Projectile.velocity = Vector2.Zero;
                    if (Projectile.timeLeft == 1000 - 180)
                    {
                        int target = Player.FindClosest(Projectile.Center, 100, 100);
                        Projectile.velocity = new Vector2(0, Projectile.ai[1]);
                    }
                    break;
                case 2:

                    break;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], Color.White, 1);

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 2 && Projectile.ai[1] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 2) / 3, ModContent.ProjectileType<ExoScythe>(), Projectile.damage, 0, Main.myPlayer, 2, 2);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 2 + MathHelper.PiOver4) / 3, ModContent.ProjectileType<ExoScythe>(), Projectile.damage, 0, Main.myPlayer, 2, 2);

            }
        }
    }
}
