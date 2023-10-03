using CalamityMod.Items;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Rogue;
using CalamityMod;
using Clamity.Content.Cooldowns;
using Microsoft.Xna.Framework;

namespace Clamity.Content.Items.Weapons.Melee.Shortswords
{
    public class SporeKnife : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;

            Item.useAnimation = Item.useTime = 12;
            Item.useStyle = 13;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 33;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5.75f;

            Item.shoot = ModContent.ProjectileType<SporeKnifeProjectile>();
            Item.shootSpeed = 2.4f;
        }
    }
    public class SporeKnifeProjectile : BaseShortswordProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<SporeKnife>().Texture;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
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
            if (!Main.player[Projectile.owner].HasCooldown(ShortstrikeCooldown.ID))
            {
                Main.player[Projectile.owner].AddCooldown(ShortstrikeCooldown.ID, 20);
                int index = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity / 2, Main.rand.Next(569, 572), (int)(Projectile.damage / 2), Projectile.knockBack, Projectile.owner);
                Main.projectile[index].DamageType = DamageClass.Melee;
            }
        }
        public override void ExtraBehavior()
        {
            if (!Utils.NextBool(Main.rand, 5))
                return;
            Dust.NewDust(new Vector2((float)Projectile.Hitbox.X, (float)Projectile.Hitbox.Y), Projectile.Hitbox.Width, Projectile.Hitbox.Height, 2, 0.0f, 0.0f, 0, new Color(), 1f);
        }
    }
}
