using CalamityMod.Items.LoreItems;
using CalamityMod.Rarities;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Profusion.Drop
{
    public class LoreProfusion : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.consumable = false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ProfusionTrophy>()
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}
