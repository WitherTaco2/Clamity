using CalamityMod.Items.LoreItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace Clamity.Content.Boss.Clamitas.Drop
{
    public class LoreWhat : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
        }
    }
}
