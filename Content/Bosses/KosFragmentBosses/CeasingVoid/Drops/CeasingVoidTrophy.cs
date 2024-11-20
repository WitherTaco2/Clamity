using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Tiles.Furniture.BossTrophies;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.KosFragmentBosses.CeasingVoid.Drops
{
    public class CeasingVoidTrophy : CeaselessVoidTrophy
    {
        public override string Texture => ModContent.GetInstance<CeaselessVoidTrophy>().Texture;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.createTile = ModContent.TileType<CeasingVoidTrophyTile>();
        }
    }
    public class CeasingVoidTrophyTile : CeaselessVoidTrophyTile
    {
        public override string Texture => ModContent.GetInstance<CeaselessVoidTrophyTile>().Texture;
    }
}
