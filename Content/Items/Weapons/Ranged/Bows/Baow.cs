using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Items;

namespace Clamity.Content.Items.Weapons.Ranged.Bows
{
    public class Baow : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 64;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 18;

            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 14;

            Item.damage = 27;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 3;
        }
        //private int count;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*if (count == 0)
            {
                count++;
                return true;
            }
            else
            {
                Vector2 vec1 = Main.MouseWorld - position;
                vec1.X = Math.Clamp(vec1.X, -50, 50);
                vec1.Y = Math.Clamp(vec1.Y, -50, 50);
                Projectile.NewProjectile(source, Main.MouseWorld + vec1, -velocity, type, damage, knockback, player.whoAmI);
                count = 0;
                return false;
            }*/
            Vector2 vec1 = Main.MouseWorld - position;
            vec1.X = Math.Clamp(vec1.X, -250, 250);
            vec1.Y = Math.Clamp(vec1.Y, -250, 250);
            Projectile.NewProjectile(source, Main.MouseWorld + vec1, -velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2?(new Vector2(-5f, 0.0f));
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DarkShard)
                .AddIngredient(ItemID.LightShard)
                .AddIngredient(ItemID.SoulofNight, 4)
                .AddIngredient(ItemID.SoulofLight, 4)
                .AddTile(TileID.Anvils)
                .Register();

        }
    }
} 
