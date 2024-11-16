using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Cybermind.NPCs
{
    public class Cyberhive : ModNPC
    {
        public static int normalIconIndex;
        public static int phase2IconIndex;

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
            NPC.LifeMaxNERB(7700, 9200, 350000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 15, 0, 0);
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

    }
}
