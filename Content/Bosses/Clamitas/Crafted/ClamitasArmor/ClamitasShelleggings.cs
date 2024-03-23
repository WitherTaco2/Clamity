using CalamityMod.Items;
using CalamityMod.Items.Armor.Mollusk;
using CalamityMod.Items.Materials;
using Clamity.Content.Bosses.Clamitas.Drop;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Clamity.Content.Bosses.Clamitas.Crafted.ClamitasArmor
{
    [AutoloadEquip(new EquipType[] { EquipType.Legs })]
    public class ClamitasShelleggings : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.Clamitas";
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 28;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.15f;
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
