using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Materials
{
    public class CoralskinFoolfish : ModItem, ILocalizedModType, IModType
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
            Item.value = Item.sellPrice(0, 0, 7);
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ItemID.SeafoodDinner)
                .AddIngredient(Type, 2)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
