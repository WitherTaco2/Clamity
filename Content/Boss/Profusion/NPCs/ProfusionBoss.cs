using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs;
using CalamityMod.World;
using Clamity.Content.Boss.Profusion.Drop;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Profusion.NPCs
{
    [AutoloadBossHead]
    public class ProfusionBoss : ModNPC
    {
        private int biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[this.Type] = true;
            Main.npcFrameCount[Type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.width = 180;
            NPC.height = 186;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 200;
            NPC.defense = 40;
            NPC.DR_NERD(0.25f);
            //NPC.lifeMax = 777000;
            NPC.LifeMaxNERB(777000, 1300000, 1700000);
            NPC.HitSound = new SoundStyle?(SoundID.NPCHit13);
            NPC.DeathSound = new SoundStyle?(SoundID.NPCDeath24);
            NPC.knockBackResist = 0.0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.value = Item.sellPrice(1);
            NPC.npcSlots = 15f;
            NPC.netUpdate = true;
            NPC.chaseable = true;
            //if (Main.getGoodWorld)
            //    NPC.scale = 1.5f;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;

            if (Main.getGoodWorld)
            {
                NPC.scale *= 0.7f;
            }

            if (!Main.dedServ)
            {
                //Music = Clamity.mod.GetMusicFromMusicMod("") ?? MusicID.Boss1;
                Music = MusicID.Boss1;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.Info.AddRange((IEnumerable<IBestiaryInfoElement>)new List<IBestiaryInfoElement>()
        {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
            new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.ProfusionBoss.Bestiary")
        });
        public override void BossLoot(ref string name, ref int potionType) => potionType = ModContent.ItemType<SupremeHealingPotion>();
        private ref float Attack => ref NPC.ai[0];
        private ref float AttackTimer => ref NPC.ai[1];
        public override void AI()
        {
            //Main.player[NPC.target].ZoneGlowshroom
            bool bossRush = BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || bossRush;
            bool revenge = CalamityWorld.revenge || bossRush;
            bool expertMode = Main.expertMode || bossRush;

            // Get a target
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            // Despawn safety, make sure to target another player if the current player target is too far away
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > CalamityGlobalNPC.CatchUpDistance200Tiles)
                NPC.TargetClosest();

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity.Y -= Math.Sign(NPC.velocity.Y) * 2f;
                    return;
                }
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

            // Enrage
            if ((!player.ZoneGlowshroom) && !bossRush)
            {
                if (biomeEnrageTimer > 0)
                    biomeEnrageTimer--;
            }
            else
                biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;

            bool biomeEnraged = biomeEnrageTimer <= 0 || bossRush;

            float enrageScale = bossRush ? 1f : 0f;
            if (biomeEnraged && (!player.ZoneGlowshroom || bossRush))
            {
                NPC.Calamity().CurrentlyEnraged = !bossRush;
                enrageScale += 1f;
            }

            if (Main.getGoodWorld)
                enrageScale += 0.5f;

            NPC.velocity = Vector2.Normalize(player.Center - NPC.Center) * 10f * (1 + enrageScale);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.1f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();


            mainRule.Add(ItemDropRule.Common(ModContent.ItemType<ThankYouPainting>(), 100));
            //Trophy
            npcLoot.Add(ModContent.ItemType<ProfusionTrophy>(), 10);
            //Relic
            //npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<>());
            //Mask
            //mainRule.Add(ItemDropRule.Common(ModContent.ItemType<>(), 7));
            //Lore
            //npcLoot.AddConditionalPerPlayer(() => !ClamitySystem.downedProfusion, ModContent.ItemType<>(), ui: true, DropHelper.FirstKillText);
            //GFB drop
            npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.Mushroom, 1, 4999, 9999, true);
        }
        public override void OnKill()
        {
            ClamitySystem.downedProfusion = true;
            CalamityNetcode.SyncWorld();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(biomeEnrageTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            biomeEnrageTimer = reader.ReadInt32();
        }
    }
}
