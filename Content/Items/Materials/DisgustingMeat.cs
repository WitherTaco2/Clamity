using CalamityMod.Items.Pets;
using CalamityMod.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Materials
{
    public class DisgustingMeat : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Materials";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 71;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 32);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe(5000)
                .AddIngredient<DeliciousMeat>(5000)
                .AddIngredient<BloodyVein>()
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
