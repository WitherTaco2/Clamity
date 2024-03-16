using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Materials
{
    public class NuclearEssence : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Materials";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.SortingPriorityMaterials[Type] = 117;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 1);
            Item.rare = ModContent.RarityType<PureGreen>();
        }
    }
}
