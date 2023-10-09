using CalamityMod.Items;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace Clamity.Content.Items.Weapons.Magic
{
    public class OmegaRay : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";

        public override void SetDefaults()
        {
            Item.damage = 210;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 106;
            Item.height = 130;
            Item.useTime = this.Item.useAnimation = 4;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.UseSound = new SoundStyle?(SoundID.Item33);
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<BigBeamofDeath>();
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override Vector2? HoldoutOffset() => new Vector2?(new Vector2(-5f, 0.0f));
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity.RotatedByRandom(0.1f);
        }
        public override bool Shoot(
              Player player,
              EntitySource_ItemUse_WithAmmo source,
              Vector2 position,
              Vector2 velocity,
              int type,
              int damage,
              float knockback)
        {
            Vector2 vector2_1 = player.RotatedRelativePoint(player.MountedCenter, true, true);
            float num4 = 0.314159274f;
            int num5 = 5;
            Vector2 vector2_2 = velocity;
            vector2_2.Normalize();
            vector2_2 = vector2_2 * 80f;
            bool flag = Collision.CanHit(vector2_1, 0, 0, vector2_1 + vector2_2, 0, 0);
            for (float index3 = 0; index3 < num5; ++index3)
            {
                float num6 = index3 - (num5 - 2f) / 2f;
                Vector2 vector2_3 = Utils.RotatedBy(vector2_2, (double)(num4 * num6), new Vector2());
                if (!flag)
                    vector2_3 = vector2_3 - vector2_2;
                Projectile.NewProjectile(source, vector2_1.X + vector2_3.X, vector2_1.Y + vector2_3.Y, velocity.X * 1.5f, velocity.Y * 1.5f, type, (int)(damage * 0.8f), knockback, player.whoAmI, 0.0f, 0.0f, 0.0f);
                int index4 = Projectile.NewProjectile(source, vector2_1.X + vector2_3.X, vector2_1.Y + vector2_3.Y, velocity.X * 2f, velocity.Y * 2f, 440, (int)(damage * 0.4f), knockback * 0.4f, player.whoAmI, 0.0f, 0.0f, 0.0f);
                Main.projectile[index4].timeLeft = 120;
                Main.projectile[index4].tileCollide = false; 
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AlphaRay>()
                .AddIngredient<AuricBar>(5)
                .AddIngredient<AscendantSpiritEssence>(5)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
