using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Clamity.Content.Items.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Accessories
{
    public class SupremeBarrier : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            this.Item.width = 60;
            this.Item.height = 54;
            this.Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            this.Item.defense = 30;
            this.Item.accessory = true;
            this.Item.rare = ModContent.RarityType<HotPink>();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModContent.GetInstance<AsgardianAegis>().UpdateAccessory(player, hideVisual);
            ModContent.GetInstance<RampartofDeities>().UpdateAccessory(player, hideVisual);
            //player.Calamity().copyrightInfringementShield = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AsgardianAegis>()
                //.AddIngredient<ShieldoftheHighRuler>()
                .AddIngredient<RampartofDeities>()
                .AddIngredient<ExoBaguette>()
                .AddIngredient<ShadowspecBar>(5)
                .AddIngredient(ItemID.GolfCupFlagWhite)
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
}
