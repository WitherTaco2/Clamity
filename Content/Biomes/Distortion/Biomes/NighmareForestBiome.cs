using Clamity.Content.Biomes.Distortion.Tiles;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion.Biomes
{
    public class NighmareForestBiome : ModBiome
    {
        private string Texture => (GetType().Namespace + "." + nameof(NighmareForestBiome)).Replace('.', '/');
        //public override string MapBackground => BackgroundPath;
        //public override string BestiaryIcon => Texture + "_Icon";
        //public override string BackgroundPath => Texture + "_Map";
        //public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<NighmareForestBiomeTileCounter>().biomeTiles >= 20;
    }
    public class NighmareForestBiomeTileCounter : ModSystem
    {
        public int biomeTiles;
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            biomeTiles = tileCounts[ModContent.TileType<NightmareGrass>()];
        }
    }
}
