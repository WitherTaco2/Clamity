using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Enemy;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Clamitas.NPCs
{
    public class BrimstonePearlBurst : PearlBurst
    {
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
