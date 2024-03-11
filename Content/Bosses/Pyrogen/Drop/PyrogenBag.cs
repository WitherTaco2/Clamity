﻿using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using Clamity.Content.Bosses.Pyrogen.Drop.Weapons;
using Clamity.Content.Bosses.Pyrogen.NPCs;
using Clamity.Content.Items.Materials;
using Terraria.GameContent.ItemDropRules;

namespace Clamity.Content.Bosses.Pyrogen.Drop
{
    public class PyrogenBag : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.TreasureBags";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.BossBag[base.Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossBags;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.4f);
        }

        public override void PostUpdate()
        {
            Item.TreasureBagLightAndDust();
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return CalamityUtils.DrawTreasureBagInWorld(base.Item, spriteBatch, ref rotation, ref scale, whoAmI);
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<PyrogenBoss>()));
            itemLoot.Add(ModContent.ItemType<EssenceOfFlame>(), 1, 10, 12);
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, ModContent.ItemType<SearedShredder>(), ModContent.ItemType<Obsidigun>(), ModContent.ItemType<TheGenerator>(), ModContent.ItemType<HellsBells>(), ModContent.ItemType<MoltenPiercer>()));
            //itemLoot.Add(ModContent.ItemType<GlacialEmbrace>(), 10);
            itemLoot.Add(ModContent.ItemType<SoulOfPyrogen>());
            //itemLoot.Add(ModContent.ItemType<CryoStone>(), DropHelper.BagWeaponDropRateFraction);
            //itemLoot.Add(ModContent.ItemType<FrostFlare>(), DropHelper.BagWeaponDropRateFraction);
            itemLoot.AddRevBagAccessories();
            itemLoot.Add(ModContent.ItemType<PyrogenMask>(), 7);
            itemLoot.Add(ModContent.ItemType<ThankYouPainting>(), 100);
        }
    }
}
