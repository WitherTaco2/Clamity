using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.KosFragmentBosses.CeasingVoid.Drops
{
    public class TheUnchanging : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 42;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            //Item.defense = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Clamity().theUnchanging = true;
        }
    }
}
