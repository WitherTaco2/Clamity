using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Rogue.FargosCrossover
{
    public class ToothyBomb : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 58;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = ItemRarityID.LightRed;

            Item.useTime = Item.useAnimation = 22;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 35;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.knockBack = 8f;

            Item.shoot = ModContent.ProjectileType<ToothyBombProjectile>();
            Item.shootSpeed = 12f;
        }
    }
    public class ToothyBombProjectile : BlastBarrelProjectile
    {
        public override string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');
    }
    public class ToothyBombShrapnel : ModProjectile
    {

    }
}
