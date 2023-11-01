using Clamity.Content.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;

namespace Clamity.Content.Items.Tools.Bags.Fish
{
    public class FrontGar : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Fishing";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
            ItemID.Sets.CanBePlacedOnWeaponRacks[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ModContent.RarityType<PureGreen>();
        }
        public override bool CanRightClick() => true;
        public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ReaperTooth>(), 5, 15));
    }
}
