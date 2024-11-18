using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.World;
using Clamity.Commons;
using Clamity.Content.Bosses.Cybermind.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Cybermind.NPCs
{
    public enum CyberhiveAttacks : int
    {
        //Minor
        Spawn = 0,
        DashAfterRotation = 1,
        Backdash = 2,

        //Phase 1
        Rotating = 11,
        NormalDash = 12,
        GasDash = 13,

        //Phase 2
        Transform = 100,
        HyperVerticalDash = 101,
        GammaDeathRay = 102,
        GooDash = 103,
        ShootVerticalRay = 104,
    }
    public class Cyberhive : ModNPC
    {
        public static int normalIconIndex;
        public static int phase2IconIndex;
        private int biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;
        private float rotationOfAttack = 0;
        private int attackCounter = 0;
        private const int preDashMaxTimer = 120;
        private const int dashMaxTimer = 60;
        private bool IsPhaseTwo => (NPC.life / (float)NPC.lifeMax) < 0.5f;
        public int Attack
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public CyberhiveAttacks GetAttack => (CyberhiveAttacks)NPC.ai[0];
        public int AttackTimer
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        internal static void LoadHeadIcons()
        {
            string normalIconPath = "Clamity/Content/Bosses/Cybermind/NPCs/Cyberhive_Head_Boss";
            string phase2IconPath = "Clamity/Content/Bosses/Cybermind/NPCs/Cyberhive_Head_Boss2";

            Clamity.mod.AddBossHeadTexture(normalIconPath, -1);
            normalIconIndex = ModContent.GetModBossHeadSlot(normalIconPath);

            Clamity.mod.AddBossHeadTexture(phase2IconPath, -1);
            phase2IconIndex = ModContent.GetModBossHeadSlot(phase2IconPath);
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = NPC.oldPos.Length;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.4f,
                PortraitPositionYOverride = 3f
            };
            value.Position.Y += 3f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 5f;
            NPC.GetNPCDamage();

            NPC.width = 200;
            NPC.height = 150;

            NPC.defense = 8;
            NPC.LifeMaxNERB(100000, 192000, 350000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 60, 0, 0);
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson,
                new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.Cyberhive.Bestiary")
            });
        }

        public override void BossHeadSlot(ref int index)
        {
            index = IsPhaseTwo ? phase2IconIndex : normalIconIndex;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(biomeEnrageTimer);
            writer.Write(rotationOfAttack);
            writer.Write(attackCounter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            biomeEnrageTimer = reader.ReadInt32();
            rotationOfAttack = reader.ReadSingle();
            attackCounter = reader.ReadInt32();
        }

        public override void AI()
        {
            #region Pre-Attacks
            // Get a target
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            bool bossRush = BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || bossRush;
            bool masterMode = Main.masterMode || bossRush;
            bool revenge = CalamityWorld.revenge || bossRush;
            bool death = CalamityWorld.death || bossRush;

            // Percent life remaining
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // Enrage
            /*if ((!player.ZoneCorrupt || !player.ZoneCrimson || (NPC.position.Y / 16f) < Main.worldSurface) && !bossRush)
            {
                if (biomeEnrageTimer > 0)
                    biomeEnrageTimer--;
            }
            else
                biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;

            bool biomeEnraged = biomeEnrageTimer <= 0 || bossRush;

            float enrageScale = bossRush ? 1f : 0f;
            if (biomeEnraged && (!player.ZoneCorrupt || !player.ZoneCrimson || bossRush))
            {
                NPC.Calamity().CurrentlyEnraged = !bossRush;
                enrageScale += 1f;
            }
            if (biomeEnraged && ((NPC.position.Y / 16f) < Main.worldSurface || bossRush))
            {
                NPC.Calamity().CurrentlyEnraged = !bossRush;
                enrageScale += 1f;
            }*/
            float enrageScale = bossRush ? 1f : 0f;
            #endregion

            #region radiation
            foreach (var p in Main.player)
            {
                if (p == null || !p.active) continue;
                if ((p.Center - NPC.Center).Length() < MathF.Max(NPC.width, NPC.height) + 100)
                    p.Calamity().SulphWaterPoisoningLevel += 0.01f;
            }
            #endregion

            #region Main AI
            if (IsPhaseTwo && Attack < 100)
            {
                SetNextAttack(100);
            }
            AttackTimer++;
            switch (GetAttack)
            {
                case CyberhiveAttacks.Spawn:
                    //SetNextAttack((int)CyberhiveAttacks.Backdash);
                    SetNextAttack((int)CyberhiveAttacks.Rotating);
                    break;
                #region 1st stage
                case CyberhiveAttacks.Rotating:
                    //rotationOfAttack += 
                    float num1 = 1f - AttackTimer / 30f * (attackCounter % 2 == 0 ? -1 : 1);
                    rotationOfAttack += num1;
                    NPC.Center = player.Center + Vector2.UnitX.RotatedBy(rotationOfAttack) * 500;
                    NPC.Opacity += 1f / 15f;
                    if (num1 <= 0)
                        SetNextAttack(CyberhiveAttacks.DashAfterRotation);
                    break;
                case CyberhiveAttacks.DashAfterRotation:
                    if (AttackTimer == 1)
                    {
                        NPC.velocity = (player.Center - NPC.Center).SafeNormalize() * 10;
                    }
                    NPC.Opacity -= 1f / 15f;
                    NPC.velocity *= 0.95f;
                    if (AttackTimer >= 30)
                    {
                        if (attackCounter < 3)
                        {
                            attackCounter++;
                            SetNextAttack(CyberhiveAttacks.Rotating);
                        }
                        else
                        {
                            attackCounter = 0;
                            SetNextAttack(CyberhiveAttacks.Backdash);
                        }
                    }
                    break;
                case CyberhiveAttacks.NormalDash:
                    if (AttackTimer == 1)
                    {
                        //NPC.velocity = (player.Center - NPC.Center).SafeNormalize() * 10;
                        NPC.velocity = -NPC.velocity;
                    }
                    NPC.velocity *= 0.98f;
                    if (AttackTimer >= 30)
                    {
                        if (attackCounter < 4)
                        {
                            attackCounter++;
                            SetNextAttack(CyberhiveAttacks.Backdash);
                        }
                        else
                        {
                            attackCounter = 0;
                            SetNextAttack(CyberhiveAttacks.GasDash);
                        }
                    }
                    break;
                case CyberhiveAttacks.Backdash:
                    if (AttackTimer == 1)
                    {
                        NPC.velocity = (NPC.Center - player.Center).SafeNormalize() * 15;
                    }
                    if (AttackTimer >= 5)
                    {
                        SetNextAttack(CyberhiveAttacks.NormalDash);
                    }

                    break;
                case CyberhiveAttacks.GasDash:
                    int sum = preDashMaxTimer + dashMaxTimer;
                    if (AttackTimer % sum == 1)
                    {
                        NPC.Center = player.Center + Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 2000;
                        NPC.velocity = Vector2.Zero;
                    }
                    if (AttackTimer % sum == preDashMaxTimer + 1)
                    {
                        NPC.velocity = (player.Center - NPC.Center).SafeNormalize() * 30;
                    }
                    /*if (AttackTimer % sum > preDashMaxTimer + 1)
                    {
                        NPC.velocity *= 0.99f;
                    }*/
                    if (AttackTimer > sum * 3)
                    {
                        SetNextAttack();
                    }
                    break;
                #endregion

                #region 2nd stage
                case CyberhiveAttacks.Transform:

                    break;
                case CyberhiveAttacks.HyperVerticalDash:

                    break;
                case CyberhiveAttacks.GammaDeathRay:

                    break;
                case CyberhiveAttacks.GooDash:
                    if (AttackTimer == 1)
                    {
                        NPC.Center = player.Center + new Vector2(1000, -1000);
                        NPC.velocity = new Vector2(-30, 0);
                        NPC.ai[2] = Main.rand.NextBool() ? -1 : 1;
                        NPC.velocity *= NPC.ai[2];
                    }

                    NPC.velocity += new Vector2(3 * NPC.ai[2], 0);
                    NPC.velocity = new Vector2(MathHelper.Clamp(NPC.velocity.X, -30, 30), 0);
                    if (AttackTimer % 10 == 9)
                    {
                        int proj1 = ModContent.ProjectileType<RadiactiveBlob>();
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, -Vector2.UnitY.RotatedByRandom(0.1f), proj1, NPC.GetProjectileDamageClamity(proj1), 0, Main.myPlayer);
                    }
                    if (AttackTimer > 100)
                    {
                        SetNextAttack();
                    }
                    break;
                case CyberhiveAttacks.ShootVerticalRay:

                    break;
                    #endregion
            }
            #endregion
        }
        #region Sets of attacks
        public void SetNextAttack()
        {
            Attack++;
            if (IsPhaseTwo)
            {
                if (Attack > 104)
                    Attack = 101;
            }
            else
            {
                if (Attack > 13)
                    Attack = 11;

            }
            AttackTimer = 0;
        }
        public void SetNextAttack(int nextAttack)
        {
            Attack = nextAttack;
            AttackTimer = 0;
        }
        public void SetNextAttack(CyberhiveAttacks nextAttack)
        {
            SetNextAttack((int)nextAttack);
        }
        #endregion

        #region Drawing
        public override void FindFrame(int frameHeight)
        {
            NPC.frame = new Rectangle(0, IsPhaseTwo ? 136 : 0, 198, 136);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int particleTimer = preDashMaxTimer;
            if (GetAttack == CyberhiveAttacks.GasDash && AttackTimer < particleTimer)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (NPC.spriteDirection == 1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Texture2D t = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomLine").Value;
                Color color3 = Color.Lerp(Color.Green, Color.Lime, AttackTimer / particleTimer);
                spriteBatch.Draw(t,
                                 NPC.Center /*- base.NPC.rotation.ToRotationVector2() * base.NPC.spriteDirection * 104f*/ - screenPos,
                                 null,
                                 color3,
                                 base.NPC.rotation + MathF.PI / 2f,
                                 new Vector2((float)t.Width / 2f, t.Height),
                                 new Vector2(1f * AttackTimer / particleTimer, 4200f),
                                 effects,
                                 0f);
            }
        }
        #endregion
    }
}
