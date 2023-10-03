using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;

namespace Clamity
{
    public static class ClamityUtils
    {
        public static ClamityPlayer Clamity(this Player player) => player.GetModPlayer<ClamityPlayer>();
        public static ClamityGlobalProjectile Clamity(this Projectile proj) => proj.GetGlobalProjectile<ClamityGlobalProjectile>();
        public static LocalizedText GetText(string key) => Language.GetOrRegister("Mods.Clamity." + key, (Func<string>)null);
        public static bool ContainType(int type, params int[] array)
        {
            bool num = false;
            foreach (int i in array)
            {
                if (i == type)
                {
                    num = true;
                    break;
                }
            }
            return num;
        }
    }
}
