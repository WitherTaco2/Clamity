using CalamityMod.World;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Clamity
{
    public static class ClamityUtils
    {
        /*[StructLayout(LayoutKind.Sequential, Size = 1)]
        internal struct EnemyStats
        {
            public static SortedDictionary<int, double> ExpertDamageMultiplier;

            public static SortedDictionary<int, int[]> ContactDamageValues;

            public static SortedDictionary<Tuple<int, int>, int[]> ProjectileDamageValues;

            public static SortedDictionary<int, Tuple<bool, int[]>> DebuffImmunities;
        }*/
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
        /*public static double GetExpertDamageMultiplierClamity(this NPC npc, bool? master = null)
        {
            if (!EnemyStats.ExpertDamageMultiplier.TryGetValue(npc.type, out var value))
            {
                return 1.0;
            }

            return value;
        }
        public static void GetNPCDamageClamity(this NPC npc)
        {
            double num = npc.GetExpertDamageMultiplierClamity() * (Main.masterMode ? 3.0 : 2.0);
            if (!EnemyStats.ContactDamageValues.TryGetValue(npc.type, out var value))
            {
                npc.damage = 1;
            }

            int num2 = value[0];
            int num3 = ((value[1] == -1) ? (-1) : ((int)Math.Round((double)value[1] / num)));
            int num4 = ((value[2] == -1) ? (-1) : ((int)Math.Round((double)value[2] / num)));
            int num5 = ((value[3] == -1) ? (-1) : ((int)Math.Round((double)value[3] / num)));
            int num6 = ((value[4] == -1) ? (-1) : ((int)Math.Round((double)value[4] / num)));
            int num7 = (Main.masterMode ? num6 : (CalamityWorld.death ? num5 : (CalamityWorld.revenge ? num4 : (Main.expertMode ? num3 : num2))));
            if (num7 != -1)
            {
                npc.damage = num7;
            }
        }*/
    }
}
