using CalamityMod.Items.LoreItems;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Rarities;

namespace Clamity.Content.Bosses.WoB.Drop
{
    public class WoBLore : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 26;
            Item.rare = ModContent.RarityType<Violet>();
            Item.consumable = false;
        }
    }
}
