using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Clamity.Content.Biomes.FrozenHell.Items;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.FrozenHell.Items.FrozenHellstoneArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class FrozenHellstoneChestplate : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.FrozenHellstone";
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.defense = 80;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(ModContent.GetInstance<TrueMeleeDamageClass>()) += 0.2f;
            player.aggro += 400;
        }

        public override void AddRecipes() => CreateRecipe().AddIngredient<EnchantedMetal>(20).AddIngredient<EndothermicEnergy>(32).AddIngredient(232).AddTile(77).Register();
    }
}
