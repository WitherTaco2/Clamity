using CalamityMod;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Items.Potions;
using CalamityMod.World;
using Clamity.Content.Boss.WoB.Drop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.NPCs
{    
    [AutoloadBossHead]
    public class WallOfBronze : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
            //NPCID.Sets.MPAllowedEnemies[this.Type] = true;
            Main.npcFrameCount[Type] = 2;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = Texture + "_BossChecklist",
                //CustomTexturePath = "CalamityMod/Projectiles/InvisibleProj",
                Scale = 0.75f,
                Position = new Vector2(50, 10),
                PortraitScale = 0.5f,
                PortraitPositionXOverride = 0,
                PortraitPositionYOverride = 0
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 72;
            NPC.height = 146;
            NPC.aiStyle = -1;
            NPC.damage = 200;
            NPC.defense = 70;
            NPC.lifeMax = 1500000;
            NPC.HitSound = new SoundStyle?(SoundID.NPCHit4);
            NPC.DeathSound = new SoundStyle?(SoundID.Item14);
            NPC.knockBackResist = 0.0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.value = Item.sellPrice(1, 50, 25, 75);
            NPC.npcSlots = 15f;
            if (Main.getGoodWorld)
                NPC.scale = 0.5f;

            if (!Main.dedServ)
            {
                Music = Clamity.mod.GetMusicFromMusicMod("WallOfBronze") ?? MusicID.Boss3;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.Info.AddRange((IEnumerable<IBestiaryInfoElement>)new List<IBestiaryInfoElement>()
        {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
            new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.WallOfBronze.Bestiary")
        });
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }
        public override void BossLoot(ref string name, ref int potionType) => potionType = ModContent.ItemType<OmegaHealingPotion>();
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 4; i++)
                Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[(Main.rand.Next(0, ListOfGuns.Length))], ai0: NPC.whoAmI);

            if (Main.netMode == 1 || !(source is EntitySource_BossSpawn entitySourceBossSpawn) || !(entitySourceBossSpawn.Target is Player target))
                return;
            NPC.position.X = (float)((target.Center.X < 2400f ? 480 : Main.maxTilesX * 16 - 480) - NPC.width / 2);
            NPC.position.Y = target.Center.Y - NPC.height / 2f;
            if (Main.netMode != 2)
                return;
            NetMessage.SendData(23, -1, -1, (NetworkText)null, NPC.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
        }
        private int[] ListOfGuns = new int[3]
        {
            ModContent.NPCType<WallOfBronzeTorret>(),
            ModContent.NPCType<WallOfBronzeLaser>(),
            ModContent.NPCType<WallOfBronzeClaw>()
        };
        private ref float GunSummonTimer => ref NPC.ai[0];
        private ref float DeathrayTimer => ref NPC.ai[1];
        public override void AI()
        {
            int num1 = 0;
            Mod mod1;
            if (ModLoader.TryGetMod("CalamityMod", out mod1))
            {
                if ((bool)mod1.Call(new object[2]
                {
                    "GetDifficultyActive",
                    "Death"
                }))
                    num1 = 2;
                else if ((bool)mod1.Call(new object[2]
                {
                    "GetDifficultyActive",
                    "Revengeance"
                }))
                    num1 = 1;
            }
            if (NPC.target < 0 || NPC.target == byte.MaxValue || Main.player[NPC.target].Center.Y < Main.UnderworldLayer * 16 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
                if ((NPC.target == (int)byte.MaxValue || Main.player[NPC.target].Center.Y < Main.UnderworldLayer * 16 || Main.player[NPC.target].dead || !Main.player[NPC.target].active) && !NPC.despawnEncouraged)
                    NPC.EncourageDespawn(30);
                if (NPC.despawnEncouraged)
                {
                    NPC.velocity.X += Math.Sign(NPC.velocity.X) * 2f;
                    NPC.velocity.Y = 0.0f;
                    return;
                }
            }

            //Base movement
            Vector2 center = Main.player[NPC.target].Center;
            if (NPC.velocity.X == 0f)
            {
                NPC.velocity.X = Math.Sign(center.X - NPC.Center.X) * 2f;
            }
            else
            {
                NPC.spriteDirection = NPC.direction = Math.Sign(NPC.velocity.X);
                NPC.velocity.X = Math.Sign(NPC.velocity.X) * (Main.expertMode ? MathHelper.Lerp(7f + num1, 4f, (float)NPC.life / (float)NPC.lifeMax) : 2f);
                NPC.velocity.Y = Math.Sign(center.Y - NPC.Center.Y) * 2;
            }

            //Horrified debuff and You can t escape
            foreach (Player player in Main.player) 
            {
                if (player == null) continue;
                if (!player.active) continue;
                if (player.Center.Y < Main.UnderworldLayer * 16)
                    player.AddBuff(BuffID.Horrified, 2);

                if (NPC.direction > 0)
                {
                    if (player.Center.X < NPC.Center.X)
                    {
                        player.velocity.X = 10;
                        player.position.X += 10;
                    }
                }
                else if (NPC.direction < 0)
                {
                    if (player.Center.X > NPC.Center.X)
                    {
                        player.velocity.X = -10;
                        player.position.X -= 10;
                    }
                }

                

            }

            //Shield
            int num3 = 0;
            foreach (Terraria.NPC npc in Main.npc)
            {
                if (npc == null) continue;
                if (!npc.active) continue;
                if (ListOfGuns.Contains<int>(npc.type))
                    num3++;
            }
            if (num3 >= (CalamityWorld.death ? 2 : (CalamityWorld.revenge ? 3 : 4)))
                NPC.dontTakeDamage = true;
            else 
                NPC.dontTakeDamage = false;

            //Summon guns
            GunSummonTimer++;
            //if (GunSummonTimer >= /*(CalamityWorld.death ? 1200 : (Main.expertMode ? 1500 : 1800))*/ 120 && num3 < 5)
            if (GunSummonTimer >= (CalamityWorld.death ? 1200 : (Main.expertMode ? 1500 : 1800)) && num3 < 5)
            {
                GunSummonTimer = 0;
                if (CanSecondStage)
                {
                    Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[2], ai0: NPC.whoAmI);
                    Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[2], ai0: NPC.whoAmI);
                    Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[2], ai0: NPC.whoAmI);
                }
                else
                {
                    Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[(Main.rand.Next(0, ListOfGuns.Length))], ai0: NPC.whoAmI);
                    Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[(Main.rand.Next(0, ListOfGuns.Length))], ai0: NPC.whoAmI);
                    Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[(Main.rand.Next(0, ListOfGuns.Length))], ai0: NPC.whoAmI);
                }
            }

            //Deathray
            if (CanSecondStage)
            {
                DeathrayTimer++;
                if (DeathrayTimer > 2000)
                {

                }
            }

        }
        private bool CanSecondStage
        {
            get => (Main.masterMode || CalamityWorld.revenge) && NPC.life < NPC.lifeMax / 10;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture2D1 = ModContent.Request<Texture2D>(Texture + "_ExtraBack").Value;
            int num = Main.screenHeight / 32 + 1;
            SpriteEffects spriteEffects = NPC.spriteDirection != 1 ? (SpriteEffects)1 : (SpriteEffects)0;
            for (int index = -num; index <= num; ++index)
            {
                if (Main.UnderworldLayer < (int)(Main.LocalPlayer.Center.Y / 16f) + index)
                    spriteBatch.Draw(texture2D1, 
                                    new Vector2(NPC.Center.X - NPC.spriteDirection * texture2D1.Width * 0.5f + Math.Sign(NPC.velocity.X) * 100, (float)((int)Main.LocalPlayer.Center.Y / 200 * 200 + index * texture2D1.Height)) - screenPos, 
                                    new Rectangle?(), 
                                    Lighting.GetColor((int)(NPC.Center.X - NPC.spriteDirection * texture2D1.Width * 0.5) / 16, (int)(Main.LocalPlayer.Center.Y / 16) + index), 
                                    0.0f, 
                                    Utils.Size(texture2D1) * 0.5f, 
                                    NPC.scale, 
                                    spriteEffects, 
                                    0.0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture2D1 = ModContent.Request<Texture2D>(Texture + "_Extra").Value;
            int num = Main.screenHeight / 32 + 1;
            SpriteEffects spriteEffects = NPC.spriteDirection != 1 ? (SpriteEffects)1 : (SpriteEffects)0;
            for (int index = -num; index <= num; ++index)
            {
                if (Main.UnderworldLayer < (int)(Main.LocalPlayer.Center.Y / 16f) + index)
                    spriteBatch.Draw(texture2D1,
                                    new Vector2(NPC.Center.X - NPC.spriteDirection * texture2D1.Width * 0.5f - Math.Sign(NPC.velocity.X), (float)((int)Main.LocalPlayer.Center.Y / 200 * 200 + index * texture2D1.Height)) - screenPos,
                                    new Rectangle?(),
                                    Lighting.GetColor((int)(NPC.Center.X - NPC.spriteDirection * texture2D1.Width * 0.5) / 16, (int)(Main.LocalPlayer.Center.Y / 16) + index),
                                    0.0f,
                                    Utils.Size(texture2D1) * 0.5f,
                                    NPC.scale,
                                    spriteEffects,
                                    0.0f);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (CanSecondStage)
                NPC.frame = new Rectangle(0, 146, NPC.width, NPC.height);
            else
                NPC.frame = new Rectangle(0, 0, NPC.width, NPC.height);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<WoBTreasureBag>()));
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();
            int[] itemIDs = new int[3]
            {
                ModContent.ItemType<AMS>(),
                ModContent.ItemType<TheWOBbler>(),
                ModContent.ItemType<LargeFather>()
            };
            mainRule.Add(ItemDropRule.OneFromOptions(1, itemIDs));

            mainRule.Add(ItemDropRule.Common(ModContent.ItemType<ThankYouPainting>(), 100));
            mainRule.Add(ItemDropRule.Common(ModContent.ItemType<WoBMask>(), 7));
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<WoBRelic>());
            npcLoot.Add(ModContent.ItemType<WoBTrophy>(), 10);
            for (int i = 0; i < 3; i++)
            {
                npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.CopperBar, 1, 10, 10, true);
                npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.TinBar, 1, 10, 10, true);
            }
            npcLoot.AddConditionalPerPlayer(() => !ClamitySystem.downedWallOfBronze, ModContent.ItemType<WoBLore>(), ui: true, DropHelper.FirstKillText);

        }
        public override void OnKill()
        {
            ClamitySystem.downedWallOfBronze = true;
            CalamityNetcode.SyncWorld();
        }
    }
}
