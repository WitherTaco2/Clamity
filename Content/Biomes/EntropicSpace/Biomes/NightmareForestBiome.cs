using Clamity.Content.Biomes.EntropicSpace.Tiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.EntropicSpace.Biomes
{
    public class NightmareForestBiome : ModBiome
    {
        private string Texture => (GetType().Namespace + "." + nameof(NightmareForestBiome)).Replace('.', '/');
        public override string MapBackground => BackgroundPath;
        public override string BackgroundPath => Texture + "_Map";
        public override string BestiaryIcon => Texture + "_Icon";
        public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => player.Clamity().ZoneNightmareForest;
    }
    public class NightmareForestBiomeTileCounter : ModSystem
    {
        public int biomeTiles;
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            biomeTiles = tileCounts[ModContent.TileType<NightmareGrass>()];
        }
    }
}
