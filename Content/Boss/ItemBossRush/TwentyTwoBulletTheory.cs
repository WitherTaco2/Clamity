using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityMod.Events.BossRushEvent;

namespace Clamity.Content.Boss.ItemBossRush
{
    public class TwentyTwoBulletTheory
    {
        internal static IEntitySource Source => new EntitySource_WorldEvent("Clamity_ItemBossRush");

        public static int HostileProjectileKillCounter;
        public static bool BulletTheoryActive = false; // Whether 22 Bullet Theory is active or not.
        public static bool DeactivateStupidFuckingBullshit = false; // Force 22 Bullet Theory to inactive.
        public static int BulletTheoryStage = 0; // Boss Rush Stage.
        public static int BulletTheorySpawnCountdown = 180; // Delay before another 22 Bullet Theory boss can spawn.
        public static List<BossRushEvent.Boss> Bosses = new List<BossRushEvent.Boss>();
        public static Dictionary<int, int[]> BossIDsAfterDeath = new Dictionary<int, int[]>();
        public static Dictionary<int, Action<NPC>> BossDeathEffects = new Dictionary<int, Action<NPC>>();
        public static int StartTimer;
        public static int EndTimer;
        public static float WhiteDimness;
        public static readonly Color UnknownGodTextColor = new(250, 213, 77); // #FAD54D
        public const int StartEffectTotalTime = 120;
        public const int EndVisualEffectTime = 340;
        public static int ClosestPlayerToWorldCenter => Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
        public static int CurrentlyFoughtBoss => Bosses[BulletTheoryStage].EntityID;
        public static int NextBossToFight => Bosses[BulletTheoryStage + 1].EntityID;

        #region Loading and Unloading
        public static void Load()
        {
            Bosses = new List<BossRushEvent.Boss>()
            {

            };
        }

        public static void Unload()
        {
            Bosses = null;
            BossIDsAfterDeath = null;
            BossDeathEffects = null;
        }
        #endregion

        public static int MusicToPlay
        {
            get => Clamity.mod.GetMusicFromMusicMod("BulletTheory") ?? 0;
        }

        #region Updates
        internal static void MiscUpdateEffects()
        {
            if (!BulletTheoryActive)
                return;

            // Handle dialogue as appropriate.
            //BossRushDialogueSystem.Tick();

            // Disable the stupid credits sequence.
            if (CreditsRollEvent.IsEventOngoing)
                CreditsRollEvent.SetRemainingTimeDirect(1);

            // Prevent Moon Lord from spawning naturally
            if (NPC.MoonLordCountdown > 0)
                NPC.MoonLordCountdown = 0;

            if (BossRushEvent.BossRushActive)
                BulletTheoryActive = false;

            // Handle projectile clearing.
            if (HostileProjectileKillCounter > 0)
            {
                HostileProjectileKillCounter--;
                if (HostileProjectileKillCounter == 1)
                    CalamityUtils.KillAllHostileProjectiles();

                if (Main.netMode == NetmodeID.Server)
                {
                    var netMessage = ModContent.GetInstance<CalamityMod.CalamityMod>().GetPacket();
                    netMessage.Write((byte)CalamityModMessageType.BRHostileProjKillSync);
                    netMessage.Write(HostileProjectileKillCounter);
                    netMessage.Send();
                }
            }
        }
        internal static void Update()
        {
            if (!BulletTheoryActive)
            {
                BulletTheorySpawnCountdown = 180;
                TwentyTwoBulletTheorySky.CurrentInterestMin = 0f;
                if (BulletTheoryStage != 0)
                {
                    BulletTheoryStage = 0;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var netMessage = Clamity.mod.GetPacket();
                        netMessage.Write((byte)ClamityMessageType.BulletTheoryStage);
                        netMessage.Write(BulletTheoryStage);
                        netMessage.Send();
                    }
                }
                return;
            }

            // Projectile deletion, preventing Credits and ML from spawning naturally, and dialogue.
            MiscUpdateEffects();

            // Do boss rush countdown and shit if no boss is alive.
            if (!CalamityPlayer.areThereAnyDamnBosses)
            {
                if (BulletTheorySpawnCountdown > 0)
                    BulletTheorySpawnCountdown--;

                // Cooldown and boss spawn.
                if (BulletTheorySpawnCountdown <= 0 && BulletTheoryStage < Bosses.Count)
                {
                    // Cooldown before next boss spawns.
                    BulletTheorySpawnCountdown = 60;

                    // Increase cooldown post-Moon Lord.
                    if (BulletTheoryStage >= Bosses.FindIndex(boss => boss.EntityID == NPCID.MoonLordCore))
                        BulletTheorySpawnCountdown += 300;

                    // Override the spawn countdown if specified.
                    if (BulletTheoryStage < Bosses.Count - 1 && Bosses[BulletTheoryStage + 1].SpecialSpawnCountdown != -1)
                        BulletTheorySpawnCountdown = Bosses[BulletTheoryStage + 1].SpecialSpawnCountdown;

                    // Change time as necessary.
                    if (Bosses[BulletTheoryStage].ToChangeTimeTo != TimeChangeContext.None)
                        CalamityUtils.ChangeTime(Bosses[BulletTheoryStage].ToChangeTimeTo == TimeChangeContext.Day);

                    // Play a special boss roar sound by default.
                    if (!Bosses[BulletTheoryStage].UsesSpecialSound)
                        SoundEngine.PlaySound(BossSummonSound, Main.player[ClosestPlayerToWorldCenter].Center);

                    // And spawn the boss.
                    Bosses[BulletTheoryStage].SpawnContext.Invoke(CurrentlyFoughtBoss);
                }
            }

            // Change dimness.
            if (BulletTheoryStage >= 0 && BulletTheoryStage < Bosses.Count)
            {
                WhiteDimness = MathHelper.Lerp(WhiteDimness, Bosses[BulletTheoryStage].DimnessFactor, 0.1f);
                if (MathHelper.Distance(WhiteDimness, Bosses[BulletTheoryStage].DimnessFactor) < 0.004f)
                    WhiteDimness = Bosses[BulletTheoryStage].DimnessFactor;
            }

            if (EndTimer > 0)
                TwentyTwoBulletTheorySky.CurrentInterest = MathHelper.Lerp(0.5f, 0.75f, Utils.GetLerpValue(5f, 145f, EndTimer, true));
            TwentyTwoBulletTheorySky.CurrentInterestMin = MathHelper.Lerp(0f, 0.5f, (float)Math.Pow(BulletTheoryStage / (float)Bosses.Count, 5D));
        }

        public static void End()
        {
            // Reset BossRushReturnPosition
            for (int playerIndex = 0; playerIndex < Main.maxPlayers; playerIndex++)
            {
                Player p = Main.player[playerIndex];
                if (p is not null && p.active)
                {
                    p.Calamity().BossRushReturnPosition = null;
                }
            }

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                EndEffects();
            }
            else
            {
                var netMessage = Clamity.mod.GetPacket();
                netMessage.Write((byte)ClamityMessageType.EndBulletTheory);
                netMessage.Send();
            }
        }

        internal static void EndEffects()
        {
            for (int doom = 0; doom < Main.maxNPCs; doom++)
            {
                NPC n = Main.npc[doom];
                if (!n.active)
                    continue;

                // will also correctly despawn EoW because none of his segments are boss flagged. Draedon isn't a boss either
                bool shouldDespawn = n.boss || n.type == NPCID.EaterofWorldsHead || n.type == NPCID.EaterofWorldsBody || n.type == NPCID.EaterofWorldsTail || n.type == ModContent.NPCType<Draedon>();
                if (shouldDespawn)
                {
                    n.active = false;
                    n.netUpdate = true;
                }
            }

            BulletTheoryActive = false;
            BulletTheoryStage = 0;
            StartTimer = 0;
            EndTimer = 0;
            CalamityUtils.KillAllHostileProjectiles();

            CalamityNetcode.SyncWorld();
            if (Main.netMode == NetmodeID.Server)
            {
                var netMessage = Clamity.mod.GetPacket();
                netMessage.Write((byte)ClamityMessageType.BulletTheoryStage);
                netMessage.Write(BulletTheoryStage);
                netMessage.Send();
                var netMessage2 = Clamity.mod.GetPacket();
                netMessage2.Write((byte)ClamityMessageType.BulletTheoryStartTimer);
                netMessage2.Write(StartTimer);
                netMessage2.Send();
                var netMessage3 = Clamity.mod.GetPacket();
                netMessage3.Write((byte)ClamityMessageType.BulletTheoryEndTimer);
                netMessage3.Write(EndTimer);
                netMessage3.Send();
            }
        }
        #endregion

        #region On Boss Kill
        internal static void OnBossKill(NPC npc, Mod mod)
        {
            // This is the generic form of "Are there any remaining NPCs on the boss list for this boss rush stage?" check.
            if ((Bosses.Any(boss => boss.EntityID == npc.type) && !BossIDsAfterDeath.ContainsKey(npc.type)) ||
                     BossIDsAfterDeath.Values.Any(killList => killList.Contains(npc.type)))
            {
                BulletTheoryStage++;
                CalamityUtils.KillAllHostileProjectiles();
                HostileProjectileKillCounter = 3;

                if (BossDeathEffects.ContainsKey(npc.type))
                {
                    BossDeathEffects[npc.type].Invoke(npc);
                }

                if (npc.type == Bosses[Bosses.Count - 1].EntityID)
                {
                    // Mark Boss Rush as complete
                    DownedBossSystem.downedBossRush = true;
                    CalamityNetcode.SyncWorld();

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(Source, npc.Center, Vector2.Zero, ModContent.ProjectileType<BossRushEndEffectThing>(), 0, 0f, Main.myPlayer);
                }
            }

            // Sync the stage and progress of Boss Rush whenever a relevant boss dies.
            if (Main.netMode == NetmodeID.Server)
            {
                var netMessage = mod.GetPacket();
                netMessage.Write((byte)ClamityMessageType.BulletTheoryStage);
                netMessage.Write(BulletTheoryStage);
                netMessage.Send();
                var netMessage2 = mod.GetPacket();
                netMessage2.Write((byte)CalamityModMessageType.BRHostileProjKillSync);
                netMessage2.Write(HostileProjectileKillCounter);
                netMessage2.Send();
            }

            TwentyTwoBulletTheorySky.CurrentInterest = 0.85f;
        }

        public static void CreateTierAnimation(int tier)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (!Main.player[i].active || Main.player[i].dead)
                        continue;

                    int animation = Projectile.NewProjectile(new EntitySource_WorldEvent(), Main.player[i].Center, Vector2.Zero, ModContent.ProjectileType<BossRushTierAnimation>(), 0, 0f, i);
                    if (Main.projectile.IndexInRange(animation))
                        Main.projectile[animation].ai[0] = tier;
                }
            }
        }
        #endregion

        #region Netcode
        public static void SyncStartTimer(int time)
        {
            StartTimer = time;
            if (Main.netMode != NetmodeID.Server)
                return;

            var netMessage = ModContent.GetInstance<CalamityMod.CalamityMod>().GetPacket();
            netMessage.Write((byte)ClamityMessageType.BulletTheoryStartTimer);
            netMessage.Write(StartTimer);
            netMessage.Send();
        }

        public static void SyncEndTimer(int time)
        {
            EndTimer = time;
            if (Main.netMode != NetmodeID.Server)
                return;

            var netMessage = Clamity.mod.GetPacket();
            netMessage.Write((byte)ClamityMessageType.EndBulletTheory);
            netMessage.Write(EndTimer);
            netMessage.Send();
        }
        #endregion
    }
}
