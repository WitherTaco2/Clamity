using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.FrozenHell.Items.FrozenHellstoneArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class FrozenHellstoneGreaves : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.FrozenHellstone";
        public override void SetDefaults()
        {
            this.Item.width = 44;
            this.Item.height = 22;
            this.Item.value = Item.sellPrice(gold: 10);
            this.Item.rare = ModContent.RarityType<DarkBlue>();
            this.Item.defense = 45;
        }

        public override void UpdateEquip(Terraria.Player player)
        {
            player.GetDamage((DamageClass)ModContent.GetInstance<TrueMeleeDamageClass>()) += 0.2f;
            player.statLifeMax2 += 75;
            player.aggro += 400;
        }

        public override void AddRecipes() => CreateRecipe().AddIngredient<EnchantedMetal>(15).AddIngredient<EndothermicEnergy>(24).AddIngredient(ItemID.MoltenGreaves).AddTile(TileID.Hellforge).Register();
    }
}
