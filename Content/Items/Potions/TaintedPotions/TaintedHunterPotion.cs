using Clamity.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions.TaintedPotions
{
    public class TaintedHunterPotion : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Tainted";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Blue;

            Item.buffType = ModContent.BuffType<TaintedHunterPotionBuff>();
            Item.buffTime = 8 * 3600;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HunterPotion)
                .AddIngredient<DisgustingMeat>()
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
    public class TaintedHunterPotionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.aggro += 800;
        }
    }
}
