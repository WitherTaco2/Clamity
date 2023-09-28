using CalamityMod;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using Clamity.Content.Cooldowns;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
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
    }
}
