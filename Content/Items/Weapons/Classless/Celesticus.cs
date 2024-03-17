using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Typeless;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Classless
{
    public class Celesticus : Aestheticus, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Classless";
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 40;
            Item.useTime = Item.useAnimation = 20;
            base.Item.Calamity().donorItem = false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Aestheticus>()
                .AddIngredient<TrashOfMagnus>()
                .AddIngredient<DivineGeode>(5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
