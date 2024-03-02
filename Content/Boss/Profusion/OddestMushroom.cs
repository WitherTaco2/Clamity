using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.SummonItems;
using CalamityMod.Rarities;
using Clamity.Content.Boss.Profusion.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Profusion
{
    public class OddestMushroom : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.SummonBoss";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 36;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.ZoneGlowshroom && !NPC.AnyNPCs(ModContent.NPCType<ProfusionBoss>()))
            {
                return !BossRushEvent.BossRushActive;
            }

            return false;
        }
        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(in SoundID.Roar, player.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ProfusionBoss>());
            }
            else
            {
                NetMessage.SendData(61, -1, -1, null, player.whoAmI, ModContent.NPCType<ProfusionBoss>());
            }

            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<OddMushroom>()
                .AddIngredient<DecapoditaSprout>()
                .AddIngredient<FungalClump>()
                .AddIngredient(ItemID.GlowingMushroom, 50)
                .AddIngredient(ItemID.Mushroom, 20)
                .AddRecipeGroup("AnyEvilMushroom", 10)
                .AddIngredient(ItemID.ShroomiteBar, 5)
                .AddIngredient<RuinousSoul>(5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
