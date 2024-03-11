using CalamityMod.Items.Accessories;
using Clamity.Content.Bosses.Clamitas.Drop;

namespace Clamity.Content.Items.Accessories
{
    public class RedDie : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories";

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Clamity().redDie = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<OldDie>()
                .AddIngredient<ClamitousPearl>(5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
