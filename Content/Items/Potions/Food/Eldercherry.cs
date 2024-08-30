using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions.Food
{
    public class Eldercherry : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Foods";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(24, 26, BuffID.WellFed2, 30 * 60 * 60);
            Item.value += Terraria.Item.sellPrice(gold: 12);
            Item.rare = ModContent.RarityType<Turquoise>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cherry)
                .AddIngredient<DivineGeode>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
