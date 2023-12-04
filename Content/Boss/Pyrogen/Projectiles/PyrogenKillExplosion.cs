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
    public class PyrogenKillExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 98;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.Center = Main.npc[(int)Projectile.ai[0]].position + new Vector2(Projectile.ai[1], Projectile.ai[2]);
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 5 == 0)
            {
                Projectile.frame++;
                if(Projectile.frame > 6)
                    Projectile.Kill();
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
    }
}
