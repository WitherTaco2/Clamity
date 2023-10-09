using Terraria.ID;
using Terraria;
using CalamityMod.Items.LoreItems;

namespace Clamity.Content.Boss.Pyrogen.Drop
{
    public class LorePyrogen : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
        }
    }
}
