using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Tiles.Furniture;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Placeable
{
    public class ShadowspecToilet : AuricToilet, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.createTile = ModContent.TileType<ShadowspecToiletTile>();
            Item.Calamity().devItem = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AuricToilet>()
                .AddIngredient<ThaumaticChair>()
                .AddIngredient(ItemID.TerraToilet)
                .AddIngredient<ShadowspecBar>(5)
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
    public class ShadowspecToiletTile : AuricToiletTile
    {

    }
}
