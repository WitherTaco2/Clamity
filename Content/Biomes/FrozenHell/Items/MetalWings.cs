using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.FrozenHell.Items
{
    [AutoloadEquip(EquipType.Wings)]
    public class MetalWings : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories.Wings";
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(280, 10.5f, 2.8f, true, 12f, 3f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }
        //When you getting hit, you receive flight time percentages equal to half of the ratio of damage received to max HP
        //While wearing Frozen Armor, you and players in your team gets a 8 increased defence
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Clamity().metalWings = true;
            if (player.Clamity().frozenParrying)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (player.team == Main.player[i].team)
                        player.statDefense += 8;

                }
            }
            //player.statDefense += 8;

        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.90f;
            ascentWhenRising = 0.16f;
            maxCanAscendMultiplier = 1.1f;
            maxAscentMultiplier = 3.2f;
            constantAscend = 0.145f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFlight, 20)
                .AddIngredient<MetalFeather>()
                .AddIngredient<EnchantedMetal>(5)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
