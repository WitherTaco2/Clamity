using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Clamity.Content.Boss.Clamitas.Drop;
using CalamityMod.NPCs.Cryogen;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using CalamityMod;
using CalamityMod.Items.Placeables;

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
        internal static void ResetAllFlags()
        {
            downedClamitas = false;
            downedPyrogen = false;
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
            tag["downedFlagsClamity"] = list;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> list = tag.GetList<string>("downedFlagsClamity");
            downedClamitas = list.Contains("clamitas");
            downedPyrogen = list.Contains("pyrogen");
        }
        public static int AnySandBlock;
        public override void AddRecipeGroups()
        {
            ClamitySystem.AnySandBlock = RecipeGroup.RegisterGroup("AnySandBlock", new RecipeGroup((Func<string>)(() => LangHelper.GetText("Misc.RecipeGroup.AnySandBlock")), new int[5]
            {
                ItemID.SandBlock, ItemID.EbonsandBlock, ItemID.PearlsandBlock, ItemID.CrimsandBlock, ModContent.ItemType<AstralSand>()
            }));
        }
    }
}
