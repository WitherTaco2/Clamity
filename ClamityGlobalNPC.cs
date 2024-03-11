using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.TownNPCs;
using Clamity.Content.Biomes.FrozenHell.Items;
using Clamity.Content.Items.Materials;
using Clamity.Content.Items.Mounts;
using Clamity.Content.Items.Potions.Food;
using Clamity.Content.Items.Weapons.Classless;
using Clamity.Content.Items.Weapons.Melee.Shortswords;
using Clamity.Content.Items.Weapons.Melee.Swords;
using Clamity.Content.Items.Weapons.Ranged.Guns;
using Terraria.GameContent.ItemDropRules;

namespace Clamity
{
    public class ClamityGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool IncreasedHeatEffects_PyroStone;
        //public int wCleave;
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            Conditions.IsHardmode hm = new Conditions.IsHardmode();
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();

            //Boss Drop
            if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LeadWizard>(), 4));
            }
            if (npc.type == ModContent.NPCType<PlaguebringerGoliath>())
            {
                mainRule.Add(ItemDropRule.Common(ModContent.ItemType<Disease>(), 4));
                mainRule.Add(ItemDropRule.Common(ModContent.ItemType<PlagueStation>()));
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<TrashOfMagnus>(), 4, 3));
            }
            if (npc.type == ModContent.NPCType<SupremeCalamitas>())
            {
                mainRule.Add(ItemDropRule.Common(ModContent.ItemType<Calamitea>(), 1, 10, 10));
            }


            //Essence of Flame drop
            if (ContainType(npc.type, NPCID.Mummy, NPCID.LightMummy, NPCID.DarkMummy, NPCID.BloodMummy,
                NPCID.DesertBeast, NPCID.DesertScorpionWalk, NPCID.DesertScorpionWall,
                NPCID.DesertDjinn, NPCID.DesertLamiaDark, NPCID.DesertLamiaLight,
                NPCID.DesertGhoul, NPCID.DesertGhoulCorruption, NPCID.DesertGhoulCrimson, NPCID.DesertGhoulHallow,
                NPCID.DuneSplicerHead)
            )
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EssenceOfFlame>(), 4));
            }
            if (ContainType(npc.type, NPCID.Vulture, NPCID.TombCrawlerHead))
            {
                npcLoot.Add(ItemDropRule.ByCondition(hm, ModContent.ItemType<EssenceOfFlame>(), 4));
            }
            if (ContainType(npc.type, NPCID.Antlion, NPCID.WalkingAntlion, NPCID.GiantWalkingAntlion, NPCID.FlyingAntlion,
                NPCID.GiantFlyingAntlion)
            )
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MandibleClaws>(), 50));
                npcLoot.Add(ItemDropRule.ByCondition(hm, ModContent.ItemType<EssenceOfFlame>(), 4));
            }


            //Food drop
            if (ContainType(npc.type, ModContent.NPCType<SeaSerpent1>(), ModContent.NPCType<EutrophicRay>(), ModContent.NPCType<GhostBell>(), ModContent.NPCType<PrismBack>(), ModContent.NPCType<SeaFloaty>(), ModContent.NPCType<BlindedAngler>()))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ClamChowder>(), 20));
            }
            if (ContainType(npc.type, ModContent.NPCType<Clam>()))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ClamChowder>(), 10));
            }
            if (ContainType(npc.type, ModContent.NPCType<GiantClam>()))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ClamChowder>(), 2));
            }
            if (ContainType(npc.type, ModContent.NPCType<ChaoticPuffer>(), ModContent.NPCType<GiantSquid>(), ModContent.NPCType<Laserfish>(), ModContent.NPCType<OarfishHead>(), ModContent.NPCType<Eidolist>(), ModContent.NPCType<MirageJelly>(), ModContent.NPCType<Bloatfish>()))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Barolegs>(), 20));
            }
            if (ContainType(npc.type, ModContent.NPCType<EidolonWyrmHead>(), ModContent.NPCType<GulperEelHead>(), ModContent.NPCType<ColossalSquid>(), ModContent.NPCType<ReaperShark>(), ModContent.NPCType<BobbitWormHead>()))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Barolegs>(), 4));
            }


            if (NPCID.Sets.Zombies[npc.type])
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DisgustingMeat>(), 4, 1, 2));
            }
            if (ContainType(npc.type, NPCID.EaterofWorldsBody))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DisgustingMeat>(), 1, 2, 4));
            }
            if (npc.type == NPCID.PartyGirl)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DisgustingMeat>(), 1, 30, 30));
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
        /*public override void PostAI(NPC npc)
        {
            if (wCleave > 0)
                --wCleave;
            int num = npc.defense - wCleave > 0 ? WarCleave.DefenseReduction : 0;
            if (num < 0)
                num = 0;
        }*/
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            bool flag1 = CalamityLists.DesertScourgeIDs.Contains(npc.type) || CalamityLists.EaterofWorldsIDs.Contains(npc.type) || CalamityLists.PerforatorIDs.Contains(npc.type) || CalamityLists.AquaticScourgeIDs.Contains(npc.type) || CalamityLists.AstrumDeusIDs.Contains(npc.type) || CalamityLists.StormWeaverIDs.Contains(npc.type);
            bool flag2 = CalamityLists.SlimeGodIDs.Contains(npc.type);
            bool flag3 = npc.drippingSlime || npc.drippingSparkleSlime;

            float num7 = flag3 ? (flag1 | flag2 ? 1.5f : 2f) : 1f;
            if (npc.Calamity().VulnerableToHeat.HasValue)
            {
                if (npc.Calamity().VulnerableToHeat.Value)
                    num7 *= flag3 ? (flag1 | flag2 ? 1.25f : 1.5f) : (flag1 | flag2 ? 1.5f : 2f);
                else
                    num7 *= flag3 ? 0.2f : 0.5f;
            }
            if (IncreasedHeatEffects_PyroStone)
                num7 += 0.5f;
            float num12 = num7 - 1f;

            if (npc.onFire)
            {
                int num15 = (int)(12.0 * num12);
                npc.lifeRegen -= num15;
                if (damage < num15 / 4)
                    damage = num15 / 4;
            }
            if (npc.onFire2)
            {
                int num16 = (int)(48.0 * num12);
                npc.lifeRegen -= num16;
                if (damage < num16 / 4)
                    damage = num16 / 4;
            }
            if (npc.onFire3)
            {
                int num17 = (int)(30.0 * num12);
                npc.lifeRegen -= num17;
                if (damage < num17 / 4)
                    damage = num17 / 4;
            }
            if (npc.daybreak)
            {
                int num18 = 0;
                for (int index = 0; index < Main.maxProjectiles; ++index)
                {
                    if (((Entity)Main.projectile[index]).active && Main.projectile[index].type == 636 && (double)Main.projectile[index].ai[0] == 1.0 && (double)Main.projectile[index].ai[1] == (double)((Entity)npc).whoAmI)
                        ++num18;
                }
                int num19 = (int)((num18 <= 1 ? 1.0 : 1.0 + 0.25 * (double)(num18 - 1)) * 2.0 * 100.0 * num12);
                npc.lifeRegen -= num19;
                if (damage < num19 / 4)
                    damage = num19 / 4;
            }
            if (npc.shadowFlame)
            {
                int num20 = (int)(60.0 * num12);
                npc.lifeRegen -= num20;
                if (damage < num20 / 4)
                    damage = num20 / 4;
            }

            bool flag5 = npc.onFrostBurn || npc.onFrostBurn2;
            bool flag6 = npc.onFire || npc.onFire2 || npc.onFire3 || npc.shadowFlame;
            bool flag7 = npc.Calamity().bFlames > 0 || npc.Calamity().hFlames > 0 || npc.Calamity().gsInferno > 0 || npc.Calamity().dragonFire > 0 || npc.Calamity().banishingFire > 0 || npc.Calamity().vulnerabilityHex > 0;
            if (npc.oiled && flag5 | flag6 | flag7)
            {
                double num23 = 1.0;
                if (flag6 || flag7 & flag5)
                    num23 *= num12;
                if (flag7 && !flag5 && !flag6)
                    num23 *= num7;
                int num24 = (int)(50.0 * num23);
                npc.lifeRegen -= num24;
                if (damage < num24 / 4)
                    damage = num24 / 4;
            }
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Steampunker)
                shop.Add<CyanSolution>(new Condition(Language.GetOrRegister("Mods.Clamity.Misc.DefeatedWoB"), () => ClamitySystem.downedWallOfBronze));
            if (shop.NpcType == ModContent.NPCType<DILF>())
                shop.Add<ColdheartIcicle>();
        }
    }
}
