using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs;
using CalamityMod.Particles;
using CalamityMod.World;
using Clamity.Commons;
using Clamity.Content.Bosses.Profusion.Drop;
using Clamity.Content.Bosses.Profusion.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Profusion.NPCs
{
    [AutoloadBossHead]
    public class ProfusionBoss : ModNPC
    {
        public enum ProfusionAIState : int
        {
            Awaken = 0,
            VerticaleMushroomStems = 1,
            ThormAttack = 2,
            Test = 999

        }
        private Player Target => Main.player[NPC.target];
        private int biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
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
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
        {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
            new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.ProfusionBoss.Bestiary")
        });
        private ref float State => ref NPC.ai[0];
        private ref float StateTimer => ref NPC.ai[1];
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
            if (!player.ZoneGlowshroom && !bossRush)
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

            //NPC.velocity = Vector2.Normalize(player.Center - NPC.Center) * 10f * (1 + enrageScale);
            StateTimer++;
            //if ()
            switch ((ProfusionAIState)State)
            {
                case ProfusionAIState.Awaken:
                    if (StateTimer == 1)
                    {
                        NPC.Center = Target.Center - Vector2.UnitY * 200;
                        SoundStyle roar = SoundID.Roar;
                        roar.Volume = 0.5f;
                        SoundEngine.PlaySound(roar, NPC.Center);
                    }
                    if (StateTimer % 15 == 0 /*&& Projectile.timeLeft > 40*/)
                    {
                        GeneralParticleHandler.SpawnParticle(new DirectionalPulseRing(NPC.Center, Vector2.Zero, Color.DarkBlue, new Vector2(0.5f, 0.5f), Main.rand.NextFloat(12f, 25f), 0f, 40f, 60));
                    }
                    if (StateTimer >= 60)
                        SetState(ProfusionAIState.ThormAttack);
                    break;
                case ProfusionAIState.ThormAttack:
                    NPC.velocity = Vector2.Lerp(NPC.Center.DirectionTo(Target.Center - Vector2.UnitY * 200), NPC.velocity, 0.02f) * 20;

                    if (StateTimer % 10 == 0)
                    {
                        SoundStyle summon = SoundID.NPCDeath13;
                        summon.Volume = 0.8f;
                        SoundEngine.PlaySound(summon, NPC.Center);

                        int type = ModContent.ProjectileType<MushroomThorn>();
                        int damage = NPC.GetProjectileDamageClamity(type);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(Target.Center).SafeNormalize(Vector2.Zero) * 15, type, damage, 1f, Main.myPlayer);
                    }
                    break;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            SetState(ProfusionAIState.Awaken);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.1f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        private void SetState(ProfusionAIState nextState)
        {
            State = (int)nextState;
            StateTimer = 0;
        }
        #region Drop
        public override void BossLoot(ref string name, ref int potionType) => potionType = ModContent.ItemType<SupremeHealingPotion>();
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
        #endregion
        #region Net Update
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(biomeEnrageTimer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            biomeEnrageTimer = reader.ReadInt32();
        }
        #endregion
    }
}
