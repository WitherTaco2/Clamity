using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;

namespace Clamity.Content.Bosses.WoB.Drop
{
    public class LoreWallOfBronze : LoreItem
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
