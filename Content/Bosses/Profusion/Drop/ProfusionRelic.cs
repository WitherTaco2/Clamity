using CalamityMod.Tiles.BaseTiles;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Profusion.Drop
{
    public class ProfusionRelic : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Placeables.Relics";

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<ProfusionRelicTile>(), 0);
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 9999;
            Item.rare = -13;
            Item.master = true;
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }
    }
    public class ProfusionRelicTile : BaseBossRelic
    {
        public override string RelicTextureName => "Clamity/Content/Bosses/Profusion/Drop/ProfusionRelicTile";
        public override int AssociatedItem => ModContent.ItemType<ProfusionRelic>();
    }
}
