﻿using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Items.Potions;
using CalamityMod.World;
using Clamity.Content.Biomes.FrozenHell.Items;
using Clamity.Content.Bosses.WoB.Drop;
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

namespace Clamity.Content.Bosses.WoB.NPCs
{
    [AutoloadBossHead]
    public class WallOfBronze : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
            //NPCID.Sets.MPAllowedEnemies[this.Type] = true;
            Main.npcFrameCount[Type] = 2;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = Texture + "_Bestiary",
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
            //NPC.lifeMax = 1500000;
            NPC.LifeMaxNERB(1500300, 288090, 288090);
            NPC.HitSound = new SoundStyle?(SoundID.NPCHit4);
            NPC.DeathSound = new SoundStyle?(SoundID.Item14);
            NPC.knockBackResist = 0.0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.value = Item.sellPrice(1, 50, 25, 75);
            NPC.npcSlots = 15f;
            NPC.netUpdate = true;
            //if (Main.getGoodWorld)
            //    NPC.scale = 1.5f;

            if (!Main.dedServ)
            {
                Music = Clamity.mod.GetMusicFromMusicMod("WallOfBronzeOld") ?? MusicID.Boss3;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.Info.AddRange((IEnumerable<IBestiaryInfoElement>)new List<IBestiaryInfoElement>()
        {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
            new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.WallOfBronze.Bestiary")
        });
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * balance * bossAdjustment);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier() * bossAdjustment);
        }
        public override void BossLoot(ref string name, ref int potionType) => potionType = ModContent.ItemType<OmegaHealingPotion>();
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 4; i++)
                Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[(Main.rand.Next(0, ListOfGuns.Length))], ai0: NPC.whoAmI);

            if (Main.netMode == NetmodeID.MultiplayerClient || !(source is EntitySource_BossSpawn entitySourceBossSpawn) || !(entitySourceBossSpawn.Target is Player target))
                return;
            NPC.position.X = (float)((target.Center.X < 2400f ? 480 : Main.maxTilesX * 16 - 480) - NPC.width / 2);
            NPC.position.Y = target.Center.Y - NPC.height / 2f;
            if (Main.netMode != NetmodeID.Server)
                return;
            NetMessage.SendData(MessageID.SyncNPC, -1, -1, (NetworkText)null, NPC.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
        }
        private readonly int[] ListOfGuns = new int[3]
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
                if ((NPC.target == (int)byte.MaxValue || Main.player[NPC.target].dead || !Main.player[NPC.target].active) || Main.player[NPC.target].Center.Y < Main.UnderworldLayer * 16)
                {
                    NPC.TargetClosest(true);
                    if ((NPC.target == (int)byte.MaxValue || Main.player[NPC.target].dead || !Main.player[NPC.target].active) || Main.player[NPC.target].Center.Y < Main.UnderworldLayer * 16 && !NPC.despawnEncouraged)
                        NPC.EncourageDespawn(30);
                }
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
            if (num3 >= ((CalamityWorld.revenge || BossRushEvent.BossRushActive) ? 3 : 4))
                NPC.dontTakeDamage = true;
            else
                NPC.dontTakeDamage = false;

            //Summon guns
            GunSummonTimer++;
            //if (GunSummonTimer >= /*(CalamityWorld.death ? 1200 : (Main.expertMode ? 1500 : 1800))*/ 120 && num3 < 5)
            if (GunSummonTimer >= (CalamityWorld.death ? 600 : (Main.expertMode ? 750 : 900)))
            {
                GunSummonTimer = 0;
                Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MechanicalLeechHead>());
                Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MetalMaw>());
                Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MetalMaw>());
                if (num3 < 5)
                {
                    if (CanSecondStage)
                    {
                        Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[2], ai0: NPC.whoAmI);
                        Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[2], ai0: NPC.whoAmI);
                        //Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[2], ai0: NPC.whoAmI);
                    }
                    else
                    {
                        Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[(Main.rand.Next(0, ListOfGuns.Length))], ai0: NPC.whoAmI);
                        Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[(Main.rand.Next(0, ListOfGuns.Length))], ai0: NPC.whoAmI);
                        //Terraria.NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ListOfGuns[(Main.rand.Next(0, ListOfGuns.Length))], ai0: NPC.whoAmI);
                    }
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
            if (Main.getGoodWorld || Main.xMas)
                texture2D1 = ModContent.Request<Texture2D>(Texture + "_ExtraBack_GFB").Value;
            int num = Main.screenHeight / 32 + 1;
            int num1 = texture2D1.Height;
            SpriteEffects spriteEffects = NPC.spriteDirection != 1 ? (SpriteEffects)1 : (SpriteEffects)0;
            for (int index = -num; index <= num; ++index)
            {
                if (Main.UnderworldLayer < (int)(Main.LocalPlayer.Center.Y / 16f) + index)
                    spriteBatch.Draw(texture2D1,
                                    new Vector2(NPC.Center.X - NPC.spriteDirection * texture2D1.Width * 0.5f + Math.Sign(NPC.velocity.X) * 100, (float)((int)Main.LocalPlayer.Center.Y / num1 * num1 + index * texture2D1.Height)) - screenPos,
                                    new Rectangle?(),
                                    Lighting.GetColor((int)(NPC.Center.X - NPC.spriteDirection * texture2D1.Width * 0.5) / 16, (int)(Main.LocalPlayer.Center.Y / 16) + index),
                                    0.0f,
                                    Utils.Size(texture2D1) * 0.5f,
                                    1f,
                                    spriteEffects,
                                    0.0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture2D1 = ModContent.Request<Texture2D>(Texture + "_Extra").Value;
            if (Main.getGoodWorld || Main.xMas)
                texture2D1 = ModContent.Request<Texture2D>(Texture + "_Extra_GFB").Value;
            int num = Main.screenHeight / 32 + 1;
            int num1 = texture2D1.Height;
            SpriteEffects spriteEffects = NPC.spriteDirection != 1 ? (SpriteEffects)1 : (SpriteEffects)0;
            for (int index = -num; index <= num; ++index)
            {
                if (Main.UnderworldLayer < (int)(Main.LocalPlayer.Center.Y / 16f) + index)
                    spriteBatch.Draw(texture2D1,
                                    new Vector2(NPC.Center.X - NPC.spriteDirection * texture2D1.Width * 0.5f - Math.Sign(NPC.velocity.X), (float)((int)Main.LocalPlayer.Center.Y / num1 * num1 + index * texture2D1.Height)) - screenPos,
                                    new Rectangle?(),
                                    Lighting.GetColor((int)(NPC.Center.X - NPC.spriteDirection * texture2D1.Width * 0.5) / 16, (int)(Main.LocalPlayer.Center.Y / 16) + index),
                                    0.0f,
                                    Utils.Size(texture2D1) * 0.5f,
                                    1f,
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
            //Trophy
            npcLoot.Add(ModContent.ItemType<WoBTrophy>(), 10);
            //Relic
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<WoBRelic>());
            //Mask
            mainRule.Add(ItemDropRule.Common(ModContent.ItemType<WoBMask>(), 7));
            //Lore
            npcLoot.AddConditionalPerPlayer(() => !ClamitySystem.downedWallOfBronze, ModContent.ItemType<LoreWallOfBronze>(), ui: true, DropHelper.FirstKillText);
            //GFB drop
            /*for (int i = 0; i < 20; i++)
            {
                npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.CopperBar, 1, 1, 10, true);
                npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.TinBar, 1, 1, 10, true);
            }*/
            npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.CopperPlating, 1, 1, 9999, hideLootReport: true);
            npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.TinPlating, 1, 1, 9999, hideLootReport: true);

        }
        public override void OnKill()
        {
            if (!ClamitySystem.downedWallOfBronze)
            {
                //int basePosX = (int)MathHelper.Clamp((int)NPC.Center.X / 16, 500, Main.maxTilesX - 500);
                int basePosX = Main.maxTilesX / 2;
                for (int i = -300; i < 300; i++)
                {
                    for (int j = Main.UnderworldLayer; j < Main.bottomWorld / 16 - 1; j++)
                    {
                        int posX = (int)MathHelper.Clamp(basePosX + i, 0, Main.maxTilesX);
                        //Tile tile = Main.tile[posX, j];

                        if (Main.tile[posX, j].TileType == TileID.Ash || Main.tile[posX, j].TileType == TileID.AshGrass)
                        {
                            Main.tile[posX, j].TileType = (ushort)ModContent.TileType<FrozenAshTile>();
                            WorldGen.SquareTileFrame(posX, j);
                            NetMessage.SendTileSquare(-1, posX, j, 1);
                        }

                        if (Main.tile[posX, j].TileType == TileID.Hellstone)
                        {
                            Main.tile[posX, j].TileType = (ushort)ModContent.TileType<FrozenHellstoneTile>();
                            WorldGen.SquareTileFrame(posX, j);
                            NetMessage.SendTileSquare(-1, posX, j, 1);
                        }
                        if (Main.tile[posX, j].LiquidType == 1 && Main.tile[posX, j].LiquidAmount > 0 && !Main.tile[posX, j].HasTile)
                        {
                            //Main.tile[posX, j].TileType = 162;
                            Main.tile[posX, j].LiquidAmount = 0;
                            WorldGen.PlaceTile(posX, j, 162, forced: true);
                            WorldGen.SquareTileFrame(posX, j);
                            NetMessage.SendTileSquare(-1, posX, j, 1);
                        }
                    }
                }
                CalamityUtils.DisplayLocalizedText("Mods.Clamity.Misc.FrozenHellMessege", new Color?(Color.LightCyan));
            }

            ClamitySystem.downedWallOfBronze = true;
            CalamityNetcode.SyncWorld();
        }
    }
}
