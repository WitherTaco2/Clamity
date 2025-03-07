using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Accessories.GemCrawlerDrop
{
    public class SharpAmethyst : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.value = Item.sellPrice(0, 0, 20);
            Item.rare = ItemRarityID.Blue;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Clamity().gemAmethyst = true;
        }

    }
    public class SharpAmethystProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Classless";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 2;
        }
    }
}
