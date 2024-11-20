using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.WoB.Drop
{
    [AutoloadEquip(new EquipType[] { EquipType.Head })]
    internal class WoBMask : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.Vanity";

        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}
