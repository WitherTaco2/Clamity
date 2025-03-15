using Clamity.Content.Biomes.EntropicSpace.Tiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.EntropicSpace.Biomes
{
    public class EndothermicPlanetoidsBiome : ModBiome
    {
        private string Texture => (GetType().Namespace + "." + nameof(ShatteredIslandsBiome)).Replace('.', '/');
        public override string MapBackground => BackgroundPath;
        public override string BackgroundPath => Texture + "_Map";
        public override string BestiaryIcon => Texture + "_Icon";
        public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => player.Clamity().ZoneEndothermicPlanetoids;
        public override int Music => Clamity.mod.GetMusicFromMusicMod("EntropicSpace/ShatteredIslands") ?? MusicID.OtherworldlySpace;

    }
    public class EndothermicPlanetoidsCounter : ModSystem
    {
        public int biomeTiles;
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            biomeTiles = tileCounts[ModContent.TileType<EndothermicIceTile>()]
                        + tileCounts[ModContent.TileType<EndothermicSnowTile>()];
        }
    }
}
