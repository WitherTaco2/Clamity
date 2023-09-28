using Terraria.GameContent.ItemDropRules;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Clamity.Content.Items.Weapons.Ranged;
using Clamity.Content.Items.Materials;
using Clamity.Content.Items.Weapons.Melee.Swords;
using CalamityMod.NPCs.PlaguebringerGoliath;
using Clamity.Content.Items.Weapons.Melee.Shortswords;

namespace Clamity
{
    public class ClamityGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            Conditions.IsHardmode hm = new Conditions.IsHardmode();
            if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LeadWizard>(), 4));
            }
            if (npc.type == ModContent.NPCType<PlaguebringerGoliath>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Disease>(), 10));
            }

            if (ContainType(npc.type, NPCID.Mummy, NPCID.LightMummy, NPCID.DarkMummy, NPCID.BloodMummy, 
                NPCID.DesertBeast, NPCID.DesertScorpionWalk, NPCID.DesertScorpionWall, 
                NPCID.DesertDjinn, NPCID.DesertLamiaDark, NPCID.DesertLamiaLight,
                NPCID.DesertGhoul, NPCID.DesertGhoulCorruption, NPCID.DesertGhoulCrimson, NPCID.DesertGhoulHallow,
                NPCID.DuneSplicerHead)
            )
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EssenceOfHeat>(), 4));
            }
            if (ContainType(npc.type, NPCID.Vulture, NPCID.TombCrawlerHead))
            {
                npcLoot.Add(ItemDropRule.ByCondition(hm, ModContent.ItemType<EssenceOfHeat>(), 4));
            }
            if (ContainType(npc.type, NPCID.Antlion, NPCID.WalkingAntlion, NPCID.GiantWalkingAntlion, NPCID.FlyingAntlion,
                NPCID.GiantFlyingAntlion)
            )
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MandibleClaws>(), 50));
                npcLoot.Add(ItemDropRule.ByCondition(hm, ModContent.ItemType<EssenceOfHeat>(), 4));
            }
        }
        private bool ContainType(int npcid, params int[] array)
        {
            bool num = false;
            foreach (int i in array)
            {
                if (i == npcid)
                {
                    num = true;
                    break;
                }
            }
            return num;
        }
    }
}
