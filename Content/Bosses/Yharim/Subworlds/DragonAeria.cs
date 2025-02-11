using Clamity.Commons;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Clamity.Content.Bosses.Yharim.Subworlds
{
    public class DragonAeria : Subworld
    {
        public class DragonAeriaPass : GenPass
        {
            public DragonAeriaPass() : base("Terrain", 1f) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Generating a Dragon Aeria";
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
                Main.spawnTileY = SubworldHeight - 200;
            }
        }
        public override List<GenPass> Tasks => new List<GenPass> { new DragonAeriaPass() };
        public static int SubworldWidth => 2000;
        public static int SubworldHeight => 700;
        public override int Width => SubworldWidth;
        public override int Height => SubworldHeight;

        public static bool HasYharimAppeared
        {
            get;
            set;
        }

        public static bool HasYharimBeenDefeated
        {
            get;
            set;
        }

        public static bool HasYharimBeenTalkedBefore
        {
            get;
            set;
        }


        #region World Data Management
        public static TagCompound SafeWorldDataToTag(string suffix, bool saveInCentralRegistry = true)
        {
            // Re-initialize the save data tag.
            TagCompound savedWorldData = [];

            // Save Calamity's boss defeat data.
            CalamityVariablesSystem.SaveDefeatStates(savedWorldData);

            // Save Yharim
            if (ClamitySystem.downedYharim)
                savedWorldData[nameof(Yharim)] = true;

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

            //Load Yharim
            ClamitySystem.downedYharim = savedWorldData.ContainsKey(nameof(Yharim));
        }

        public override void CopyMainWorldData() => SafeWorldDataToTag("Main");

        public override void ReadCopiedMainWorldData() => LoadWorldDataFromTag("Main");

        public override void CopySubworldData() => SafeWorldDataToTag("Subworld");

        public override void ReadCopiedSubworldData() => LoadWorldDataFromTag("Subworld");
        #endregion
    }
}
