using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.Drop
{
    [AutoloadEquip(new EquipType[] { EquipType.Head })]
    internal class WoBMask : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.Vanity";

        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                ArmorIDs.Head.Sets.DrawHead[base.Item.headSlot] = false;
            }
        }

        public override void SetDefaults()
        {
            base.Item.width = 18;
            base.Item.height = 22;
            base.Item.rare = ItemRarityID.Blue;
            base.Item.vanity = true;
        }
    }
}
