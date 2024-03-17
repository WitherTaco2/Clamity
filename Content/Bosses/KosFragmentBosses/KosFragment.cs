using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.KosFragmentBosses
{
    public class KosFragment : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.SummonBoss";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 19;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = null;
            Item.consumable = false;
            Item.rare = ItemRarityID.Orange;
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return (player.ZoneSkyHeight || player.ZoneUnderworldHeight || player.ZoneDungeon) &&
                !NPC.AnyNPCs(ModContent.NPCType<CeasingVoid.NPCs.CeasingVoid>()) && !BossRushEvent.BossRushActive;
        }
        public override bool? UseItem(Player player)
        {
            if (player.ZoneDungeon)
            {
                SoundEngine.PlaySound(RuneofKos.CVSound, player.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<CeasingVoid.NPCs.CeasingVoid>());
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<CeasingVoid.NPCs.CeasingVoid>());
            }

            /*else if (player.ZoneUnderworldHeight)
            {
                SoundEngine.PlaySound(RuneofKos.SignutSound, player.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Signus>());
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Signus>());
            }*/

            /*else if (player.ZoneSkyHeight)
            {
                SoundEngine.PlaySound(RuneofKos.StormSound, player.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<StormWeaverHead>());
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<StormWeaverHead>());
            }*/

            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> list)
        {
            Player player = Main.LocalPlayer;
            string line = this.GetLocalizedValue("SpawnInfo");
            if (player.ZoneDungeon)
                line = this.GetLocalizedValue("SpawnVoid");
            else if (player.ZoneUnderworldHeight)
                line = this.GetLocalizedValue("SpawnSignus");
            else if (player.ZoneSkyHeight)
                line = this.GetLocalizedValue("SpawnWeaver");
            list.FindAndReplace("[SPAWN]", line);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 25)
                .AddIngredient(ItemID.Hellstone, 10)
                .AddIngredient<DemonicBoneAsh>(2)
                .AddTile(TileID.BoneWelder)
                .Register();
        }
    }
}
