using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.NPCs;
using CalamityMod.Sounds;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.KosFragmentBosses.CeasingVoid.NPCs
{
    public class CeasingVoid : ModNPC
    {
        public enum CeasingVoidStates : int
        {
            Awaken = 0

        }
        private Player Target => Main.player[NPC.target];
        private int biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 116;
            NPC.height = 176;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 100;
            NPC.defense = 10;
            NPC.DR_NERD(0.5f);
            //NPC.lifeMax = 777000;
            NPC.LifeMaxNERB(5000, 6600, 1500000);
            NPC.DeathSound = new("CalamityMod/Sounds/NPCKilled/CeaselessVoidDeath");
            NPC.knockBackResist = 0.0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.value = Item.sellPrice(0, 5);
            NPC.npcSlots = 15f;
            NPC.netUpdate = true;
            //NPC.chaseable = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().canBreakPlayerDefense = true;
            if (Main.getGoodWorld)
                NPC.scale = 1.5f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
        {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
            new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.CeasingVoid.Bestiary")
        });
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0 && NPC.life >= NPC.lifeMax * 0.05f)
            {
                NPC.soundDelay = 8;
                float pitchVar = 0;
                if (Main.zenithWorld)
                {
                    pitchVar = Main.rand.Next(-60, 41) * 0.01f;
                }
                SoundEngine.PlaySound(CommonCalamitySounds.OtherwordlyHitSound with { Pitch = CommonCalamitySounds.OtherwordlyHitSound.Pitch + pitchVar }, NPC.Center);
            }
        }
        private ref float State => ref NPC.ai[0];
        private ref float StateTimer => ref NPC.ai[1];
        public override void AI()
        {
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
                    //NPC.despawnEncouraged
                    NPC.velocity.Y -= Math.Sign(NPC.velocity.Y) * 2f;
                    return;
                }
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

            // Enrage
            if (!player.ZoneDungeon && !bossRush)
            {
                if (biomeEnrageTimer > 0)
                    biomeEnrageTimer--;
            }
            else
                biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;

            bool biomeEnraged = biomeEnrageTimer <= 0 || bossRush;

            float enrageScale = bossRush ? 2f : 1f;
            if (biomeEnraged && (!player.ZoneDungeon || bossRush))
            {
                NPC.Calamity().CurrentlyEnraged = !bossRush;
                enrageScale += 1f;
            }

            if (Main.getGoodWorld)
                enrageScale += 0.5f;

        }
        public override void OnSpawn(IEntitySource source)
        {
            SetState(CeasingVoidStates.Awaken);
        }
        private void SetState(CeasingVoidStates nextState)
        {
            State = (int)nextState;
            StateTimer = 0;
        }
        public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.HealingPotion;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();


            mainRule.Add(ItemDropRule.Common(ModContent.ItemType<ThankYouPainting>(), 100));
            //Trophy
            //npcLoot.Add(ModContent.ItemType<>(), 10);
            //Relic
            //npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<>());
            //Mask
            //mainRule.Add(ItemDropRule.Common(ModContent.ItemType<>(), 7));
            //Lore
            //npcLoot.AddConditionalPerPlayer(() => !ClamitySystem.downedProfusion, ModContent.ItemType<>(), ui: true, DropHelper.FirstKillText);
            //GFB drop
            //npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID., 1, 1, 1, true);
        }
    }
}
