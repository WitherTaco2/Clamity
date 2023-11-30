using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Pyrogen.Projectiles
{
    public class Firethrower : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = 193;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 2;
        }
        public bool Spawned
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int num = (int)Utils.Remap(Projectile.localAI[0], 0.0f, 72f, 10f, 40f);
            hitbox.Inflate(num, num);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return projHitbox.Intersects(targetHitbox) && Collision.CanHit(Projectile.Center, 0, 0, targetHitbox.Center.ToVector2(), 0, 0);
        }
        public override void AI()
        {
            //Projectile.velocity *= 0.95f;
            //Projectile.position = Projectile.position - Projectile.velocity;
        }
    }
}
