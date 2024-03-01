using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.FrozenHell.Items.FrozenHellstoneArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class FrozenHellstoneVisor : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Armor.FrozenHellstone";

        public override void SetDefaults()
        {
            this.Item.width = 26;
            this.Item.height = 26;
            this.Item.value = Item.sellPrice(gold: 10);
            this.Item.rare = ModContent.RarityType<DarkBlue>();
            this.Item.defense = 75;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<FrozenHellstoneChestplate>() && legs.type == ModContent.ItemType<FrozenHellstoneGreaves>();

        public override void UpdateEquip(Terraria.Player player)
        {
            player.GetDamage(ModContent.GetInstance<TrueMeleeDamageClass>()) += 0.2f;
            player.Clamity().inflicingMeleeFrostburn = true;
        }

        public override void UpdateArmorSet(Terraria.Player player)
        {
            //player.setBonus = "Cannot be frozen.\nPress Armor Set Bonus to create an ice shield that parries attacks.[WIP]\nFailing to parry will cause you to overcool.[WIP]";
            player.buffImmune[44] = true;
            player.buffImmune[324] = true;
            player.buffImmune[47] = true;
            player.aggro += 400;
        }

        public override void AddRecipes() => CreateRecipe().AddIngredient<EnchantedMetal>(10).AddIngredient<EndothermicEnergy>(18).AddIngredient(ItemID.MoltenHelmet).AddTile(TileID.Hellforge).Register();
    }
}
