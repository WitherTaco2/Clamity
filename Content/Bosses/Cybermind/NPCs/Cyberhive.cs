using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.World;
using Microsoft.Xna.Framework;
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
        Backdash = 1,

        //Phase 1
        Rotating = 11,
        NormalDash = 12,
        GasDash = 13,

        //Phase 2
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
        private bool IsPhaseTwo => (NPC.life / (float)NPC.lifeMax) < 0.5f;
        public int Attacks
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public CyberhiveAttacks GetAttacks => (CyberhiveAttacks)NPC.ai[0];
        public int AttacksTimer
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
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            biomeEnrageTimer = reader.ReadInt32();
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame = new Rectangle(0, IsPhaseTwo ? 136 : 0, 198, 136);
        }

        public override void AI()
        {
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

            if (IsPhaseTwo)
            {

            }
            AttacksTimer++;
            switch (GetAttacks)
            {
                case CyberhiveAttacks.Spawn:
                    SetNextAttack();
                    break;
                case CyberhiveAttacks.Rotating:

                    break;
                case CyberhiveAttacks.NormalDash:

                    break;
                case CyberhiveAttacks.GasDash:

                    break;

                case CyberhiveAttacks.HyperVerticalDash:

                    break;
                case CyberhiveAttacks.GammaDeathRay:

                    break;
                case CyberhiveAttacks.GooDash:

                    break;
                case CyberhiveAttacks.ShootVerticalRay:

                    break;
            }
        }
        public void SetNextAttack()
        {
            Attacks++;
            if (IsPhaseTwo)
            {
                if (Attacks > 104)
                    Attacks = 101;
            }
            else
            {
                if (Attacks > 13)
                    Attacks = 11;

            }
            AttacksTimer = 0;
        }
    }
}
