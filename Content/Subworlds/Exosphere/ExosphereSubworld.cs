using CalamityMod.Tiles.FurnitureExo;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Clamity.Content.Subworlds.Exosphere
{
    public class ExosphereSubworld : Subworld
    {
        public override int Width => 4000;
        public override int Height => 2000;
        public override bool ShouldSave => false;
        public override bool NoPlayerSaving => false;
        public override bool NormalUpdates => false;
        public override List<GenPass> Tasks => new()
        {
            new TestPass0("Pre-Generation", 1f),
            new TestPass1("Generating Platforms", 1f)
        };
        /*public override void OnEnter()
        {
            Main.numClouds = 0;
            Main.numCloudsTemp = 0;
            Main.cloudBGAlpha = 0f;

            Main.cloudAlpha = 0f;
            Main.cloudBGAlpha = 0;
            Main.resetClouds = true;
            Main.moonPhase = 4;

            Main.rainTime = 0;
            Main.raining = false;
            Main.maxRaining = 0f;
            Main.slimeRain = false;
        }*/
    }
    public class TestPass0 : GenPass
    {
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            WorldGenerator.CurrentGenerationProgress = progress;

            progress.Message = "Loading";
            //WorldGen.noTileActions = true;
            Main.spawnTileX = 1000 / 16;
            Main.spawnTileY = 1000 / 16;
            Main.worldSurface = Main.maxTilesY - 42;
            Main.rockLayer = Main.maxTilesY + 42;

        }
        public TestPass0(string name, float loadWeight) : base(name, loadWeight)
        {

        }
    }
    public class TestPass1 : GenPass
    {
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            WorldGenerator.CurrentGenerationProgress = progress;

            progress.Message = "Placing Platforms";
            for (int i = 1; i < 4000 - 41; i++)
            {
                for (int j = 1; j < 2000 - 1; j++)
                {
                    //progress.Value = ((float)i * Main.maxTilesY + j) / (Main.maxTilesX * Main.maxTilesY);
                    if (j % 15 == 14)
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<ExoPlatformTile>());
                    }
                }
            }
        }
        public TestPass1(string name, float loadWeight) : base(name, loadWeight)
        {

        }
    }
}
