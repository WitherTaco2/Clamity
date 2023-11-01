using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Materials
{
    public class MediocreMatter : MiracleMatter
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
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
                .AddIngredient<EssenceofSunlight>()
                .AddIngredient<EssenceofHavoc>()
                .AddIngredient<EssenceofEleum>()
                .AddIngredient<EssenceOfFlame>()
                .AddIngredient<SeaPrism>()
                .AddIngredient<PlagueCellCanister>()
                .AddIngredient<BloodOrb>()
                .AddIngredient(ItemID.Obsidian, 30)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
