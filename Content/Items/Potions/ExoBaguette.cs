﻿using CalamityMod;
using CalamityMod.Buffs.Alcohol;
using CalamityMod.Buffs.Potions;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Clamity.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions
{
    //Increase threshold of alchohol poisoning by 1 buff
    public class ExoBaguette : Baguette, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Foods";
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<ExoBaguetteBuff>();
            Item.rare = ModContent.RarityType<Violet>();
            Item.value += Terraria.Item.sellPrice(0, 2, 40) + ModContent.GetInstance<ExoPrism>().Item.value + ModContent.GetInstance<AuricBar>().Item.value;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SoulBaguette>()
                .AddIngredient<ExoPrism>()
                .AddIngredient<AuricBar>()
                .AddTile<DraedonsForge>()
                .Register();
        }
        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<ExoBaguetteBuff>(), CalamityUtils.SecondsToFrames(300f));
            player.AddBuff(ModContent.BuffType<BaguetteBuff>(), CalamityUtils.SecondsToFrames(600f));
            player.AddBuff(BuffID.WellFed3, CalamityUtils.SecondsToFrames(300f));
        }
    }
}
