using Terraria.ModLoader;
using Terraria;
using CalamityMod.Tiles.BaseTiles;

namespace Clamity.Content.Boss.WoB.Drop
{
    public class WoBRelic : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Placeables.Relics";

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<WoBRelicTile>(), 0);
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 9999;
            Item.rare = -13;
            Item.master = true;
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }
    }
    public class WoBRelicTile : BaseBossRelic
    {
        public override string RelicTextureName => "Clamity/Content/Boss/WoB/Drop/WoBRelicTile";
        public override int AssociatedItem => ModContent.ItemType<WoBRelic>();
    }

}
