using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Accessories
{
    public class SeaShell : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 46;
            Item.accessory = true;
            Item.value = Item.sellPrice(0, 0, 40);
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Clamity().seaShell = true;
        }
    }
}
