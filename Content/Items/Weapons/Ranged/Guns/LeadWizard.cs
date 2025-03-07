using CalamityMod.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Ranged.Guns
{
    public class LeadWizard : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;

            Item.useTime = Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.noMelee = true;

            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Bullet;

            Item.damage = 58;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 24;
            Item.knockBack = 5f;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return !Main.rand.NextBool(3);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.Bullet)
                type = ProjectileID.BulletHighVelocity;
            //velocity = velocity.RotatedBy(MathF.Sin(Main.GlobalTimeWrappedHourly * 4f) / 6f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathF.Sin(Main.GlobalTimeWrappedHourly * 6f) / 5f), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathF.Sin(Main.GlobalTimeWrappedHourly * 6f + 0.5f) / 5f), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathF.Sin(Main.GlobalTimeWrappedHourly * 6f + 1f) / 5f), type, damage, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2?(new Vector2(-10f, 0f));
    }
}
