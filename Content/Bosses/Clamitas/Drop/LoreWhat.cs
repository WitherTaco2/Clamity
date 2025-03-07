using CalamityMod.Items.LoreItems;
using Terraria;
using Terraria.ID;

namespace Clamity.Content.Bosses.Clamitas.Drop
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
