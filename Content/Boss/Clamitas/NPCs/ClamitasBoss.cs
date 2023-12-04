
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs;
using CalamityMod;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Projectiles.Enemy;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Boss;
using static Mono.CompilerServices.SymbolWriter.CodeBlockEntry;
using CalamityMod.Items.Placeables;
using Clamity.Content.Boss.Clamitas.Drop;
using CalamityMod.Items.LoreItems;
using CalamityMod.NPCs.Cryogen;
using Microsoft.CodeAnalysis;
using CalamityMod.Dusts;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;

namespace Clamity.Content.Boss.Clamitas.NPCs
{
    [AutoloadBossHead]
    public class ClamitasBoss : ModNPC
    {
        public static readonly SoundStyle SlamSound = new SoundStyle("CalamityMod/Sounds/Item/ClamImpact");

        private int hitAmount;

        private int attack = -1;

        private bool attackAnim;

        private bool hasBeenHit;

        private bool statChange;

        private bool hide;

        private int flareFrame;

        private int flareFrameCounter;

        private float AttackCounter { get => NPC.Calamity().newAI[0]; set => NPC.Calamity().newAI[0] = value; }

        private float FallAttackType { get => NPC.Calamity().newAI[1]; set => NPC.Calamity().newAI[1] = value; }

        private float AttackTimer { get => NPC.Calamity().newAI[2]; set => NPC.Calamity().newAI[2] = value; }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 12;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0);
            nPCBestiaryDrawModifiers.Scale = 0.4f;
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
            value.Position.Y += 40f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;

            //GiantClam

        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.lavaImmune = true;
            NPC.npcSlots = 5f;
            NPC.damage = 50;
            NPC.width = 160;
            NPC.height = 120;
            NPC.defense = 9999;
            NPC.DR_NERD(0.3f);
            //base.NPC.lifeMax = (Main.hardMode ? 7500 : 1250);
            NPC.lifeMax = 50000;
            NPC.aiStyle = -1;
            AIType = -1;
            //base.NPC.value = (Main.hardMode ? Item.buyPrice(0, 8) : Item.buyPrice(0, 1));
            NPC.value = Item.buyPrice(0, 10);
            NPC.HitSound = SoundID.NPCHit4;
            NPC.knockBackResist = 0f;
            NPC.rarity = 2;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToWater = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<BrimstoneCragsBiome>().Type };
            NPC.boss = true;


            /*if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ClamitasTheme");
            }*/
            if (!Main.dedServ)
            {
                Music = Clamity.mod.GetMusicFromMusicMod("Clamitas") ?? MusicID.Boss3;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[1]
            {
                new FlavorTextBestiaryInfoElement("Mods.Clamity.NPCs.ClamitasBoss.Bestiary")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hitAmount);
            writer.Write(attack);
            writer.Write(attackAnim);
            writer.Write(NPC.dontTakeDamage);
            writer.Write(NPC.chaseable);
            writer.Write(hasBeenHit);
            writer.Write(statChange);
            writer.Write(hide);
            writer.Write(flareFrame);
            writer.Write(flareFrameCounter);
            for (int i = 0; i < 2; i++)
            {
                writer.Write(NPC.Calamity().newAI[i]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hitAmount = reader.ReadInt32();
            attack = reader.ReadInt32();
            attackAnim = reader.ReadBoolean();
            NPC.dontTakeDamage = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
            hasBeenHit = reader.ReadBoolean();
            statChange = reader.ReadBoolean();
            hide = reader.ReadBoolean();
            flareFrame = reader.ReadInt32();
            flareFrameCounter = reader.ReadInt32();
            for (int i = 0; i < 2; i++)
            {
                NPC.Calamity().newAI[i] = reader.ReadSingle();
            }
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();
            if (hitAmount < 5)
            {
                hitAmount++;
                hasBeenHit = true;
            }

            NPC.chaseable = hasBeenHit;

            flareFrameCounter++;
            if (flareFrameCounter >= 5)
            {
                flareFrameCounter = 0;
                flareFrame++;
                if (flareFrame >= 6)
                    flareFrame = 0;
            }
            //hitAmount = 5;
            if (hitAmount != 5)
            {
                return;
            }

            if (Main.netMode != 2 && !Main.player[NPC.target].dead && Main.player[NPC.target].active)
            {
                player.AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatDebuffs.Clamity>(), 2);
            }
            if (Main.player[NPC.target].dead && !Main.player[NPC.target].active)
            {
                NPC.active = false;
            }

            if (!hide)
            {
                Lighting.AddLight(NPC.Center, 0.75f, 0, 0);
            }

            if (!statChange)
            {
                NPC.defense = 35;
                NPC.damage = 200;

                statChange = true;
            }

            if (NPC.ai[0] < 240f)
            {
                NPC.ai[0] += 1f;
                hide = false;
            }
            else if (attack == -1)
            {
                attack = Main.rand.Next(2);
                if (attack == 0)
                {
                    attack = Main.rand.Next(2);
                }
            }
            else if (attack == 0)
            {
                hide = true;
                NPC.defense = 9999;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= 90f)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    hide = false;
                    attack = -1;
                    NPC.defense = 35;
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.Center.X + 5f), (int)NPC.Center.Y, ModContent.NPCType<Clam>());
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Clam>());
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.Center.X - 5f), (int)NPC.Center.Y, ModContent.NPCType<Clam>());
                }
            }
            else if (attack == 1)
            {
                if (NPC.ai[2] == 0f)
                {
                    if (Main.netMode != 1)
                    {
                        NPC.TargetClosest();
                        NPC.ai[2] = 1f;
                        NPC.netUpdate = true;
                        if (FallAttackType != 1 || AttackCounter >= 4)
                            FallAttackType = Main.rand.NextBool(3) ? 1 : 0;
                    }
                }
                else if (NPC.ai[2] == 1f)
                {
                    NPC.damage = 0;
                    NPC.chaseable = false;
                    NPC.dontTakeDamage = true;
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    NPC.alpha += 8;
                    if (NPC.alpha >= 255)
                    {
                        NPC.alpha = 255;
                        NPC.position.X = player.Center.X - NPC.width / 2;
                        NPC.position.Y = player.Center.Y - NPC.height / 2 + player.gfxOffY - 200f;
                        if (FallAttackType == 0)
                        {
                            NPC.position.X = NPC.position.X - 15f;
                            NPC.position.Y = NPC.position.Y - 100f;
                        }
                        else if (FallAttackType == 1)
                        {
                            NPC.position.X = NPC.position.X - 400f * (AttackCounter % 2 == 0 ? 1 : -1);
                            NPC.position.Y = NPC.position.Y - 100f;
                        }
                        NPC.ai[2] = 2f;
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[2] == 2f)
                {
                    if (Main.rand.NextBool(2))
                    {
                        int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<BrimstoneFlame>(), 0f, 0f, 200, default, 1.5f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].velocity *= 0.75f;
                        Main.dust[num].fadeIn = 1.3f;
                        Vector2 vector = new Vector2(Main.rand.Next(-200, 201), Main.rand.Next(-200, 201));
                        vector.Normalize();
                        vector *= Main.rand.Next(100, 200) * 0.04f;
                        Main.dust[num].velocity = vector;
                        vector.Normalize();
                        vector *= 34f;
                        Main.dust[num].position = NPC.Center - vector;
                    }

                    NPC.alpha -= 7;
                    if (NPC.alpha <= 0)
                    {
                        NPC.damage = Main.expertMode ? 200 : 100;

                        NPC.chaseable = true;
                        NPC.dontTakeDamage = false;
                        NPC.alpha = 0;
                        NPC.ai[2] = 3f;
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[2] == 3f)
                {
                    NPC.velocity.Y += 0.8f;
                    attackAnim = true;
                    if (NPC.Center.Y > player.Center.Y - NPC.height / 2 + player.gfxOffY - 15f)
                    {
                        NPC.noTileCollide = false;
                        NPC.ai[2] = 4f;
                        NPC.netUpdate = true;
                    }
                }
                else if (NPC.ai[2] == 4f)
                {
                    if (NPC.velocity.Y == 0f)
                    {
                        if (FallAttackType == 0 || FallAttackType == 1 && AttackCounter >= 4)
                        {
                            NPC.ai[2] = 0f;
                            NPC.ai[0] = 0f;
                            attack = -1;
                            AttackCounter = 0;
                            //NPC.ai[3] = -30;
                        }
                        NPC.netUpdate = true;
                        NPC.noGravity = false;
                        if (FallAttackType == 1)
                        {
                            if (AttackCounter < 4)
                            {
                                attack = 1;
                                NPC.ai[2] = 0f;
                            }
                            AttackCounter++;
                            for (int i = 0; i < 18; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedBy(MathHelper.TwoPi / 18f * i) * 2f, ModContent.ProjectileType<BrimstoneBarrage>(), NPC.damage / 10, 0f);
                            }
                        }
                        /*if (FallAttackType == 0 || (FallAttackType == 1 && AttackCounter >= 4))
                        {
                        }*/
                        SoundEngine.PlaySound(in SlamSound, NPC.position);
                        if (Main.netMode != 2)
                        {
                            for (int i = (int)NPC.position.X - 30; i < (int)NPC.position.X + NPC.width + 60; i += 30)
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    int num2 = Dust.NewDust(new Vector2(NPC.position.X - 30f, NPC.position.Y + NPC.height), NPC.width + 30, 4, DustID.Water, 0f, 0f, 100, default, 1.5f);
                                    Main.dust[num2].velocity *= 0.2f;
                                }

                                int num3 = Gore.NewGore(NPC.GetSource_FromAI(), new Vector2(i - 30, NPC.position.Y + NPC.height - 12f), default, Main.rand.Next(61, 64));
                                Main.gore[num3].velocity *= 0.4f;
                            }
                        }
                    }

                    NPC.velocity.Y += 0.8f;
                }
            }

            /*if (Main.zenithWorld)
            {
                calamityGlobalNPC.newAI[0] += 1f;
                if (Main.netMode != 1)
                {
                    calamityGlobalNPC.newAI[0] += 1f;
                    if (Main.hardMode)
                    {
                        int type = ModContent.ProjectileType<PearlBurst>();
                        int damage = (Main.expertMode ? 28 : 35);
                        float num4 = 180f;
                        float num5 = 3f;
                        if (calamityGlobalNPC.newAI[0] <= 300f)
                        {
                            if (calamityGlobalNPC.newAI[0] % num4 == 0f)
                            {
                                Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X + (float)Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, num5, type, damage, 0f, Main.myPlayer, 1f);
                            }
                        }
                        else if (calamityGlobalNPC.newAI[0] <= 600f && calamityGlobalNPC.newAI[0] > 300f)
                        {
                            if (calamityGlobalNPC.newAI[0] % num4 == 0f)
                            {
                                Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + (float)Main.rand.Next(-1000, 1001), 0f - num5, 0f, type, damage, 0f, Main.myPlayer, 1f);
                            }
                        }
                        else if (calamityGlobalNPC.newAI[0] > 600f && calamityGlobalNPC.newAI[0] % num4 == 0f)
                        {
                            Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X + (float)Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, num5, type, damage, 0f, Main.myPlayer, 1f);
                        }
                    }

                    calamityGlobalNPC.newAI[1] += 1f;
                    float num6 = 60f;
                    if (calamityGlobalNPC.newAI[1] >= num6)
                    {
                        calamityGlobalNPC.newAI[1] = 0f;
                        int type2 = ModContent.ProjectileType<PearlRain>();
                        int damage2 = (Main.expertMode ? 28 : 35);
                        float num7 = 4f;
                        if (calamityGlobalNPC.newAI[0] % (num6 * 6f) == 0f)
                        {
                            float num8 = (Main.rand.NextBool() ? (-1000f) : 1000f);
                            float speedX = ((num8 == -1000f) ? num7 : (0f - num7));
                            Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X + num8, player.position.Y, speedX, 0f, type2, damage2, 0f, Main.myPlayer, 1f);
                        }

                        if (calamityGlobalNPC.newAI[0] < 300f)
                        {
                            Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X + (float)Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, num7, type2, damage2, 0f, Main.myPlayer, 1f);
                        }
                        else if (calamityGlobalNPC.newAI[0] < 600f)
                        {
                            Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + (float)Main.rand.Next(-1000, 1001), 0f - (num7 - 0.5f), 0f, type2, damage2, 0f, Main.myPlayer, 1f);
                            Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + (float)Main.rand.Next(-1000, 1001), num7 - 0.5f, 0f, type2, damage2, 0f, Main.myPlayer, 1f);
                        }
                        else
                        {
                            Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X + (float)Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, num7 - 1f, type2, damage2, 0f, Main.myPlayer, 1f);
                            Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + (float)Main.rand.Next(-1000, 1001), 0f - (num7 - 1f), 0f, type2, damage2, 0f, Main.myPlayer, 1f);
                            Projectile.NewProjectile(base.NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + (float)Main.rand.Next(-1000, 1001), num7 - 1f, 0f, type2, damage2, 0f, Main.myPlayer, 1f);
                        }
                    }
                }
            }*/

            if (NPC.ai[3] < 120f)
            {
                NPC.ai[3] += 1f;
            }
            else
            {
                if (attack == -1)
                {
                    attack = Main.rand.Next(2, 6);
                }
                else if (attack == 2)
                {
                    SoundEngine.PlaySound(in SoundID.Item67, NPC.position);
                    Vector2 vector2 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                    //float num9 = 0.783f;
                    //float num10 = MathF.Atan2(base.NPC.velocity.X, base.NPC.velocity.Y) - (num9 / 2f);
                    //float num11 = num9 / 8f;
                    Vector2 num9 = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    int type3 = ModContent.ProjectileType<BrimstoneHellblast>();
                    int damage3 = NPC.damage / 10;
                    //Vector2 vector3 = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)(base.NPC.height / 2));
                    float num12 = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - NPC.Center.X + Main.rand.Next(-20, 21);
                    float num13 = Main.player[NPC.target].position.Y + Main.player[NPC.target].height * 0.5f - NPC.Center.Y + Main.rand.Next(-20, 21);
                    float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
                    num14 = 5f / num14;
                    num12 *= num14;
                    num13 *= num14;
                    //Projectile.NewProjectile(base.NPC.GetSource_FromAI(), NPC.Center, num9 * 10f, type3, damage3, 0f, Main.myPlayer);
                    for (int k = 0; k < 12; k++)
                    {
                        //float num15 = num10 + num11 * (k + k * k) / 2f + (32f * k);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, num9.RotatedBy(MathHelper.TwoPi / 12f * k) * 8f, type3, damage3, 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, num9.RotatedBy(MathHelper.TwoPi / 12f * k + MathHelper.PiOver4 / 4) * 4f, type3, damage3, 0f, Main.myPlayer);
                        //Projectile.NewProjectile(base.NPC.GetSource_FromAI(), vector2.X, vector2.Y, (float)(Math.Sin(num15) * 3.0), (float)(Math.Cos(num15) * 3.0), type3, damage3, 0f, Main.myPlayer);
                        //Projectile.NewProjectile(base.NPC.GetSource_FromAI(), vector2.X, vector2.Y, (float)((0.0 - Math.Sin(num15)) * 3.0), (float)((0.0 - Math.Cos(num15)) * 3.0), type3, damage3, 0f, Main.myPlayer);
                    }

                    attack = -1;
                    NPC.ai[3] = 0f;
                }
                else if (attack == 3)
                {
                    if (Main.netMode != 1)
                    {
                        SoundEngine.PlaySound(in SoundID.Item68, NPC.position);
                        int damage4 = NPC.damage / 10;
                        float num16 = 1200f;
                        for (int l = 0; l < 15; l++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + num16 * 1.4f, player.Center.Y - 950f, 0f, 4f, ModContent.ProjectileType<BrimstoneHellblast>(), damage4, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + num16 * 1.25f, player.Center.Y - 1250f, 0f, 2.5f, ModContent.ProjectileType<BrimstoneHellblast>(), damage4, 0f, Main.myPlayer);
                            num16 -= 150f;
                        }
                    }

                    attack = -1;
                    NPC.ai[3] = 0f;
                }
                else if (attack == 4)
                {
                    AttackTimer++;
                    if (AttackTimer % 4 == 0)
                    {
                        if (AttackTimer % 12 == 0)
                            SoundEngine.PlaySound(in SoundID.Item21, NPC.position);
                        AttackCounter++;
                        Vector2 num9 = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                        int type3 = ModContent.ProjectileType<BrimstoneHellblast>();
                        int damage3 = NPC.damage / 10;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, num9 * 8f, type3, damage3, 0f, Main.myPlayer);
                    }

                    if (AttackCounter >= 15)
                    {
                        attack = -1;
                        NPC.ai[3] = 0f;
                        AttackCounter = 0;
                        AttackTimer = 0;
                    }
                }
                else if (attack == 5)
                {
                    AttackTimer++;
                    if (AttackTimer % 20 == 0)
                    {
                        SoundEngine.PlaySound(in SoundID.Item21, NPC.position);
                        AttackCounter++;
                        Vector2 num9 = (Main.player[NPC.target].Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                        int type3 = ModContent.ProjectileType<BrimstoneHellblast>();
                        int damage3 = NPC.damage / 10;
                        for (int i = 0; i < 5; i++)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, num9 * 8f + Main.rand.NextVector2CircularEdge(1f, 1f), type3, damage3, 0f, Main.myPlayer);
                    }

                    if (AttackCounter >= 10)
                    {
                        attack = -1;
                        NPC.ai[3] = 0f;
                        AttackCounter = 0;
                        AttackTimer = 0;
                    }
                }
                else if (attack == 6)
                {

                }
            }
        }

        public override bool CheckActive()
        {
            return Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 5600f;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
            {
                return hasBeenHit;
            }

            return null;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter > (attackAnim ? 2.0 : 5.0))
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y = NPC.frame.Y + frameHeight;
            }

            if ((hitAmount < 5 || hide) && !NPC.IsABestiaryIconDummy)
            {
                NPC.frame.Y = frameHeight * 11;
            }
            else if (attackAnim)
            {
                if (NPC.frame.Y < frameHeight * 3)
                {
                    NPC.frame.Y = frameHeight * 3;
                }

                if (NPC.frame.Y > frameHeight * 10)
                {
                    hide = true;
                    attackAnim = false;
                }
            }
            else if (NPC.frame.Y > frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }

        /*public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Calamity().ZoneSunkenSea && spawnInfo.Water && DownedBossSystem.downedDesertScourge && !NPC.AnyNPCs(ModContent.NPCType<GiantClam>()))
            {
                return SpawnCondition.CaveJellyfish.Chance * 0.24f;
            }

            return 0f;
        }*/

        /*public override void ModifyTypeName(ref string typeName)
        {
            if (Main.zenithWorld)
            {
                if (Main.hardMode)
                {
                    typeName = CalamityUtils.GetTextValue("NPCs.SupremeClamitas");
                }
                else
                {
                    typeName = CalamityUtils.GetTextValue("NPCs.Clamitas");
                }
            }
        }*/

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 37, hit.HitDirection, -1f);
            }

            if (NPC.life <= 0)
            {
                for (int j = 0; j < 50; j++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 37, hit.HitDirection, -1f);
                }

                if (Main.netMode != 2)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModLoader.GetMod("CalamityMod").Find<ModGore>("GiantClam1").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModLoader.GetMod("CalamityMod").Find<ModGore>("GiantClam2").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModLoader.GetMod("CalamityMod").Find<ModGore>("GiantClam3").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModLoader.GetMod("CalamityMod").Find<ModGore>("GiantClam4").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModLoader.GetMod("CalamityMod").Find<ModGore>("GiantClam5").Type);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "_Extra").Value, NPC.Center - Vector2.UnitY * 20f * NPC.scale - screenPos, new Rectangle(0, flareFrame * 174, 116, 174), NPC.GetAlpha(Color.White), NPC.rotation, new Vector2(116, 174) * 0.5f, NPC.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, SpriteEffects.None);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D value2 = ModContent.Request<Texture2D>(Texture + "Glow").Value;
            SpriteEffects effects = SpriteEffects.None;
            Vector2 vector = new Vector2(NPC.Center.X, NPC.Center.Y);
            Vector2 vector2 = new Vector2(value.Width / 2, value.Height / Main.npcFrameCount[NPC.type] / 2);
            Vector2 position = vector - screenPos;
            position -= new Vector2(value2.Width, value2.Height / Main.npcFrameCount[NPC.type]) * 1f / 2f;
            position += vector2 * 1f + new Vector2(0f, 4f + NPC.gfxOffY);
            Color color = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.Red);
            Main.EntitySpriteDraw(value2, position, NPC.frame, color, NPC.rotation, vector2, NPC.scale, effects);
        }

        public override void OnKill()
        {
            /*if (NPC.FindFirstNPC(ModContent.NPCType<SEAHOE>()) == -1 && Main.netMode != 1)
            {
                NPC.NewNPC(base.NPC.GetSource_Death(), (int)base.NPC.Center.X, (int)base.NPC.Center.Y, ModContent.NPCType<SEAHOE>());
            }*/

            //DownedBossSystem.downedCLAM = true;
            //DownedBossSystem.downedCLAMHardMode = Main.hardMode || DownedBossSystem.downedCLAMHardMode;

            ClamitySystem.downedClamitas = true;
            CalamityNetcode.SyncWorld();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //LeadingConditionRule mainRule = npcLoot.DefineConditionalDropSet(DropHelper.Hardmode());
            npcLoot.Add(ModContent.ItemType<BrimstoneSlag>(), 1, 30, 40);
            npcLoot.Add(ModContent.ItemType<HuskOfCalamity>(), 1, 25, 30);
            npcLoot.Add(ModContent.ItemType<ClamitousPearl>(), 1, 2, 4);
            npcLoot.AddConditionalPerPlayer(() => !ClamitySystem.downedClamitas, ModContent.ItemType<LoreWhat>(), ui: true, DropHelper.FirstKillText);
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<ClamitasRelic>());
            npcLoot.Add(ModContent.ItemType<ThankYouPainting>(), 100);

            /*int[] itemIDs = new int[4]
            {
                ModContent.ItemType<ClamCrusher>(),
                ModContent.ItemType<ClamorRifle>(),
                ModContent.ItemType<Poseidon>(),
                ModContent.ItemType<ShellfishStaff>()
            };
            mainRule.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, itemIDs));
            npcLoot.Add(4412, 2);
            npcLoot.Add(4413, 4);
            npcLoot.Add(4414, 10);
            npcLoot.Add(ModContent.ItemType<GiantPearl>(), 3);
            npcLoot.Add(ModContent.ItemType<AmidiasPendant>(), 3);
            npcLoot.Add(ModContent.ItemType<GiantClamTrophy>(), 10);
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<GiantClamRelic>());
            */
        }
    }
}