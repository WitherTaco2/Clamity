using CalamityMod;
using CalamityMod.Buffs.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Clamity.Content.Buffs
{
    public class ExoBaguetteBuff : BaguetteBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);
            if (player.Calamity().alcoholPoisonLevel > 0)
                player.Calamity().alcoholPoisonLevel--;
        }
    }
}
