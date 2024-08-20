using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Clamity.Content.Bosses.Losbaf.Projectiles
{
    public class LosbafCloneSlamAttack : BaseLosbafCloneProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 1000;
        }
        public ref float hoverTime => ref Projectile.ai[1];
        public ref float sitInPlaceTime => ref Projectile.ai[2];
        public ref float slamTime => ref Projectile.Clamity().extraAI[0];
        public ref float slamRotation => ref Projectile.Clamity().extraAI[1];
        public ref float target => ref Projectile.Clamity().extraAI[2];
        public ref float Timer => ref Projectile.Clamity().extraAI[3];
        public override void AI()
        {
            Player player = Main.player[(int)target];
            //int hoverTime = 33;
            //int slamTime = 90;
            //int sitInPlaceTime = 18;
            Timer++;
            if (Timer > MaxTimeLeft) Projectile.Kill();

            int slamDelay = (int)hoverTime + (int)sitInPlaceTime;
            float wrappedAttackTimer = Timer % (slamDelay + slamTime);

            if (wrappedAttackTimer <= hoverTime && wrappedAttackTimer >= 2f)
            {
                float hoverInterpolant = MathF.Pow(wrappedAttackTimer / hoverTime, 0.74f);
                Vector2 start = player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * slamRotation) * 195f;
                Vector2 end = player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * slamRotation) * 360f;
                Projectile.Center = Vector2.Lerp(start, end, hoverInterpolant);
                Projectile.velocity = Vector2.Zero;
            }
        }
        public int MaxTimeLeft => (int)(hoverTime + sitInPlaceTime + slamTime);
        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
