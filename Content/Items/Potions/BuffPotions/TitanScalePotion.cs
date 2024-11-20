using CalamityMod.Items.Materials;
using Clamity.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions.BuffPotions
{
    public class TitanScalePotion : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.value = 4000;
            Item.rare = ItemRarityID.Yellow;

            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTime = Item.useAnimation = 17;
            Item.UseSound = SoundID.Item3;

            Item.consumable = true;
            Item.buffType = ModContent.BuffType<TitanScalePotionBuff>();
            Item.buffTime = 8 * 60 * 60;
        }
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemID.TitanPotion, 4)
                .AddIngredient(ItemID.BeetleHusk)
                .AddIngredient<CoralskinFoolfish>()
                .AddTile(TileID.AlchemyTable)
                .Register();
            CreateRecipe(4)
                .AddIngredient(ItemID.BottledWater, 4)
                .AddIngredient<BloodOrb>(40)
                .AddIngredient(ItemID.BeetleHusk)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
    public class TitanScalePotionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.Titan] = true;
            player.GetKnockback<MeleeDamageClass>() += 0.5f;
            player.statDefense += 5;
            player.endurance += 0.05f;

            player.Clamity().titanScale = true;
            if (player.Clamity().titanScaleTimer > 0)
            {
                player.statDefense += 20;
                player.endurance += 0.05f;
            }
        }
    }
}
