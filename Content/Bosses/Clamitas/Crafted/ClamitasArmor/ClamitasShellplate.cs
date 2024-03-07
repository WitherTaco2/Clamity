using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Mollusk;
using CalamityMod.Items.Materials;
using Clamity.Content.Boss.Clamitas.Drop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Clamitas.Crafted.ClamitasArmor
{
    [AutoloadEquip(new EquipType[] { EquipType.Body })]
    public class ClamitasShellplate : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.Clamitas";
        public override void SetDefaults()
        {
            base.Item.width = 30;
            base.Item.height = 22;
            base.Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            base.Item.rare = ItemRarityID.Lime;
            base.Item.defense = 22;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.15f;
            player.GetCritChance<GenericDamageClass>() += 4; 
            player.Calamity().giantPearl = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MolluskShellplate>()
                .AddIngredient<HuskOfCalamity>(20)
                .AddIngredient<AshesofCalamity>(8)
                .AddIngredient<GiantPearl>()
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
