﻿using Clamity.Content.Boss.WoB.FrozenHell.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.FrozenHell.Biome
{
    public class FrozenHell : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override string MapBackground => BackgroundPath;
        public override string BestiaryIcon => "Clamity/Content/Boss/WoB/FrozenHell/Biome/FrozenHellIcon";
        public override string BackgroundPath => "Clamity/Content/Boss/WoB/FrozenHell/Biome/FrozenHellMap";
        public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<FrozenHellTileCounter>().frozenAsh >= 120 && player.ZoneUnderworldHeight;
        public override int Music => /*Clamity.mod.GetMusicFromMusicMod("") ??*/ MusicID.OtherworldlySpace;
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