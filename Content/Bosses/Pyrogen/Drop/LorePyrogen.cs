using CalamityMod.Items.LoreItems;
using Terraria;
using Terraria.ID;


namespace Clamity.Content.Bosses.Pyrogen.Drop
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
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PyrogenTrophy>()
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}
