using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Clamity.Content.Items.Materials
{
    public class EssenceOfHeat : EssenceofEleum
    {
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float num = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.9f * num, 0.4f * num, 0.5f * num);
        }
    }
    public class CoreOfHeat : CoreofEleum
    {
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float num = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.9f * num, 0.4f * num, 0.5f * num);
        }
        public override void AddRecipes()
        {
            CreateRecipe(3).AddIngredient<EssenceOfHeat>().AddIngredient(1508).AddTile(134)
                .Register();
        }
    }
}
