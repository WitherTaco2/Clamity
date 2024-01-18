using CalamityMod;
using Clamity.Content.Boss.WoB.FrozenHell.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.FrozenHell.NPCs
{
    public class GlacialSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[this.NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            //NPC.CloneDefaults(NPCID.CaveBat);
            //AIType = NPCID.CaveBat;
            NPC.width = 66;
            NPC.height = 42;
            NPC.aiStyle = 1;
            AIType = 184;
            AnimationType = 184;
            NPC.value = 37500;

            NPC.knockBackResist = 0.1f;
            NPC.DR_NERD(0.1f);
            NPC.defense = 30;
            NPC.lifeMax = 10500;
            NPC.damage = 110;
            //NPC.Calamity().VulnerableToHeat = false;
            //NPC.Calamity().VulnerableToSickness = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[1]
              {
                ((ModSceneEffect) ModContent.GetInstance<FrozenHell.Biome.FrozenHell>()).Type
              };

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.Info.AddRange((IEnumerable<IBestiaryInfoElement>)new List<IBestiaryInfoElement>()
        {
            //BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
            new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.GlacialSlime.Bestiary")
        });
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 23, 30));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrozenHellstone>(), 1, 3, 6));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) => !spawnInfo.Player.Clamity().ZoneFrozenHell ? 0.0f : 0.25f;
    }
}
