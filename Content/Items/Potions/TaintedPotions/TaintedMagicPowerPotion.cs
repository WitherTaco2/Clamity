using Clamity.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions.TaintedPotions
{
    public class TaintedMagicPowerPotion : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Tainted";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Blue;

            Item.buffType = ModContent.BuffType<TaintedMagicPowerPotionBuff>();
            Item.buffTime = 4 * 3600;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MagicPowerPotion)
                .AddIngredient<DisgustingMeat>()
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
    public class TaintedMagicPowerPotionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<MagicDamageClass>() -= 0.2f;
            player.Clamity().taintedMagicPower = true;
        }
    }
}
