using CalamityMod.Projectiles.Melee;

namespace Clamity.Content.Items.Weapons.Melee
{
    public class BaseSlash : ExobeamSlash
    {
        public override string Texture => "Clamity/Content/Items/Weapons/Melee/BaseSlash";
        public virtual float Scale => 1f;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = Projectile.height = 24;
            Projectile.scale = Scale;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
        public override void OnHitPlayer(Player target, Player.HurtInfo info) { }
        public virtual Color FirstColor => Color.White;
        public virtual Color SecondColor => Color.Gray;
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(FirstColor, SecondColor, (float)base.Projectile.identity / 7f % 1f) * base.Projectile.Opacity;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(250, 0).RotatedBy(Projectile.rotation) * Scale, Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center - new Vector2(250, 0).RotatedBy(Projectile.rotation) * Scale, Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint);
        }
    }
}
