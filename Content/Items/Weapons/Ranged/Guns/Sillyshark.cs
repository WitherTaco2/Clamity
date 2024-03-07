using Clamity.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Ranged.Guns
{
    public class Sillyshark : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override bool IsLoadingEnabled(Mod mod)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 64;
            Item.value = Item.sellPrice(0, 0, 16);
            Item.rare = ItemRarityID.Orange;

            Item.useTime = Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 20f;
            Item.noMelee = true;

            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 3.75f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(10))
            {
                int index = Projectile.NewProjectile(source, position, velocity, ProjectileID.DD2OgreSpit, damage, knockback, player.whoAmI);
                Main.projectile[index].friendly = true;
                Main.projectile[index].hostile = false;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bass)
                .AddIngredient<MediocreMatter>()
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }
    }
}
