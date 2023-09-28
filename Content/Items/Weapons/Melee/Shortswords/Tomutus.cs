using CalamityMod.Items.Materials;
using CalamityMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod;
using Terraria;
using Microsoft.Xna.Framework;
using Clamity.Content.Cooldowns;

namespace Clamity.Content.Items.Weapons.Melee.Shortswords
{
    public class Tomutus : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 36; Item.height = 38;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;

            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = 13;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 35;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5.5f;

            Item.shoot = ModContent.ProjectileType<TomutusProjectile>();
            Item.shootSpeed = 2.4f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class TomutusProjectile : BaseShortswordProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<Tomutus>().Texture;
        public override void SetDefaults()
        {
            Projectile.width = 36; Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 60);
            if (!Main.player[Projectile.owner].HasCooldown(ShortstrikeCooldown.ID))
            {
                Main.player[Projectile.owner].AddCooldown(ShortstrikeCooldown.ID, 30);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ProjectileID.Volcano, hit.Damage, hit.Knockback, Projectile.owner);
            }
        }
    }
}
