using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Sounds;
using CalamityMod.World;
using Clamity.Commons;
using Clamity.Content.Bosses.Losbaf.Drop;
using Clamity.Content.Bosses.Losbaf.Particles;
using Clamity.Content.Bosses.Losbaf.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf.NPCs
{
    public enum LosbafAttack : int
    {
        Spawn = 0,
        Slam = 1,
        ExoScytheWithTeleports = 2,
        Rotating = 3,
        CircleExoScythe = 4,
    }
    public enum LosbafCloneColorType : int
    {
        Cyan = 0,
        Yellow = 1,
        Magenta = 2,
    }
    [AutoloadBossHead]
    public class LosbafSuperboss : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = NPC.height = 304;
            NPC.npcSlots = 50f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(1);
            NPC.netAlways = true;
            //NPC.hide = true;
            NPC.canGhostHeal = false;

            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound; //new SoundStyle("CalamityMod/Sounds/Item/ClamImpact")
            if (Main.getGoodWorld)
                NPC.DeathSound = new SoundStyle("Clamity/Sounds/Custom/GFBPipe");

            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.lavaImmune = true;

            NPC.LifeMaxNERB(1980000, 3300000, 3300000);
            NPC.DR_NERD(0.3f);
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.damage = 400;

            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToWater = true;

            if (!Main.dedServ)
            {
                Music = Clamity.mod.GetMusicFromMusicMod("Losbaf") ?? MusicID.Boss2;
            }
        }
        public LosbafAttack CurrectAttack
        {
            get => (LosbafAttack)NPC.ai[0];
            set => NPC.ai[0] = (int)value;
        }
        public int AttackTimer
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = (int)value;
        }
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 3200f)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];
            bool bossRushActive = BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || bossRushActive;
            bool rev = CalamityWorld.revenge || bossRushActive;
            bool death = CalamityWorld.death || bossRushActive;

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(faceTarget: false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = 0f;
                    }

                    NPC.velocity.Y -= 0.5f;
                    if (NPC.velocity.Y < -30f)
                    {
                        NPC.velocity.Y = -30f;
                    }

                    if (NPC.timeLeft > 60)
                    {
                        NPC.timeLeft = 60;
                    }

                    return;
                }
            }
            else if (NPC.timeLeft < 1800)
            {
                NPC.timeLeft = 1800;
            }

            int exoScytheType = ModContent.ProjectileType<ExoScythe>();

            #region Attack
            AttackTimer++;
            switch (CurrectAttack)
            {
                case LosbafAttack.Spawn:
                    if (AttackTimer == 1)
                        TeleportTo(player.Center - Vector2.UnitY * 200);

                    if (AttackTimer % 5 == 1)
                    {
                        ExpandingChromaticBurstParticle burst = new(NPC.Center, Vector2.Zero, Main.DiscoColor, 8, 0.1f);
                        burst.Spawn();
                    }

                    if (AttackTimer >= 60)
                    {
                        NextState(LosbafAttack.Slam);
                    }
                    break;
                case LosbafAttack.Slam:
                    int allSlamCount = 4;
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
                        slamRotation = Main.rand.Next(1, 8);
                        if (slamCounter < verticalSlamCount)
                            slamRotation = 0;
                        NPC.rotation = MathHelper.PiOver4 * slamRotation;

                        TeleportTo(player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * slamRotation) * 200);
                    }

                    // Stay above the player before slamming down.
                    if (wrappedAttackTimer <= hoverTime && wrappedAttackTimer >= 2f)
                    {
                        float hoverInterpolant = MathF.Pow(wrappedAttackTimer / hoverTime, 0.74f);
                        Vector2 start = player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * slamRotation) * 195f;
                        Vector2 end = player.Center - Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * slamRotation) * 360f;
                        NPC.Center = Vector2.Lerp(start, end, hoverInterpolant);
                        NPC.velocity = Vector2.Zero;
                    }

                    // Slam downward and release telegraphed spikes from the sides.
                    if (wrappedAttackTimer == slamDelay)
                    {
                        //SoundEngine.PlaySound(ExplosionTeleportSound, NPC.Center);
                        NPC.velocity = Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * slamRotation) * startingSlamSpeed;
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

                    break;
                case LosbafAttack.ExoScytheWithTeleports:
                    float distance = 300;
                    ref float teleportationTime = ref NPC.ai[2];
                    ref float maxTeleportationTime = ref NPC.ai[3];
                    if (AttackTimer == 1)
                    {
                        NPC.velocity = Vector2.Zero;
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
                    NPC.rotation = vec1.ToRotation() - MathHelper.PiOver2;

                    if (AttackTimer % 10 == 0)
                    {
                        float velocity = 20;
                        if (rev)
                            velocity = 30;
                        if (death)
                            velocity = 40;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vec1 * 20, ModContent.ProjectileType<ExoScythe>(), NPC.GetProjectileDamageClamity(exoScytheType), 0, Main.myPlayer, 0, velocity);
                    }
                    if (teleportationTime >= maxTeleportationTime)
                    {
                        TeleportTo(player.Center - vec1 * distance);
                        maxTeleportationTime *= 0.75f;
                        teleportationTime = 0;
                    }
                    if (maxTeleportationTime < 10)
                    {
                        NextState(LosbafAttack.Slam);
                        //teleportationTime = 0;
                        //maxTeleportationTime = 0;
                    }
                    break;
            }
            #endregion
        }
        private void NextState(LosbafAttack nextAttack)
        {
            CurrectAttack = nextAttack;
            AttackTimer = 0;
            NPC.ai[2] = 0;
            NPC.ai[3] = 0;
        }
        public void TeleportTo(Vector2 teleportPosition)
        {
            NPC.Center = teleportPosition;
            NPC.velocity = Vector2.Zero;
            NPC.netUpdate = true;

            // Reset the oldPos array, so that afterimages don't suddenly "jump" due to the positional change.
            for (int i = 0; i < NPC.oldPos.Length; i++)
                NPC.oldPos[i] = NPC.position;

            //SoundEngine.PlaySound(TeleportOutSound, NPC.Center);

            // Create teleport particle effects.
            ExpandingGreyscaleCircleParticle circle = new(NPC.Center, Vector2.Zero, new(219, 194, 229), 10, 0.28f);
            VerticalLightStreakParticle bigLightStreak = new(NPC.Center, Vector2.Zero, new(228, 215, 239), 10, new(2.4f, 3f));
            MagicBurstParticle magicBurst = new(NPC.Center, Vector2.Zero, new(150, 109, 219), 12, 0.1f);
            for (int i = 0; i < 30; i++)
            {
                Vector2 smallLightStreakSpawnPosition = NPC.Center + Main.rand.NextVector2Square(-NPC.width, NPC.width) * new Vector2(0.4f, 0.2f);
                Vector2 smallLightStreakVelocity = Vector2.UnitY * Main.rand.NextFloat(-3f, 3f);
                VerticalLightStreakParticle smallLightStreak = new(smallLightStreakSpawnPosition, smallLightStreakVelocity, Color.White, 10, new(0.1f, 0.3f));
                smallLightStreak.Spawn();
            }

            circle.Spawn();
            bigLightStreak.Spawn();
            magicBurst.Spawn();
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 107, 0f, 0f, 100, new Color(0, 255, 255), 1f);

            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 3;
                SoundEngine.PlaySound(CommonCalamitySounds.ExoHitSound, NPC.Center);
            }

            /*if (NPC.life <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 107, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
                }
                for (int j = 0; j < 20; j++)
                {
                    int plasmaDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 107, 0f, 0f, 0, new Color(0, 255, 255), 2.5f);
                    Main.dust[plasmaDust].noGravity = true;
                    Main.dust[plasmaDust].velocity *= 3f;
                    plasmaDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 107, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
                    Main.dust[plasmaDust].velocity *= 2f;
                    Main.dust[plasmaDust].noGravity = true;
                }

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody5").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody6").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody7").Type, 1f);
                }
            }*/
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LosbafBag>()));
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();


            mainRule.Add(ItemDropRule.Common(ModContent.ItemType<ThankYouPainting>(), 100));
            //Trophy
            //npcLoot.Add(ModContent.ItemType<>(), 10);
            //Relic
            //npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<>());
            //Mask
            //mainRule.Add(ItemDropRule.Common(ModContent.ItemType<>(), 7));
            //Lore
            //npcLoot.AddConditionalPerPlayer(() => !ClamitySystem.downedLosbaf, ModContent.ItemType<>(), ui: true, DropHelper.FirstKillText);

            //npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.CopperPlating, 1, 1, 9999, hideLootReport: true);
        }
        public override void OnKill()
        {
            ClamitySystem.downedLosbaf = true;
            CalamityNetcode.SyncWorld();
        }
        public static Color GetLosbafCloneColor(int type, float alpha = 128)
        {
            switch (type)
            {
                case 0:
                    return new Color(0, 255, 255, alpha); //Cyan
                case 1:
                    return new Color(255, 255, 0, alpha); //Yellow
                case 2:
                    return new Color(255, 0, 255, alpha); //Magenta
                default:
                    return Color.White;
            }
        }
        public static Color GetLosbafCloneColor(LosbafCloneColorType type, float alpha = 128)
        {
            return GetLosbafCloneColor((int)type, alpha);
        }
    }
}
