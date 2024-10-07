using CalamityMod;
using CalamityMod.Events;
using CalamityMod.World;
using Clamity.Commons;
using Clamity.Content.Bosses.Losbaf.Particles;
using Clamity.Content.Bosses.Losbaf.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf.NPCs
{
    public partial class LosbafSuperboss
    {
        public void Spawn(Player player)
        {
            if (AttackTimer == 1)
                TeleportTo(player.Center - Vector2.UnitY * 250);

            if (AttackTimer % 5 == 1)
            {
                ExpandingChromaticBurstParticle burst = new(NPC.Center, Vector2.Zero, Main.DiscoColor, 8, 0.1f);
                burst.Spawn();
            }

            if (AttackTimer >= 60)
            {
#if DEBUG
                NextState(ClamityConfig.Instance.StartingLosbafAttack);
#else
                NextState(LosbafAttack.Slam);
#endif
            }
        }
        public void Slam(Player player)
        {
            bool bossRushActive = BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || bossRushActive;
            bool rev = CalamityWorld.revenge || bossRushActive;
#if DEBUG
            int allSlamCount = 1;
#else
            int allSlamCount = 3;
#endif
            int verticalSlamCount = 2;
            int hoverTime = 33;
            int slamTime = 90;
            int sitInPlaceTime = 18;
            ref float slamCounter = ref NPC.ai[3];
            ref float slamRotation = ref NPC.Calamity().newAI[0];

            if (expert)
            {
                if (slamCounter >= 1f)
                    hoverTime -= 3;
                sitInPlaceTime -= 3;
            }
            if (rev)
            {
                allSlamCount++;
#if DEBUG
                allSlamCount--;
#endif

                if (slamCounter >= 1f)
                    hoverTime -= 3;
                sitInPlaceTime -= 4;
            }

            int slamDelay = hoverTime + sitInPlaceTime;
            float wrappedAttackTimer = AttackTimer % (slamDelay + slamTime);
            float startingSlamSpeed = 20f;
            float maxSlamSpeed = 100f;
            float slamAcceleration = 1.14f;

            if (wrappedAttackTimer == 1)
            {
                slamRotation = Main.rand.Next(1, 4);
                if (slamCounter < verticalSlamCount)
                    slamRotation = 0;
                NPC.rotation = MathHelper.PiOver2 * slamRotation;

                TeleportTo(player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * slamRotation) * 200);
                int j = 0;
                int cloneProj = ModContent.ProjectileType<LosbafCloneSlamAttack>();
                for (int i = 0; i < 4; i++)
                {
                    if (i == slamRotation) continue;
                    int index = ClamityUtils.NewProjectile(NPC.GetSource_FromAI(),
                                                         player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 200,
                                                         Vector2.Zero,
                                                         cloneProj,
                                                         NPC.GetProjectileDamageClamity(cloneProj),
                                                         0,
                                                         Main.myPlayer,
                                                         j,
                                                         slamCounter,
                                                         i,
                                                         NPC.target);
                    j++;
                }
            }

            // Stay above the player before slamming down.
            if (wrappedAttackTimer <= hoverTime && wrappedAttackTimer >= 2f)
            {
                float hoverInterpolant = MathF.Pow(wrappedAttackTimer / hoverTime, 0.74f);
                Vector2 start = player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * slamRotation) * 195f;
                Vector2 end = player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * slamRotation) * 360f;
                NPC.Center = Vector2.Lerp(start, end, hoverInterpolant);
                NPC.velocity = Vector2.Zero;
            }

            // Slam downward and release telegraphed spikes from the sides.
            if (wrappedAttackTimer == slamDelay)
            {
                //SoundEngine.PlaySound(ExplosionTeleportSound, NPC.Center);
                NPC.velocity = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * slamRotation) * startingSlamSpeed;
                NPC.netUpdate = true;
            }

            // Accelerate after slamming.
            if (wrappedAttackTimer >= slamDelay && NPC.velocity.Length() < maxSlamSpeed)
                NPC.velocity *= slamAcceleration;

            if (wrappedAttackTimer == slamDelay + slamTime - 1f)
                slamCounter++;

            if (AttackTimer >= (slamDelay + slamTime) * allSlamCount)
            {
                slamCounter = 0f;
                NextState(LosbafAttack.ExoScytheWithTeleports);
            }
        }
        public void ExoScytheWithTeleports(Player player)
        {
            bool bossRushActive = BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || bossRushActive;
            bool rev = CalamityWorld.revenge || bossRushActive;
            bool death = CalamityWorld.death || bossRushActive;

            float distance = 500;
            ref float teleportationTime = ref NPC.ai[2];
            ref float maxTeleportationTime = ref NPC.ai[3];
            if (AttackTimer == 1)
            {
                NPC.velocity = Vector2.Zero;
                NPC.chaseable = true;
                TeleportTo(player.Center - Vector2.UnitY * distance);
                teleportationTime = 0;
                maxTeleportationTime = 120;
            }
            teleportationTime++;
            //NPC.velocity = Vector2.Zero;

            Vector2 vec1 = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero);
            Vector2 destination = player.Center + vec1 * distance;
            Vector2 distanceFromDestination = destination - NPC.Center;
            CalamityUtils.SmoothMovement(NPC, distance, distanceFromDestination, 10f, 1.1f, false);
            NPC.rotation = vec1.ToRotation() + MathHelper.PiOver2;

            if (AttackTimer % 10 == 0)
            {
                float velocity = 20;
                if (rev)
                    velocity = 30;
                if (death)
                    velocity = 40;
                int exoBeamType = ModContent.ProjectileType<LosbafExoBeam>();
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vec1 * 20, exoBeamType, NPC.GetProjectileDamageClamity(exoBeamType), 0, Main.myPlayer, 0, velocity);
            }
            if (teleportationTime >= maxTeleportationTime)
            {
                TeleportTo(player.Center - vec1 * distance);
                maxTeleportationTime *= 0.75f;
                teleportationTime = 0;
            }
            if (maxTeleportationTime < 10)
            {
                NPC.chaseable = false;
                foreach (Projectile p in Main.projectile)
                {
                    if ((p.type == ModContent.ProjectileType<LosbafExoBeam>() || p.type == ModContent.ProjectileType<ExoScythe>()) && p.ai[0] == 0 && p.timeLeft > 1000 - 180)
                        p.timeLeft = 1000 - 170;
                }
                NextState(LosbafAttack.DownfallExoScythe);
            }

        }
        public static float DistanceOnDownfallScytheAttack = 300;
        public static float DuratationBetweenDownfallScytheAttack = 120;
        public static float DownfallAttackCount = 4;
        public void DownfallExoScythe(Player player)
        {
            bool bossRushActive = BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || bossRushActive;
            bool rev = CalamityWorld.revenge || bossRushActive;
            bool death = CalamityWorld.death || bossRushActive;

            if (AttackTimer == 1)
            {
                TeleportTo(player.Center - Vector2.UnitY * DistanceOnDownfallScytheAttack);
                NPC.chaseable = true;
            }
            //Vector2 vec2 = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero);
            Vector2 destination2 = player.Center - new Vector2(0, DistanceOnDownfallScytheAttack);
            Vector2 distanceFromDestination2 = destination2 - NPC.Center;
            CalamityUtils.SmoothMovement(NPC, DistanceOnDownfallScytheAttack, distanceFromDestination2, 30f, 1.05f, false);
            NPC.rotation = (NPC.Center - player.Center).ToRotation() + MathHelper.PiOver2;
            if (AttackTimer % DuratationBetweenDownfallScytheAttack == 60 && AttackTimer < DuratationBetweenDownfallScytheAttack * DownfallAttackCount - 1)
            {
                int exoBeamType = ModContent.ProjectileType<LosbafExoBeam>();
                float velocity = 20;
                if (rev)
                    velocity = 25;
                if (death)
                    velocity = 30;
                for (int i = -2; i <= 2; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, -Vector2.UnitY.RotatedBy(MathHelper.Pi / 10 * i) * 10, exoBeamType, NPC.GetProjectileDamageClamity(exoBeamType), 0, Main.myPlayer, 4, velocity);
                }
                int distanceDelay = 300;
                if (rev)
                    distanceDelay = 200;
                for (int i = -15; i <= 15; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(distanceDelay * i, 300), Vector2.Zero, exoBeamType, NPC.GetProjectileDamageClamity(exoBeamType), 0, Main.myPlayer, 1, velocity * 3f);
                }
            }
            if (AttackTimer > DuratationBetweenDownfallScytheAttack * DownfallAttackCount + 30)
            {
                NPC.chaseable = false;
                NextState(LosbafAttack.RotatingAroundPlayer);
            }

        }
        public static float DistanceOnRotationAttack = 700;
        public static float DuratationOfRotationAttack = 300;
        public static float RotationPower = 2f;
        public static float VelocityOnRotationAttack = 12;
        public void RotatingAroundPlayer(Player player)
        {
            if (AttackTimer == 1)
            {
                TeleportTo(player.Center - Vector2.UnitY.RotatedBy(Main.GlobalTimeWrappedHourly) * DistanceOnRotationAttack);
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<LosbafCloneRotatable>(), 0, 0, Main.myPlayer, i, NPC.target);
                }
            }
            NPC.Center = player.Center + Vector2.UnitY.RotatedBy(Main.GlobalTimeWrappedHourly * RotationPower) * DistanceOnRotationAttack;
            NPC.rotation = (player.Center - NPC.Center).ToRotation() - MathHelper.PiOver2;
            if (AttackTimer % 15 == 0 && AttackTimer > 30 && AttackTimer < DuratationOfRotationAttack)
            {
                int exoBeamType = ModContent.ProjectileType<LosbafExoBeam>();
                Vector2 vec2 = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vec2 * VelocityOnRotationAttack, exoBeamType, NPC.GetProjectileDamageClamity(exoBeamType), 0, Main.myPlayer, 2, (AttackTimer % 60 == 0).ToInt());
                SoundEngine.PlaySound(SoundID.Item15 with { Pitch = 0.5f }, NPC.Center);
            }
            if (AttackTimer == LosbafSuperboss.DuratationOfRotationAttack + 30)
            {
                NextState(LosbafAttack.Slam);
            }

        }
        public const int FrontwardAttackDelay = 60;
        public const int FrontwardAttackCount = 3;
        public void FrontwardScythes(Player player)
        {
            bool bossRushActive = BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || bossRushActive;
            bool rev = CalamityWorld.revenge || bossRushActive;
            bool death = CalamityWorld.death || bossRushActive;


            Vector2 destination2 = player.Center - new Vector2(0, DistanceOnDownfallScytheAttack);
            Vector2 distanceFromDestination2 = destination2 - NPC.Center;
            CalamityUtils.SmoothMovement(NPC, DistanceOnDownfallScytheAttack, distanceFromDestination2, 30f, 1.05f, false);
            NPC.rotation = (NPC.Center - player.Center).ToRotation() + MathHelper.PiOver2;

        }
    }
}
