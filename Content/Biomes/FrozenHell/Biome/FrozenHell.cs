using Clamity.Content.Biomes.FrozenHell.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.FrozenHell.Biome
{
    public class FrozenHell : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override string MapBackground => BackgroundPath;
        public override string BestiaryIcon => "Clamity/Content/Boss/WoB/FrozenHell/Biome/FrozenHellIcon";
        public override string BackgroundPath => "Clamity/Content/Boss/WoB/FrozenHell/Biome/FrozenHellMap";
        public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<FrozenHellTileCounter>().frozenAsh >= 120 && player.ZoneUnderworldHeight;
        public override int Music => Clamity.mod.GetMusicFromMusicMod("FrozenHell") ?? MusicID.OtherworldlySpace;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Clamity:FrozenHellSky", isActive);
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
