using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace Clamity.Content.Buffs.Shortstrike
{
    public class WulfrumShortstrike : ModBuff, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Buffs.Shortstrike";
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.Clamity().wulfrumShortstrike = true;
        }
    }
}
