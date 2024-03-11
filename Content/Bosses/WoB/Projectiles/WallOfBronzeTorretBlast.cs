namespace Clamity.Content.Bosses.WoB.Projectiles
{
    public class WallOfBronzeTorretBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.Calamity().DealsDefenseDamage = true;
        }
        public override void AI()
        {
            ++Projectile.frameCounter;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                ++Projectile.frame;
            }
            if (Projectile.frame >= 3)
            {
                Projectile.frame = 0;
            }
            Projectile.velocity *= 1.01f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, texture: ModContent.Request<Texture2D>(Texture).Value);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.getGoodWorld)
                target.AddBuff(BuffID.Frozen, 180);
        }
    }
}
