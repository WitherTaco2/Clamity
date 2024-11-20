using CalamityMod;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.KosFragmentBosses.CeasingVoid.Drops
{
    public class CeasingVoidTreasureBag : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.TreasureBags";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.BossBag[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Expert;
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
            return CalamityUtils.DrawTreasureBagInWorld(Item, spriteBatch, ref rotation, ref scale, whoAmI);
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            //Money
            //itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<CeasingVoid>()));

            //Weapons
            itemLoot.Add(ItemDropRule.FewFromOptionsNotScalingWithLuck(2, 1, ModContent.ItemType<SteelBlade>(), ModContent.ItemType<ConcentrationStaff>()));

            //Rev+ drop
            itemLoot.AddRevBagAccessories();

            //Mask
            itemLoot.Add(ModContent.ItemType<CeasingVoidMask>(), 7);
            //Thank You Painting
            itemLoot.Add(ModContent.ItemType<ThankYouPainting>(), 100);
        }
    }
}
