using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using Clamity.Content.Bosses.Clamitas.NPCs;
using Clamity.Content.Bosses.Pyrogen.NPCs;
using Clamity.Content.Bosses.Pyrogen.Projectiles;
using Clamity.Content.Bosses.WoB.NPCs;
using Clamity.Content.Bosses.WoB.Projectiles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Commons
{
    public static class NewNPCStats
    {
        public static void Load()
        {

            EnemyStats.ProjectileDamageValues = new SortedDictionary<Tuple<int, int>, int[]>()
            {
                {
                  new Tuple<int, int>(ModContent.NPCType<PyrogenShield>(), ModContent.ProjectileType<SmallFireball>()),
                  new int[5]{ 60, 100, 120, 132, 180 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<PyrogenShield>(), ModContent.ProjectileType<InfernoFireball>()),
                  new int[5]{ 80, 120, 136, 152, 210 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<PyrogenBoss>(), ModContent.ProjectileType<SmallFireball>()),
                  new int[5]{ 60, 100, 120, 132, 180 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<PyrogenBoss>(), ModContent.ProjectileType<SmallFireballHoming>()),
                  new int[5]{ 60, 100, 120, 132, 180 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<PyrogenBoss>(), ModContent.ProjectileType<InfernoFireball>()),
                  new int[5]{ 80, 120, 136, 152, 210 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<PyrogenBoss>(), ModContent.ProjectileType<FireBomb>()),
                  new int[5]{ 60, 100, 120, 132, 180 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<PyrogenBoss>(), ModContent.ProjectileType<Firethrower>()),
                  new int[5]{ 60, 100, 120, 132, 180 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<ClamitasBoss>(), ModContent.ProjectileType<BrimstoneBarrage>()),
                  new int[5]{ 90, 110, 130, 136, 150 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<ClamitasBoss>(), ModContent.ProjectileType<BrimstonePearlBurst>()),
                  new int[5]{ 90, 110, 130, 136, 150 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<ClamitasBoss>(), ModContent.ProjectileType<BrimstoneHellblast>()),
                  new int[5]{ 90, 110, 130, 136, 150 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<WallOfBronzeClaw>(), ModContent.ProjectileType<WallOfBronzeClawProjectile>()),
                  new int[5]{ 300, 330, 360, 410, 450 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<WallOfBronzeLaser>(), ModContent.ProjectileType<WallOfBronzeLaserBeamStart>()),
                  new int[5]{ 300, 330, 360, 410, 450 }
                },
                {
                  new Tuple<int, int>(ModContent.NPCType<WallOfBronzeTorret>(), ModContent.ProjectileType<WallOfBronzeTorretBlast>()),
                  new int[5]{ 300, 330, 360, 410, 450 }
                }
            };
        }
        public static void UnLoad()
        {
            EnemyStats.ProjectileDamageValues = null;
        }
        public static int GetProjectileDamageClamity(this NPC npc, int projType)
        {
            double num1 = Main.masterMode ? 6.0 : Main.expertMode ? 4.0 : 2.0;
            int[] numArray;
            if (!EnemyStats.ProjectileDamageValues.TryGetValue(new Tuple<int, int>(npc.type, projType), out numArray))
                return 1;
            int num2 = (int)Math.Round(numArray[0] / num1);
            int num3 = (int)Math.Round(numArray[1] / num1);
            int projectileDamage1 = (int)Math.Round(numArray[2] / num1);
            int projectileDamage2 = (int)Math.Round(numArray[3] / num1);
            int projectileDamage3 = (int)Math.Round(numArray[4] / num1);
            if (Main.masterMode)
                return projectileDamage3;
            if (CalamityWorld.death)
                return projectileDamage2;
            if (CalamityWorld.revenge)
                return projectileDamage1;
            return !Main.expertMode ? num2 : num3;
        }
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        internal struct EnemyStats
        {
            public static SortedDictionary<int, double> ExpertDamageMultiplier;
            public static SortedDictionary<int, int[]> ContactDamageValues;
            public static SortedDictionary<Tuple<int, int>, int[]> ProjectileDamageValues;
        }
    }
}
