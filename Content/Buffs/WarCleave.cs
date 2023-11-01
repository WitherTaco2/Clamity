using CalamityMod;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Buffs
{
    internal class WarCleave : ModBuff
    {
    public static int DefenseReduction = 15;

        public virtual void SetDefaults()
        {
            Main.debuff[this.Type] = true;
            Main.pvpBuff[this.Type] = true;
            Main.buffNoSave[this.Type] = true;
            BuffID.Sets.LongerExpertDebuff[this.Type] = true;
        }

        public virtual void Update(NPC npc, ref int buffIndex)
        {
            if (npc.Calamity().wCleave < npc.buffTime[buffIndex])
                npc.Calamity().wCleave = npc.buffTime[buffIndex];
            npc.DelBuff(buffIndex);
            --buffIndex;
        }

        public virtual void Update(Player player, ref int buffIndex) => player.Calamity().wCleave = true;
    }
}
