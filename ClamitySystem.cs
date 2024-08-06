﻿using CalamityMod.Items.Placeables;
using CalamityMod.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Clamity
{
    public class ClamitySystem : ModSystem
    {
        public Dictionary<int, List<int>> enchantebleAccessories;

        internal static bool _downedClamitas;
        public static bool downedClamitas
        {
            get
            {
                return _downedClamitas;
            }
            set
            {
                if (!value)
                {
                    _downedClamitas = false;
                }
                else
                {
                    NPC.SetEventFlagCleared(ref _downedClamitas, -1);
                }
            }
        }
        internal static bool _downedPyrogen;
        public static bool downedPyrogen
        {
            get
            {
                return _downedPyrogen;
            }
            set
            {
                if (!value)
                {
                    _downedPyrogen = false;
                }
                else
                {
                    NPC.SetEventFlagCleared(ref _downedPyrogen, -1);
                }
            }
        }

        internal static bool _downedWallOfBronze;
        public static bool downedWallOfBronze
        {
            get
            {
                return _downedWallOfBronze;
            }
            set
            {
                if (!value)
                {
                    _downedWallOfBronze = false;
                }
                else
                {
                    NPC.SetEventFlagCleared(ref _downedWallOfBronze, -1);
                }
            }
        }

        internal static bool _downedLosbaf;
        public static bool downedLosbaf
        {
            get
            {
                return _downedLosbaf;
            }
            set
            {
                if (!value)
                {
                    _downedLosbaf = false;
                }
                else
                {
                    NPC.SetEventFlagCleared(ref _downedLosbaf, -1);
                }
            }
        }
        internal static void ResetAllFlags()
        {
            downedClamitas = false;
            downedPyrogen = false;
            downedWallOfBronze = false;
            downedLosbaf = false;
        }
        public override void Load()
        {
            enchantebleAccessories = new();
        }
        public override void Unload()
        {
            enchantebleAccessories = null;
        }
        private void AddEnchantebleAccessories(int acc, params int[] projList)
        {
            foreach (int i in projList)
            {
                enchantebleAccessories.Add(acc, projList.ToList<int>());
            }
        }
        public override void OnWorldLoad()
        {
            ResetAllFlags();

            static void AddEntry(string eventId, string songName, TimeSpan length, Func<bool> shouldPlay, Func<bool> enabled, TimeSpan? introSilence = null, TimeSpan? outroSilence = null)
            {
                MusicEventEntry entry = new(eventId, Clamity.mod.GetMusicFromMusicMod(songName).Value, length, introSilence ?? TimeSpan.Zero, outroSilence ?? TimeSpan.Zero, shouldPlay, enabled);
                MusicEventSystem.EventCollection.Add(entry);
            }

            AddEntry("LosbafDefeated", "LosbafPostFight", TimeSpan.FromSeconds(128d),
               () => ClamitySystem.downedLosbaf, () => ClamityConfig.Instance.LosbafIntelude);

        }
        public override void OnWorldUnload()
        {
            ResetAllFlags();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            List<string> list = new List<string>();
            if (downedClamitas)
                list.Add("clamitas");
            if (downedPyrogen)
                list.Add("pyrogen");
            if (downedWallOfBronze)
                list.Add("wob");
            if (downedLosbaf)
                list.Add("losbaf");
            tag["downedFlagsClamity"] = list;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> list = tag.GetList<string>("downedFlagsClamity");
            downedClamitas = list.Contains("clamitas");
            downedPyrogen = list.Contains("pyrogen");
            downedWallOfBronze = list.Contains("wob");
            downedLosbaf = list.Contains("losbaf");
        }
        public static int AnySandBlock;
        public static int AnyGemHook;
        public override void AddRecipeGroups()
        {
            ClamitySystem.AnySandBlock = RecipeGroup.RegisterGroup("AnySandBlock", new RecipeGroup((Func<string>)(() => LangHelper.GetText("Misc.RecipeGroup.AnySandBlock")), new int[5]
            {
                ItemID.SandBlock, ItemID.EbonsandBlock, ItemID.PearlsandBlock, ItemID.CrimsandBlock, ModContent.ItemType<AstralSand>()
            }));
            ClamitySystem.AnyGemHook = RecipeGroup.RegisterGroup("AnyGemHook", new RecipeGroup((Func<string>)(() => LangHelper.GetText("Misc.RecipeGroup.AnyGemHook")), new int[7]
            {
                ItemID.AmethystHook, ItemID.TopazHook, ItemID.SapphireHook, ItemID.EmeraldHook, ItemID.RubyHook, ItemID.AmberHook, ItemID.DiamondHook
            }));
        }
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedClamitas;
            flags[1] = downedPyrogen;
            flags[2] = downedWallOfBronze;
            flags[3] = downedLosbaf;

            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedClamitas = flags[0];
            downedPyrogen = flags[1];
            downedWallOfBronze = flags[2];
            downedLosbaf = flags[3];
        }
    }
}
