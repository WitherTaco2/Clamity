using CalamityMod.Items.Placeables;
using Clamity.Content.Bosses.Yharim.NPCs;
using Clamity.Content.Bosses.Yharim.Subworlds;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
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
        internal static bool _generatedFrozenHell;
        public static bool generatedFrozenHell
        {
            get
            {
                return _generatedFrozenHell;
            }
            set
            {
                if (!value)
                {
                    _generatedFrozenHell = false;
                }
                else
                {
                    NPC.SetEventFlagCleared(ref _generatedFrozenHell, -1);
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
        internal static bool _isYharimFirstTalking;
        public static bool isYharimFirstTalking
        {
            get
            {
                return _isYharimFirstTalking;
            }
            set
            {
                if (!value)
                {
                    _isYharimFirstTalking = false;
                }
                else
                {
                    NPC.SetEventFlagCleared(ref _isYharimFirstTalking, -1);
                }
            }
        }
        internal static bool _downedYharim;
        public static bool downedYharim
        {
            get
            {
                return _downedYharim;
            }
            set
            {
                if (!value)
                {
                    _downedYharim = false;
                }
                else
                {
                    NPC.SetEventFlagCleared(ref _downedYharim, -1);
                }
            }
        }
        internal static void ResetAllFlags()
        {
            downedClamitas = false;
            downedPyrogen = false;
            downedWallOfBronze = false;
            downedYharim = false;
            generatedFrozenHell = false;
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
            if (downedYharim)
                list.Add("yharim");
            tag["downedFlagsClamity"] = list;
            tag["generatedFrozenHell"] = generatedFrozenHell;
            tag["isYharimFirstTalking"] = isYharimFirstTalking;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> list = tag.GetList<string>("downedFlagsClamity");
            downedClamitas = list.Contains("clamitas");
            downedPyrogen = list.Contains("pyrogen");
            downedWallOfBronze = list.Contains("wob");
            downedYharim = list.Contains("yharim");
            generatedFrozenHell = tag.GetBool("generatedFrozenHell");
            isYharimFirstTalking = tag.GetBool("isYharimFirstTalking");
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
            flags[3] = downedYharim;

            writer.Write(flags);

            writer.Write(generatedFrozenHell);
            writer.Write(isYharimFirstTalking);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedClamitas = flags[0];
            downedPyrogen = flags[1];
            downedWallOfBronze = flags[2];
            downedYharim = flags[3];

            generatedFrozenHell = reader.ReadBoolean();
            isYharimFirstTalking = reader.ReadBoolean();
        }
        public override void PostUpdateEverything()
        {
            bool inDragonAeria = SubworldSystem.IsActive<DragonAeria>();
            if (!inDragonAeria)
            {
                DragonAeria.HasYharimAppeared = false;

                if (DragonAeria.HasYharimBeenDefeated)
                {
                    DragonAeria.HasYharimBeenDefeated = false;

                    NPC fakeNPC = new();
                    fakeNPC.SetDefaults(ModContent.NPCType<Yharim>());
                    for (int i = 0; i < 100; i++)
                        Main.BestiaryTracker.Kills.RegisterKill(fakeNPC);

                }
            }
            else
            {

                CalamityMod.CalamityMod.StopRain();
                LanternNight.WorldClear();

                for (int i = 0; i < Main.maxClouds; i++)
                {
                    Main.cloud[i].position = Vector2.One * -10000f;
                    Main.cloud[i].active = false;
                }
            }

            if (!DragonAeria.HasYharimAppeared && inDragonAeria && !Main.LocalPlayer.dead)
            {
                int x = DragonAeria.SubworldWidth / 2;
                int y = DragonAeria.SubworldHeight - 200;

                if (Main.netMode is not NetmodeID.MultiplayerClient)
                    NPC.NewNPC(new EntitySource_WorldEvent(), x * 16, y * 16, ModContent.NPCType<Yharim>(), 1);

                DragonAeria.HasYharimAppeared = true;
            }

        }
    }
}
