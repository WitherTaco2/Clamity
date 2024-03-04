using CalamityMod;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using Clamity.Content.Cooldowns;
using Clamity.Content.Items.Accessories;
using Clamity.Content.Items.Mounts;
using Clamity.Content.Items.Potions.Food;
using Clamity.Content.Items.Weapons.Melee.Shortswords;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity
{
    public class ClamityGlobalItem : GlobalItem
    {
        /*public readonly int geode;
        public override void Load()
        {
            geode = ModContent.ItemType<NecromanticGeode>();
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            switch (item.type)
            {
                case geode:

                    break;
            }
        }*/
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

            if (!player.HasCooldown(ShortstrikeCooldown.ID))
            {

            }
            if (item.DamageType == ModContent.GetInstance<RogueDamageClass>())
            {
                if (player.Clamity().vampireEX && player.Calamity().StealthStrikeAvailable())
                {
                    //NPC npc = NPC
                    for (int i = 0; i < 10; i++)
                    {
                        Projectile.NewProjectile(player.GetSource_Accessory(item), player.Center, Main.rand.NextVector2CircularEdge(5, 5), ModContent.ProjectileType<DraculasCharmProj>(), 25, 0.1f, player.whoAmI, -1);
                    }
                }
            }

        }
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if ((item.DamageType == DamageClass.Melee || item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>()) && player.Clamity().inflicingMeleeFrostburn)
                target.AddBuff(BuffID.Frostburn, 180);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string str1 = "ItemExtraTooltip.Shortstrike.";
            switch (item.type)
            {
                case ItemID.CopperShortsword:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Copper")));
                    break;
                case ItemID.TinShortsword:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Tin")));
                    break;
                case ItemID.IronShortsword:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Iron")));
                    break;
                case ItemID.LeadShortsword:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Lead")));
                    break;
                case ItemID.SilverShortsword:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Silver")));
                    break;
                case ItemID.TungstenShortsword:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Tungsten")));
                    break;
                case ItemID.GoldShortsword:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Gold")));
                    break;
                case ItemID.PlatinumShortsword:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Platinum")));
                    break;
                case ItemID.Gladius:
                    tooltips.Add(new TooltipLine(Mod, "Shortstrike", LangHelper.GetText(str1 + "Gladius")));
                    break;
            }
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ModContent.ItemType<StarterBag>())
            {
                LeadingConditionRule leadingConditionRule = itemLoot.DefineConditionalDropSet((Func<bool>)(() => WorldGen.SavedOreTiers.Copper == 166));


                //Mod clicker = ModLoader.GetMod("ClickerClass");

                if (ModLoader.TryGetMod("ClickerClass", out Mod clicker))
                {
                    leadingConditionRule.Add(new CommonDrop(clicker.Find<ModItem>("CopperClicker").Item.type, 1));
                    leadingConditionRule.OnFailedConditions(new CommonDrop(clicker.Find<ModItem>("TinClicker").Item.type, 1));
                }
            }
            if (item.type == ModContent.ItemType<PlaguebringerGoliathBag>())
            {
                itemLoot.Add(ModContent.ItemType<Disease>(), 4);
                itemLoot.Add(ModContent.ItemType<PlagueStation>());
            }
            if (item.type == ModContent.ItemType<CalamitasCoffer>())
            {
                itemLoot.Add(ModContent.ItemType<Calamitea>(), 1, 10, 10);
            }
        }
    }
}
