using Clamity.Commons;
using Clamity.Content.Biomes.EntropicSpace.Tiles;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Clamity.Content.Biomes.EntropicSpace
{
    public class EntropicSpaceSubworld : Subworld
    {
        public class ShatteredIslandsPass : GenPass
        {
            public ShatteredIslandsPass() : base("Terrain", 1f) { }

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
                    for (int j = DurksunHeight; j < SubworldHeight - 100; j++)
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
                        WorldGen.PlaceTile(x + b, y + a, WorldGen.genRand.NextBool(10) ? ModContent.TileType<CosmiliteOreTile>() : ModContent.TileType<EntropicSlagTile>());

                        /*Tile tile1 = Main.tile[i + b, j + a];
                        tile1.TileType = ModContent.TileType<EntropicSlagTile>();
                        tile1.Get<TileWallWireStateData>().HasTile = true;*/
                    }
                }

                //Old Ore Gen

                /*int oreCenterX = x + WorldGen.genRand.Next(-3, 3), oreCenterY = y + 1 + WorldGen.genRand.Next(0, 3);
                bool isGenerateOre = WorldGen.genRand.NextBool(2);
                if (isGenerateOre)
                {
                    for (int a = -1; a <= 1 + WorldGen.genRand.Next(2); a++)
                    {
                        for (int b = -1; b <= 1 + WorldGen.genRand.Next(2); b++)
                        {
                            if (!WorldGen.InWorld(oreCenterX + a, oreCenterY + b))
                                continue;
                            Tile tile1 = Main.tile[oreCenterX + a, oreCenterY + b];
                            if (tile1.HasTile)
                            {
                                Main.tile[oreCenterX + a, oreCenterY + b].TileType = (ushort)ModContent.TileType<CosmiliteOreTile>();
                                WorldGen.SquareTileFrame(oreCenterX + a, oreCenterY + b);
                                NetMessage.SendTileSquare(-1, oreCenterX + a, oreCenterY + b, 1);

                                //WorldGen.PlaceTile(oreCenterX + a, oreCenterY + b, ModContent.TileType<CosmiliteOreTile>());
                            }
                        }
                    }
                }*/
            }
        }
        public class NightmareForestPass : GenPass
        {
            public NightmareForestPass() : base("Terrain", 1f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                //WorldGen.genRand;
                progress.Message = "Filling Nightmare Fuel";

                Main.worldSurface = SubworldHeight - 25;
                Main.rockLayer = SubworldHeight - 30;

                for (int i = 100; i < SubworldWidth / 4; i += 100 + WorldGen.genRand.Next(30))
                {
                    for (int j = DurksunHeight; j < SubworldHeight - 100; j += 100 + WorldGen.genRand.Next(50))
                    {
                        Tile tile = Main.tile[i, j];
                        if (WorldGen.genRand.NextBool(3))
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
                    WorldGen.PlaceTile(x + i, y, ModContent.TileType<NightmareGrass>());

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
                        WorldGen.PlaceTile(x + i, y + j + 1, WorldGen.genRand.NextBool(10) ? ModContent.TileType<CosmiliteOreTile>() : ModContent.TileType<EntropicSlagTile>());
                    }
                    num++;
                }
            }
        }
        public class EndothermicMountainsPass : GenPass
        {
            public class PlaceSplotches : GenAction
            {
                private int _chance;
                private float _strenght;
                private int _step;
                private int _type;
                public PlaceSplotches(int chance, float strenght, int step, int type)
                {
                    _chance = chance;
                    _strenght = strenght;
                    _step = step;
                    _type = type;
                }
                public override bool Apply(Point origin, int x, int y, params object[] args)
                {
                    //WorldUtils.TileFrame(x, y, _frameNeighbors);
                    if (WorldGen.genRand.NextBool(_chance))
                        WorldGen.TileRunner(x, y, _strenght, _step, _type);
                    return UnitApply(origin, x, y, args);
                }
            }
            public EndothermicMountainsPass() : base("Terrain", 1f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Frozing a Planets";

                //Feldy, need make gen in here
                //Mountaing is on right side of world
                //Between SubworldWidth * 3 / 4 and SubworldWidth

                //Also planned generation of draedon lab in mountain
                //Used an Structure Helper

                float centinWithinZoneX = MathHelper.Lerp(SubworldWidth * 3 / 4, SubworldWidth, 0.5f);
                int planet1CenterX = (int)MathHelper.Lerp(SubworldWidth * 3 / 4, centinWithinZoneX, 0.75f);
                int planet2CenterX = (int)MathHelper.Lerp(SubworldWidth, centinWithinZoneX, 0.75f);

                CreatePlanetoid(planet1CenterX, SubworldHeight * 3 / 4, (int)(SubworldWidth / 4 / 3));
            }
            private void CreatePlanetoid(int x, int y, int radius)
            {
                WorldUtils.Gen(new Point(x, y), new Shapes.Circle(radius), Actions.Chain(new GenAction[]
                {
                    new Modifiers.Blotches(4, 0.5),
                    new Actions.PlaceTile((ushort)ModContent.TileType<EndothermicSnowTile>()),
                    new Actions.SetFrames()
                }));
                WorldUtils.Gen(new Point(x, y), new Shapes.Circle(radius - 25), Actions.Chain(new GenAction[]
                {
                    //new Modifiers.Blotches(4, 0.5),
                    new Actions.ClearTile(),
                    new Actions.PlaceTile((ushort)ModContent.TileType<EndothermicSnowTile>()),
                    new PlaceSplotches(25, WorldGen.genRand.NextFloat(3f, 6f), WorldGen.genRand.Next(7, 17), ModContent.TileType<EndothermicIceTile>())
                    //new Actions.PlaceTile((ushort)ModContent.TileType<EndothermicIceTile>()),
                    //new Actions.SetFrames()
                }));

            }
        }

        public class DarksunPass : GenPass
        {
            public DarksunPass() : base("Terrain", 1f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Filling dark to sun";

                //I don t have ideas for unique gen for it
            }
        }

        public class ShrinePass : GenPass
        {
            public ShrinePass() : base("Terrain", 1f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Placing a Cosmic Treasure";

                //Structure Helper mod things
            }
        }
        public override List<GenPass> Tasks => new List<GenPass> { new ShatteredIslandsPass(), new NightmareForestPass(), new EndothermicMountainsPass(), new DarksunPass() };
        public static int SubworldWidth => 4000;
        public static int SubworldHeight => 1800;
        public static int DurksunHeight => SubworldHeight / 4;
        public override int Width => SubworldWidth;
        public override int Height => SubworldHeight;

        //TODO - remove commending after completing an generation
        //public override bool ShouldSave => true;

        public override bool NormalUpdates => true;

        /*public override void DrawMenu(GameTime gameTime)
        {
            DistortionSky.SetDistortionBG();

            Vector2 textPosition = new Vector2(Main.screenWidth, Main.screenHeight) / 2f - FontAssets.DeathText.Value.MeasureString(Main.statusText) / 2f;

            Main.spriteBatch.DrawString(FontAssets.DeathText.Value, Main.statusText, textPosition, Color.White);

        }*/


        #region World Data Management
        public static TagCompound SafeWorldDataToTag(string suffix, bool saveInCentralRegistry = true)
        {
            // Re-initialize the save data savedWorldData.
            TagCompound savedWorldData = [];

            // Save difficulty data. This is self-explanatory.
            bool revengeanceMode = CalamityVariablesSystem.RevengeanceModeActive;
            bool deathMode = CalamityVariablesSystem.DeathModeActive;
            if (revengeanceMode)
                savedWorldData["RevengeanceMode"] = revengeanceMode;
            if (deathMode)
                savedWorldData["DeathMode"] = deathMode;
            if (Main.zenithWorld)
                savedWorldData["GFB"] = Main.zenithWorld;

            // Save Calamity's boss defeat data.
            CalamityVariablesSystem.SaveDefeatStates(savedWorldData);
            //if (DownedBossSystem.downedDoG)
            //    savedWorldData["DoG"] = true;

            // Store the savedWorldData.
            if (saveInCentralRegistry)
                SubworldSystem.CopyWorldData($"TheDistortionSavedWorldData_{suffix}", savedWorldData);

            return savedWorldData;
        }

        private void LoadWorldDataFromTag(string suffix, TagCompound specialTag = null)
        {
            TagCompound savedWorldData = specialTag ?? SubworldSystem.ReadCopiedWorldData<TagCompound>($"TheDistortionSavedWorldData_{suffix}");

            /*if (savedWorldData.ContainsKey("DoG") || savedWorldData.GetBool("DoG"))
            {
                //CalamityVariablesSystem.SetDownedValue("_downedDoG", true);
                //DownedBossSystem.downedDoG = true;
                typeof(DownedBossSystem).GetField("_downedDoG", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, true);
            }*/

            CalamityVariablesSystem.RevengeanceModeActive = savedWorldData.ContainsKey("RevengeanceMode");
            CalamityVariablesSystem.DeathModeActive = savedWorldData.ContainsKey("DeathMode");
            Main.zenithWorld = savedWorldData.ContainsKey("GFB");

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
