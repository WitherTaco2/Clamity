using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Clamity
{
    public static class ClamityUtils
    {
        public static ClamityPlayer Clamity(this Player player) => player.GetModPlayer<ClamityPlayer>();
        public static ClamityGlobalProjectile Clamity(this Projectile proj) => proj.GetGlobalProjectile<ClamityGlobalProjectile>();

    }
}
