using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.FrozenHell.Items.FrozenHellstoneArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class FrozenHellstoneChestplate : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.FrozenHellstone";
        public override void SetDefaults()
        {
            this.Item.width = 44;
            this.Item.height = 22;
            this.Item.value = Item.sellPrice(gold: 10);
            this.Item.rare = ModContent.RarityType<DarkBlue>();
            this.Item.defense = 80;
        }

        public override void UpdateEquip(Terraria.Player player)
        {
            player.GetDamage((DamageClass)ModContent.GetInstance<TrueMeleeDamageClass>()) += 0.2f;
            player.aggro += 400;
        }

        public override void AddRecipes() => this.CreateRecipe().AddIngredient<EnchantedMetal>(20).AddIngredient<EndothermicEnergy>(32).AddIngredient(232).AddTile(77).Register();
    }
}
