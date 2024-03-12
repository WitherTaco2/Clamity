using Clamity.Content.NPCs;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace Clamity.Content.Bosses.WoB.NPCs
{
    public class MechanicalLeechHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<MechanicalLeechBody>();

        public override int TailType => ModContent.NPCType<MechanicalLeechTail>();

        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = Texture + "_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);

        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.width = 8;
            NPC.height = 26;
            //NPC.lifeMax = 1200;
            NPC.LifeMaxNERB(1200, 3500, 5000);
            NPC.DR_NERD(0.14f);
            NPC.defense = 50;
            NPC.damage = 160;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.lavaImmune = true;
            NPC.value = 0;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

                new FlavorTextBestiaryInfoElement("Mechanical Leeches are Hardmode boss servant enemies that are spawned by The Wall of Bronze. ")
            });
        }

        public override void Init()
        {
            MinSegmentLength = 8;
            MaxSegmentLength = 12;
            CanFly = true;
            MoveSpeed = 1000;

            CommonWormInit(this);
        }

        internal static void CommonWormInit(Worm worm)
        {
            // These two properties handle the movement of the worm
            worm.MoveSpeed = 5.5f;
            worm.Acceleration = 0.045f;
        }

        private int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackCounter > 0)
                {
                    attackCounter--; // tick down the attack counter.
                }

                Player target = Main.player[NPC.target];
                if (attackCounter <= 0 && Vector2.Distance(NPC.Center, target.Center) < 200 && Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1))
                {
                    Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 5, ProjectileID.DeathLaser, 100, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 300;
                    attackCounter = 500;
                    NPC.netUpdate = true;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Heart, 1));
        }
    }
    public class MechanicalLeechBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.width = 8;
            NPC.height = 8;
            NPC.lifeMax = 1200;
            NPC.defense = 50;
            NPC.damage = 160;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.lavaImmune = true;
            NPC.value = 0;
        }

        public override void Init()
        {
            MechanicalLeechHead.CommonWormInit(this);
        }
    }
    public class MechanicalLeechTail : WormTail
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerTail);
            NPC.width = 8;
            NPC.height = 14;
            NPC.lifeMax = 1200;
            NPC.defense = 50;
            NPC.damage = 160;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.lavaImmune = true;
            NPC.value = 0;
        }

        public override void Init()
        {
            MechanicalLeechHead.CommonWormInit(this);
        }
    }
}
