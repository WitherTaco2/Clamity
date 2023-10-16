using Terraria.ModLoader;
using Terraria;
using CalamityMod.Tiles.BaseTiles;

namespace Clamity.Content.Boss.Pyrogen.Drop
{
    public class PyrogenRelic : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Placeables";

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<PyrogenRelicTile>(), 0);
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 9999;
            Item.rare = -13;
            Item.master = true;
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }
    }
    public class PyrogenRelicTile : BaseBossRelic
    {
        public override string RelicTextureName => "Clamity/Content/Boss/Pyrogen/Drop/PyrogenRelicTile";
        public override int AssociatedItem => ModContent.ItemType<PyrogenRelic>();
    }
}
