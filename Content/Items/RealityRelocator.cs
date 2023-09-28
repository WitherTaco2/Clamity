using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Placeables.Plates;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items
{
    public class RealityRelocator : NormalityRelocator
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.Calamity().donorItem = false;
            //ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<NormalityRelocator>()] = Item.type;
        }
        public override void UpdateInventory(Player player)
        {
            player.Clamity().realityRelocator = true;

        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.RodOfHarmony).AddIngredient<Cinderplate>(5).AddIngredient<ExodiumCluster>(10)
                .AddIngredient(3459, 30)
                .AddTile(412)
                .Register();
        }
    }
}
