using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Enemy;

namespace Clamity.Content.Bosses.Clamitas.NPCs
{
    public class BrimstonePearlBurst : PearlBurst
    {
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
