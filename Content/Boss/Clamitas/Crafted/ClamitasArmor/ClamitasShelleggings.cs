using CalamityMod.Items;
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
    [AutoloadEquip(new EquipType[] { EquipType.Legs })]
    public class ClamitasShelleggings : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.Clamitas";
        public override void SetDefaults()
        {
            base.Item.width = 22;
            base.Item.height = 18;
            base.Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            base.Item.rare = ItemRarityID.Lime;
            base.Item.defense = 31;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.2f;
            player.GetCritChance<GenericDamageClass>() += 9;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MolluskShelleggings>()
                .AddIngredient<HuskOfCalamity>(12)
                .AddIngredient<AshesofCalamity>(6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
