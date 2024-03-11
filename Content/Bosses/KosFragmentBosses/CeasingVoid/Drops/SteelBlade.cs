using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.KosFragmentBosses.CeasingVoid.Drops
{
    public class SteelBlade : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = Item.height = 72;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;

            Item.useAnimation = Item.useTime = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 7f;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Item.GetSource_FromThis(), target.Center, Vector2.UnitX.RotatedByRandom(1f) * 0.1f, ModContent.ProjectileType<SteelBladeSlash>(), (int)(damageDone * 0.5f), Item.knockBack, player.whoAmI);
        }
    }
    public class SteelBladeSlash : ExobeamSlash
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.scale = 0.5f;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.White, Color.LightGray, Projectile.identity / 7f % 1f) * Projectile.Opacity;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(250, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center - new Vector2(250, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint);
        }
    }
}
