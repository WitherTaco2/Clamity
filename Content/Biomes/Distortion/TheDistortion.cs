using Clamity.Commons;
using Clamity.Content.Biomes.Distortion.Tiles;
using Luminance.Common.Utilities;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Clamity.Content.Biomes.Distortion
{
    public class TheDistortion : Subworld
    {
        public class TheDistortionShatteredIslandsPass : GenPass
        {
            public TheDistortionShatteredIslandsPass() : base("Terrain", 1f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                /*progress.Message = "Generating a Dragon Aeria";
                Main.worldSurface = SubworldHeight - 25;
                Main.rockLayer = SubworldHeight - 30;

                for (int i = 0; i < SubworldWidth; i++)
                {
                    for (int j = SubworldHeight - 200; j < SubworldHeight; j++)
                    {
                        Main.tile[i, j].TileType = TileID.Dirt;
                        Main.tile[i, j].Get<TileWallWireStateData>().HasTile = true;
                    }
                    Main.tile[i, SubworldHeight - 200].TileType = TileID.Grass;
                    Main.tile[i, SubworldHeight - 200].Get<TileWallWireStateData>().HasTile = true;
                }

                Main.spawnTileX = SubworldWidth / 4;
                Main.spawnTileY = SubworldHeight - 200;*/




                //WorldGen.genRand;
                progress.Message = "Shattering Islands";

                Main.worldSurface = SubworldHeight - 25;
                Main.rockLayer = SubworldHeight - 30;

                for (int i = SubworldWidth / 4; i < SubworldWidth * 3 / 4; i++)
                {
                    for (int j = SubworldHeight / 4; j < SubworldHeight - 100; j++)
                    {
                        Tile tile = Main.tile[i, j];
                        if (WorldGen.genRand.NextBool(1000))
                        {
                            CreateIsland(i, j);
                        }
                    }
                }
                CreateIsland(Main.spawnTileX, Main.spawnTileY + 1);
            }
            private void CreateIsland(int x, int y)
            {
                int maxWight = 8;
                for (int a = 0; a < maxWight; a++) //Y
                {
                    //UnifiedRandom random = new UnifiedRandom(WorldGen._genRandSeed + i + j);
                    for (int b = -(maxWight - a) / 2 - 1 - WorldGen.genRand.Next(2); b < (maxWight - a) / 2 + 1 + WorldGen.genRand.Next(2); b++) //X
                    {
                        if (!WorldGen.InWorld(x + b, y + a))
                            continue;
                        WorldGen.PlaceTile(x + b, y + a, (ushort)ModContent.TileType<EntropicSlagTile>());

                        /*Tile tile1 = Main.tile[i + b, j + a];
                        tile1.TileType = ModContent.TileType<EntropicSlagTile>();
                        tile1.Get<TileWallWireStateData>().HasTile = true;*/
                    }
                }
            }
        }
        public class TheDistortionNightmareForestPass : GenPass
        {
            public TheDistortionNightmareForestPass() : base("Terrain", 1f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                //WorldGen.genRand;
                progress.Message = "Filling Nightmare Fuel";

                Main.worldSurface = SubworldHeight - 25;
                Main.rockLayer = SubworldHeight - 30;

                for (int i = 100; i < SubworldWidth / 4; i += 100 + WorldGen.genRand.Next(30))
                {
                    for (int j = SubworldHeight / 4; j < SubworldHeight - 100; j += 100 + WorldGen.genRand.Next(90))
                    {
                        Tile tile = Main.tile[i, j];
                        if (WorldGen.genRand.NextBool(4))
                        {
                            CreateIsland(i, j);
                        }
                    }
                }
            }
            private void CreateIsland(int x, int y)
            {
                /*
                int maxWight = 8;
                for (int a = 0; a < maxWight; a++) //Y
                {
                    for (int b = -(maxWight - a) / 2 - 1 - WorldGen.genRand.Next(2); b < (maxWight - a) / 2 + 1 + WorldGen.genRand.Next(2); b++) //X
                    {
                        if (!WorldGen.InWorld(x + b, y + a))
                            continue;
                        WorldGen.PlaceTile(x + b, y + a, (ushort)ModContent.TileType<EntropicSlagTile>());
                    }
                }
                */
                int maxWight = 100;

                /*bool f = false;
                for (int a = -100; a < 0; a++)
                {
                    if (f)
                        break;
                    for (int b = -maxWight / 2; b <= maxWight / 2; b++)
                    {
                        if (WorldGen.InWorld(x + b, y + a))
                        {
                            if (Main.tile[x + b, y + a].TileType > 0)
                            {
                                f = true; break;
                            }
                        }
                    }
                }
                if (f)
                    return;*/

                for (int i = -maxWight / 2; i <= maxWight / 2; i++) //X
                {
                    if (!WorldGen.InWorld(x + i, y))
                        continue;
                    WorldGen.PlaceTile(x + i, y, (ushort)ModContent.TileType<NightmareGrass>());

                    Tile tile = Framing.GetTileSafely(x + i, y); // Safely get the tile at the given coordinates
                    bool growSuccess = WorldGen.GrowTree(x + i, y); ; // A bool to see if the tree growing was successful.

                    //If growing the tree was a success and the player is near, show growing effects
                    if (growSuccess && WorldGen.genRand.NextBool(10))
                    {
                        WorldGen.TreeGrowFXCheck(x + i, y);
                    }
                }
                int num = 1;
                for (int j = 0; j < 8; j++) //Y
                {
                    for (int i = -maxWight / 2 + j * num; i <= maxWight / 2 - j * num; i++) //X
                    {
                        if (!WorldGen.InWorld(x + i, y + j + 1))
                            continue;
                        WorldGen.PlaceTile(x + i, y + j + 1, (ushort)ModContent.TileType<EntropicSlagTile>());
                    }
                    num++;
                }

            }
        }
        public override List<GenPass> Tasks => new List<GenPass> { new TheDistortionShatteredIslandsPass(), new TheDistortionNightmareForestPass() };
        public static int SubworldWidth => 4000;
        public static int SubworldHeight => 1800;
        public override int Width => SubworldWidth;
        public override int Height => SubworldHeight;
        //public override bool ShouldSave => true;




        #region World Data Management
        public static TagCompound SafeWorldDataToTag(string suffix, bool saveInCentralRegistry = true)
        {
            // Re-initialize the save data tag.
            TagCompound savedWorldData = [];

            // Save Calamity's boss defeat data.
            CalamityVariablesSystem.SaveDefeatStates(savedWorldData);

            // Store the tag.
            if (saveInCentralRegistry)
                SubworldSystem.CopyWorldData($"DragonAeriaSavedWorldData_{suffix}", savedWorldData);

            return savedWorldData;
        }

        private void LoadWorldDataFromTag(string suffix, TagCompound specialTag = null)
        {
            TagCompound savedWorldData = specialTag ?? SubworldSystem.ReadCopiedWorldData<TagCompound>($"DragonAeriaSavedWorldData_{suffix}");

            //Load Calamity's boss defeat data.
            CalamityVariablesSystem.LoadDefeatStates(savedWorldData);
        }

        public override void CopyMainWorldData() => SafeWorldDataToTag("Main");

        public override void ReadCopiedMainWorldData() => LoadWorldDataFromTag("Main");

        public override void CopySubworldData() => SafeWorldDataToTag("Subworld");

        public override void ReadCopiedSubworldData() => LoadWorldDataFromTag("Subworld");
        #endregion
    }
}
