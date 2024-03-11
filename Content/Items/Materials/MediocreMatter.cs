using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Plates;

namespace Clamity.Content.Items.Materials
{
    public class MediocreMatter : MiracleMatter
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 66;
            Item.maxStack = 9999;
            base.Item.value = Terraria.Item.sellPrice(copper: 50);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Cinderplate>(10)
                .AddIngredient<Havocplate>(10)
                .AddIngredient<Elumplate>(10)
                .AddIngredient<Navyplate>(10)
                .AddIngredient<Plagueplate>(10)
                .AddTile(TileID.Hellforge)
                .Register();

            CreateRecipe()
                .AddIngredient<EssenceofSunlight>()
                .AddIngredient<EssenceofHavoc>()
                .AddIngredient<EssenceofEleum>()
                .AddIngredient<SeaPrism>()
                .AddIngredient<PlagueCellCanister>()
                .AddIngredient(ItemID.Obsidian, 30)
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }
    }
}
