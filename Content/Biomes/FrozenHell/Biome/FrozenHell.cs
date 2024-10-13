using Clamity.Content.Biomes.FrozenHell.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Clamity.Commons.CalRemixCompatibilitySystem;

namespace Clamity.Content.Biomes.FrozenHell.Biome
{
    public class FrozenHell : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override string MapBackground => BackgroundPath;
        public override string BestiaryIcon => "Clamity/Content/Biomes/FrozenHell/Biome/FrozenHellIcon";
        public override string BackgroundPath => "Clamity/Content/Biomes/FrozenHell/Biome/FrozenHellMap";
        public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<FrozenHellTileCounter>().frozenAsh >= 120 && player.ZoneUnderworldHeight;
        public override int Music => Clamity.mod.GetMusicFromMusicMod("FrozenHell") ?? MusicID.OtherworldlySpace;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Clamity:FrozenHellSky", isActive);
        }

        public override void SetStaticDefaults()
        {
            var fanny1 = new FannyDialog("FrozenHell", "Sob").WithDuration(4f).WithCondition(_ => { return Main.LocalPlayer.Clamity().ZoneFrozenHell; });

            fanny1.Register();
        }

        public static void Convert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        /*if (wall != 0)
                            {
                                Main.tile[k, l].WallType = (ushort)ModContent.WallType<ExampleWall>();
                                WorldGen.SquareWallFrame(k, l);
                                NetMessage.SendTileSquare(-1, k, l, 1);
                            }*/

                        /*if (TileID.Sets.Conversion.Stone[type] || type == TileID.ClayBlock)
                            {
                                Main.tile[k, l].TileType = (ushort)ModContent.TileType<SwampMudNewTile>();
                                WorldGen.SquareTileFrame(k, l);
                                NetMessage.SendTileSquare(-1, k, l, 1);
                            }

                        if (TileID.Sets.Conversion.Dirt[type] || TileID.Sets.Conversion.Grass[type])
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<SwampMudNewTile>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }*/

                        if (Main.tile[k, l].TileType == TileID.Ash || Main.tile[k, l].TileType == TileID.AshGrass)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<FrozenAshTile>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }

                        if (Main.tile[k, l].TileType == TileID.Hellstone)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<FrozenHellstoneTile>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }

                        if (Main.tile[k, l].LiquidType == LiquidID.Lava)
                        {
                            Main.tile[k, l].LiquidAmount = 0;
                            Main.tile[k, l].TileType = TileID.BreakableIce;
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                    }
                }
            }
        }

    }
    public class FrozenHellTileCounter : ModSystem
    {
        public int frozenAsh;
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            frozenAsh = tileCounts[ModContent.TileType<FrozenAshTile>()];
        }
    }
}
