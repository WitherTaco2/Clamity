﻿using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions
{
    public class AuricApple : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Foods";
        public override void SetDefaults()
        {
            Item.DefaultToFood(24, 26, BuffID.WellFed, 60 * 60 * 60);
            Item.value += Terraria.Item.sellPrice(gold: 12);
            Item.rare = ModContent.RarityType<Violet>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(5)
                .AddIngredient(ItemID.Apple, 5)
                .AddIngredient<AuricBar>()
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}