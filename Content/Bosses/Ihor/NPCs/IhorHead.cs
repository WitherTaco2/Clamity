using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.World;
using Clamity.Commons;
using Clamity.Content.Particles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Ihor.NPCs
{
    /*public enum IhorMagicAttacks : int
    {
        Summon = 0,
        MagicBurst,
        HomingStowballs,

        StormPillars,
    }
    public enum IhorMeleeAttacks : int
    {
        Summon = 0,
        LinearDash,
        HomingDash,
        DoGLikeDash,
    }*/
    public enum IhorAttacks : int
    {
        Summon = 0, //23 seconds. Probably a lot of time to spend on intro
        MagicBurst,
        HomingDash,
        HomingSnowballs,

        StormPillars,
    }
    public class IhorHead : ModNPC
    {
        private int biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;
        private bool tailSpawned = false;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);

        }
        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.GetNPCDamageClamity();
            NPC.defense = 4;
            NPC.npcSlots = 12f;
            NPC.width = 114;
            NPC.height = 114;

            NPC.LifeMaxNERB(95000, 114400, 1650000);

            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.value = Item.buyPrice(0, 5, 0, 0);
            //NPC.alpha = 255;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;

            if (!Main.dedServ)
            {
                Music = Clamity.mod.GetMusicFromMusicMod("Ihor") ?? MusicID.Boss2;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(biomeEnrageTimer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            biomeEnrageTimer = reader.ReadInt32();
        }
        public override void OnSpawn(IEntitySource source)
        {
            Attack = (int)IhorAttacks.MagicBurst;
            PreviousAttack = (int)IhorAttacks.MagicBurst;
            AttackTimer = 0;
        }
        public Player player => Main.player[NPC.target];
        public ref float Attack => ref NPC.ai[0];
        public ref float AttackTimer => ref NPC.ai[1];
        public ref float PreviousAttack => ref NPC.ai[2];
        public override void AI()
        {
            #region Pre-Attack
            bool bossRush = BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || bossRush;
            bool masterMode = Main.masterMode || bossRush;
            bool revenge = CalamityWorld.revenge || bossRush;
            bool death = CalamityWorld.death || bossRush;

            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            // Enrage
            if (!player.ZoneSnow && !bossRush)
            {
                if (biomeEnrageTimer > 0)
                    biomeEnrageTimer--;
            }
            else
                biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;

            bool biomeEnraged = biomeEnrageTimer <= 0 || bossRush;

            float enrageScale = bossRush ? 1f : 0f;
            if (biomeEnraged)
            {
                NPC.Calamity().CurrentlyEnraged = !bossRush;
                enrageScale += 2f;
            }
            #endregion

            //Main AI
            //NPC.velocity = NPC.Center.SafeDirectionTo(player.Center) * 10;

            //NPC.velocity = (player.Center - NPC.Center) * 0.1f;
            //NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;

            if (Main.GlobalTimeWrappedHourly % 60 == 0)
            {
                ChromaticBurstParticle p = new(NPC.Center, Vector2.Zero, Color.LightBlue, 20, 6f, 0f);
            }

            AttackTimer++;
            switch ((IhorAttacks)((int)Attack))
            {
                case IhorAttacks.Summon:
                    AISummon();
                    break;
                case IhorAttacks.MagicBurst:
                    AIMagicBurst();
                    break;
                case IhorAttacks.HomingDash:
                    AIHomingDash();
                    break;
                case IhorAttacks.HomingSnowballs:
                    AIHomingSnowballs();
                    break;

                    /*case IhorAttacks.StormPillars:

                        break;*/
            }

            #region Summon body
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!tailSpawned && NPC.ai[0] == 0f)
                {
                    int previous = NPC.whoAmI;
                    int minLength = death ? 24 : revenge ? 21 : expertMode ? 18 : 15;


                    for (int i = 0; i < minLength + 1; i++)
                    {
                        int lol, ihorType;
                        if (i > (int)(minLength * 0.75f))
                            ihorType = ModContent.NPCType<IhorBodySmall>();
                        else
                            ihorType = ModContent.NPCType<IhorBody>();
                        lol = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<IhorBody>(), NPC.whoAmI);

                        Main.npc[lol].ai[2] = NPC.whoAmI;
                        Main.npc[lol].realLife = NPC.whoAmI;
                        Main.npc[lol].ai[1] = previous;
                        Main.npc[previous].ai[0] = lol;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, lol, 0f, 0f, 0f, 0);
                        previous = lol;
                    }
                }
                tailSpawned = true;
            }
            #endregion
        }
        private void SetRandomAttack()
        {
            List<int> list = new List<int>() { 1, 2, 3 };
            list.Remove((int)PreviousAttack);
            PreviousAttack = Attack;
            Attack = Main.rand.Next(list);
            AttackTimer = 0;
        }
        private void AISummon()
        {
            SetRandomAttack();

            if (AttackTimer == 20 * 60)
            {
                NPC.Opacity = 1;
                NPC.velocity = Vector2.UnitY * 10;
                NPC.Center = player.Center + Vector2.UnitY * 1000;
            }
            else if (AttackTimer < 20 * 60)
            {
                NPC.Opacity = 0;
                NPC.Center = player.Center + Vector2.UnitY * 1000;
            }
            if (AttackTimer > 23 * 60)
            {
                NPC.Opacity = 1;
                SetRandomAttack();
            }
        }
        private void AIMagicBurst()
        {
            NPC.velocity = (player.Center - NPC.Center) * 0.1f;
            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
        }
        private void AIHomingDash()
        {

        }
        private void AIHomingSnowballs()
        {

        }
        /*
        private void AI()
        {

        }
        */
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.IhorHead.Bestiary")
            });
        }
        public override bool CheckActive() => false;
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance * bossAdjustment);
        }
    }
}
