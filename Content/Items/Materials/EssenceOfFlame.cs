using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;

namespace Clamity.Content.Items.Materials
{
    public class EssenceOfFlame : EssenceofEleum
    {
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float num = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.9f * num, 0.4f * num, 0.5f * num);
        }
    }
    public class CoreOfFlame : CoreofEleum
    {
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float num = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.9f * num, 0.4f * num, 0.5f * num);
        }
        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient<EssenceOfFlame>()
                .AddIngredient(ItemID.Ectoplasm)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
