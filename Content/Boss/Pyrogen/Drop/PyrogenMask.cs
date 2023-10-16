using CalamityMod.Items.Armor.Vanity;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Pyrogen.Drop
{
    [AutoloadEquip(new EquipType[] { EquipType.Head })]
    public class PyrogenMask : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.Vanity";

        public override void SetStaticDefaults()
        {
            if (Main.netMode != 2)
            {
                ArmorIDs.Head.Sets.DrawHead[base.Item.headSlot] = false;
            }
        }

        public override void SetDefaults()
        {
            base.Item.width = 28;
            base.Item.height = 20;
            base.Item.rare = ItemRarityID.Blue;
            base.Item.vanity = true;
        }
    }
}
