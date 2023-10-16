using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.Cryogen;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Clamity.Content.Boss.Pyrogen.NPCs;
using Microsoft.Xna.Framework;
using Clamity.Content.Items.Materials;

namespace Clamity.Content.Boss.Pyrogen
{
    public class PyroKey : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.SummonBoss";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[base.Type] = 7;
        }

        public override void SetDefaults()
        {
            base.Item.width = 26;
            base.Item.height = 48;
            base.Item.rare = 5;
            base.Item.useAnimation = 10;
            base.Item.useTime = 10;
            base.Item.useStyle = 4;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ZoneDesert && !NPC.AnyNPCs(ModContent.NPCType<PyrogenBoss>()))
            {
                return !BossRushEvent.BossRushActive;
            }

            return false;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(in SoundID.Roar, player.Center);
            if (Main.netMode != 1)
            {
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<PyrogenBoss>());
            }
            else
            {
                NetMessage.SendData(61, -1, -1, null, player.whoAmI, ModContent.NPCType<PyrogenBoss>());
            }

            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(value, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(value, base.Item.position - Main.screenPosition, null, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddRecipeGroup("AnySandBlock", 50).AddIngredient(520, 5).AddIngredient(521, 5)
                .AddIngredient<EssenceOfHeat>(8)
                .AddTile(16)
                .Register();
        }
    }
}
