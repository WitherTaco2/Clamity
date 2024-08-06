using Clamity.Commons;
using Clamity.Content.Bosses.Losbaf.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf.Projectiles
{
    public class LosbafCloneRotatable : BaseLosbafCloneProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = (int)LosbafSuperboss.DuratationOfRotationAttack;
        }
        public override void AI()
        {
            Player player = Main.player[(int)Projectile.ai[1]];
            Projectile.Center = player.Center + Vector2.UnitY.RotatedBy(Main.GlobalTimeWrappedHourly * LosbafSuperboss.RotationPower + MathHelper.PiOver2 * (Projectile.ai[0] + 1)) * LosbafSuperboss.DistanceOnRotationAttack;
            Projectile.rotation = (player.Center - Projectile.Center).ToRotation() - MathHelper.PiOver2;
            int AttackTimer = (int)LosbafSuperboss.DuratationOfRotationAttack - Projectile.timeLeft;
            if (AttackTimer % 15 == 0 && AttackTimer > 30)
            {
                Vector2 vec2 = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                //float velocity = 
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec2 * LosbafSuperboss.VelocityOnRotationAttack, ModContent.ProjectileType<LosbafExoBeam>(), ModContent.GetInstance<LosbafSuperboss>().NPC.GetProjectileDamageClamity(ModContent.ProjectileType<LosbafExoBeam>()), 0, Main.myPlayer, 2, (AttackTimer % 60 == 0).ToInt());
            }

        }
    }
}
