﻿using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions.Food
{
    public class EldercherryJuice : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Foods";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(24, 26, BuffID.WellFed3, 30 * 60 * 60, true);
            Item.value += Terraria.Item.sellPrice(gold: 24);
            Item.rare = ModContent.RarityType<Turquoise>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Eldercherry>(2)
                .AddIngredient(ItemID.Bottle)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
