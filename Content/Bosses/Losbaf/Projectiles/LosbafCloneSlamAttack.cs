using CalamityMod.Events;
using CalamityMod.World;
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
        public ref float slamCounter => ref Projectile.ai[1];
        public ref float slamRotation => ref Projectile.ai[2];
        public ref float target => ref Projectile.Clamity().extraAI[1];
        public ref float Timer => ref Projectile.Clamity().extraAI[2];
        public override void AI()
        {
            Player player = Main.player[(int)target];

            bool bossRushActive = BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || bossRushActive;
            bool rev = CalamityWorld.revenge || bossRushActive;

            int hoverTime = 33;
            int slamTime = 90;
            int sitInPlaceTime = 18;
            if (expert)
            {
                if (slamCounter >= 1f)
                    hoverTime -= 3;
                sitInPlaceTime -= 3;
            }
            if (rev)
            {
                if (slamCounter >= 1f)
                    hoverTime -= 3;
                sitInPlaceTime -= 4;
            }


            Timer++;
            int MaxTimeLeft = (int)(hoverTime + sitInPlaceTime + slamTime);
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
        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
