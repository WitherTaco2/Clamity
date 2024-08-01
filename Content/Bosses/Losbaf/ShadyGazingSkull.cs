using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Clamity.Content.Biomes.FrozenHell.Items;
using Clamity.Content.Bosses.Losbaf.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf
{
    public class ShadyGazingSkull : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.SummonBoss";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 7;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 48;
            Item.rare = ModContent.RarityType<Violet>();

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
            return !NPC.AnyNPCs(ModContent.NPCType<LosbafSuperboss>()) && !BossRushEvent.BossRushActive;
        }
        public override bool? UseItem(Player player)
        {
            int center = Main.maxTilesX * 16 / 2;
            //NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)player.Center.Y - 200, ModContent.NPCType<LosbafSuperboss>());

            if (Main.netMode != 1)
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<LosbafSuperboss>());
            else
                NetMessage.SendData(61, number: player.whoAmI, number2: (float)ModContent.NPCType<LosbafSuperboss>());

            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MechanicalSkull)
                .AddIngredient(ItemID.MechanicalEye, 2)
                .AddRecipeGroup("AnyAdamantiteBar", 10)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient<ShadowspecBar>()
                .AddIngredient<EnchantedMetal>(15)
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
}
