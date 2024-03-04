using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Clamity.Content.Boss.Clamitas.Drop;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Clamity
{
    public class ClamitySystem : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult(ModContent.ItemType<TheAbsorber>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<MolluskHusk>());
                    recipe.AddIngredient<HuskOfCalamity>(5);
                }
                if (recipe.HasResult(ModContent.ItemType<TheAmalgam>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<MolluskHusk>());
                    recipe.AddIngredient<HuskOfCalamity>(10);
                }
                if (recipe.HasResult(ModContent.ItemType<AbyssalDivingSuit>()))
                {
                    recipe.RemoveIngredient(ModContent.ItemType<MolluskHusk>());
                    recipe.AddIngredient<HuskOfCalamity>(15);
                }
            }
        }
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
        internal static bool _downedProfusion;
        public static bool downedProfusion
        {
            get
            {
                return _downedProfusion;
            }
            set
            {
                if (!value)
                {
                    _downedProfusion = false;
                }
                else
                {
                    NPC.SetEventFlagCleared(ref _downedProfusion, -1);
                }
            }
        }
        internal static void ResetAllFlags()
        {
            downedClamitas = false;
            downedPyrogen = false;
            downedWallOfBronze = false;
            downedProfusion = false;
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
            if (downedProfusion)
                list.Add("profusion");
            tag["downedFlagsClamity"] = list;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> list = tag.GetList<string>("downedFlagsClamity");
            downedClamitas = list.Contains("clamitas");
            downedPyrogen = list.Contains("pyrogen");
            downedWallOfBronze = list.Contains("wob");
            downedProfusion = list.Contains("profusion");
        }
        public static int AnySandBlock;
        public static int AnyEvilMushroom;
        public override void AddRecipeGroups()
        {
            ClamitySystem.AnySandBlock = RecipeGroup.RegisterGroup("AnySandBlock", new RecipeGroup((Func<string>)(() => LangHelper.GetText("Misc.RecipeGroup.AnySandBlock")), new int[5]
            {
                ItemID.SandBlock, ItemID.EbonsandBlock, ItemID.PearlsandBlock, ItemID.CrimsandBlock, ModContent.ItemType<AstralSand>()
            }));
            ClamitySystem.AnyEvilMushroom = RecipeGroup.RegisterGroup("AnyEvilMushroom", new RecipeGroup((Func<string>)(() => LangHelper.GetText("Misc.RecipeGroup.AnyEvilMushroom")), new int[2]
            {
                ItemID.VileMushroom, ItemID.ViciousMushroom
            }));
        }
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedClamitas;
            flags[1] = downedPyrogen;
            flags[2] = downedWallOfBronze;

            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedClamitas = flags[0];
            downedPyrogen = flags[1];
            downedWallOfBronze = flags[2];
            downedProfusion = flags[3];
        }
    }
}
