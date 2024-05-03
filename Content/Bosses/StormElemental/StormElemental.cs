using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Clamity.Content.Bosses.StormElemental
{
    public class StormElemental : ModNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public enum AttackState
        {
            Hover,
            CloudTeleport,
            LightningSummon,
            TornadoSummon,
            LightningBladeSlice,
            NimbusSummon
        }

        public Player Target => Main.player[NPC.target];

        public AttackState CurrentAttackState
        {
            get
            {
                return (AttackState)NPC.ai[0];
            }
            set
            {
                if (NPC.ai[0] != (float)value)
                {
                    NPC.ai[0] = (float)value;
                    NPC.netUpdate = true;
                }
            }
        }

        public bool Phase2 => (float)NPC.life < (float)NPC.lifeMax * 0.5f;

        public ref float AttackTimer => ref NPC.ai[1];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers();
            nPCBestiaryDrawModifiers.Position = new Vector2(28f, 20f);
            nPCBestiaryDrawModifiers.Scale = 0.65f;
            nPCBestiaryDrawModifiers.PortraitScale = 0.65f;
            nPCBestiaryDrawModifiers.PortraitPositionXOverride = 10f;
            nPCBestiaryDrawModifiers.PortraitPositionYOverride = 2f;
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
            NPCID.Sets.NPCBestiaryDrawOffset[base.Type] = value;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 3f;
            NPC.damage = 38;
            NPC.width = 80;
            NPC.height = 140;
            NPC.defense = 18;
            NPC.DR_NERD(0.05f);
            NPC.lifeMax = 6000;
            NPC.knockBackResist = 0.05f;
            NPC.value = Item.buyPrice(0, 1, 50);
            NPC.HitSound = SoundID.NPCHit23;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.rarity = 2;
            base.Banner = NPC.type;
            base.BannerItem = ModContent.ItemType<CloudElementalBanner>();
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToHeat = false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[3]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.ThiccWaifu")
            });
        }

        public override void AI()
        {
            Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.375f, 0.5f, 0.625f);
            if (Target.dead || !Target.active || !Main.player.IndexInRange(NPC.target))
            {
                NPC.TargetClosest();
            }

            switch (CurrentAttackState)
            {
                case AttackState.Hover:
                    DoBehavior_Hover();
                    break;
                case AttackState.CloudTeleport:
                    DoBehavior_CloudTeleport();
                    break;
                case AttackState.LightningSummon:
                    DoBehavior_LightningSummon();
                    break;
                case AttackState.TornadoSummon:
                    DoBehavior_TornadoSummon();
                    break;
                case AttackState.LightningBladeSlice:
                    DoBehavior_LightningBladeSlice();
                    break;
                case AttackState.NimbusSummon:
                    DoBehavior_NimbusSummon();
                    break;
            }

            AttackTimer += 1f;
        }

        public void DoBehavior_Hover()
        {
            float num = (float)NPC.life / (float)NPC.lifeMax;
            int num2 = (int)MathHelper.Lerp(330f, 180f, 1f - num);
            float moveSpeed = MathHelper.Lerp(0.2f, 0.425f, 1f - num);
            Vector2 vector = new Vector2(8.5f, 4.5f);
            if (Main.rand.NextBool(8) && !Main.dedServ)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud);
                    dust.velocity = Main.rand.NextVector2CircularEdge(4f, 4f);
                    dust.velocity.Y /= 3f;
                    dust.scale = Main.rand.NextFloat(1.15f, 1.35f);
                    dust.noGravity = true;
                }
            }

            if (AttackTimer < (float)(num2 - 30))
            {
                Vector2 desiredVelocity = NPC.SafeDirectionTo(Target.Center) * vector;
                if (Math.Abs(NPC.Center.X - Target.Center.X) > 30f)
                {
                    NPC.SimpleFlyMovement(desiredVelocity, moveSpeed);
                    NPC.spriteDirection = (NPC.velocity.X > 0f).ToDirectionInt();
                }
            }
            else
            {
                NPC.velocity *= 0.95f;
            }

            if (AttackTimer >= (float)num2)
            {
                List<AttackState> list = new List<AttackState>
                {
                    AttackState.NimbusSummon,
                    AttackState.TornadoSummon
                };
                if (Phase2)
                {
                    list.Add(AttackState.LightningSummon);
                    list.Add(AttackState.LightningBladeSlice);
                }

                if (NPC.CountNPCS(250) >= 10)
                {
                    list.Remove(AttackState.NimbusSummon);
                }

                if (Main.rand.NextBool(3))
                {
                    CurrentAttackState = AttackState.CloudTeleport;
                }
                else
                {
                    CurrentAttackState = Main.rand.Next(list);
                }

                AttackTimer = 0f;
                NPC.netUpdate = true;
            }
        }

        public void DoBehavior_CloudTeleport()
        {
            int num = 75;
            int num2 = 60;
            if (AttackTimer <= (float)num)
            {
                float lerpValue = Utils.GetLerpValue(0f, num, AttackTimer, clamped: true);
                float num3 = MathHelper.Clamp(lerpValue + 0.6f, 0.5f, 1f);
                NPC.Opacity = MathHelper.Lerp(1f, 0f, lerpValue);
                if (Main.rand.NextFloat() < num3 && !Main.dedServ)
                {
                    Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud);
                    if (Main.rand.NextBool(15) && Main.netMode != NetmodeID.Server)
                    {
                        int type = Utils.SelectRandom<int>(Main.rand, 825, 826, 827);
                        Vector2 velocity = Main.rand.NextVector2CircularEdge(6f, 6f);
                        Gore.NewGorePerfect(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2Circular(40f, 40f), velocity, type);
                    }
                }
            }

            if (AttackTimer == (float)num)
            {
                float num4 = 420f;
                NPC.Center = Target.Center + Main.rand.NextVector2CircularEdge(num4, num4);
                NPC.netUpdate = true;
            }

            if (AttackTimer > (float)num && AttackTimer <= (float)(num + num2))
            {
                float lerpValue2 = Utils.GetLerpValue(num, num + num2, AttackTimer, clamped: true);
                float num5 = MathHelper.Clamp(lerpValue2 + 0.6f, 0.5f, 1f);
                NPC.Opacity = lerpValue2;
                if (Main.rand.NextFloat() < num5 && !Main.dedServ)
                {
                    Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud);
                }
            }

            if (AttackTimer >= (float)(num + num2))
            {
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                NPC.netUpdate = true;
            }
        }

        public void DoBehavior_LightningSummon()
        {
            int num = 60;
            int num2 = 30;
            int num3 = 5;
            int num4 = (Main.expertMode ? 23 : 36);
            if (Phase2)
            {
                num2 -= 5;
                num3 += 2;
                num4 += 2;
            }

            NPC.velocity *= 0.96f;
            if (AttackTimer > (float)num && Main.netMode != NetmodeID.MultiplayerClient && (AttackTimer - (float)num) % (float)num2 == (float)(num2 - 1))
            {
                int type = ModContent.ProjectileType<LightningCloud>();
                float num5 = (AttackTimer - (float)num) / (float)num2 * 50f;
                Vector2 position = NPC.Top + new Vector2(num5, -36f);
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), position, Vector2.Zero, type, num4, 0f, Main.myPlayer);
                position = NPC.Top + new Vector2(0f - num5, -36f);
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), position, Vector2.Zero, type, num4, 0f, Main.myPlayer);
            }

            if (AttackTimer >= (float)(num + num2 * num3))
            {
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                NPC.netUpdate = true;
            }
        }

        public void DoBehavior_TornadoSummon()
        {
            int num = 60;
            int num2 = (Phase2 ? 8 : 5);
            NPC.velocity *= 0.96f;
            if (Main.netMode != NetmodeID.MultiplayerClient && AttackTimer == (float)num)
            {
                int type = ModContent.ProjectileType<StormMarkHostile>();
                for (int i = 0; i < num2; i++)
                {
                    float f = MathF.PI * 2f / (float)num2 * (float)i;
                    Vector2 position = Target.Center + f.ToRotationVector2() * 620f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), position, Vector2.Zero, type, 0, 0f, Main.myPlayer);
                }
            }

            if (AttackTimer >= (float)num + 180f)
            {
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                NPC.netUpdate = true;
            }
        }

        public void DoBehavior_NimbusSummon()
        {
            int num = 45;
            int num2 = 5;
            int num3 = 50;
            if (Phase2)
            {
                num2++;
                num3 -= 10;
            }

            if (AttackTimer < (float)num)
            {
                NPC.velocity *= 0.92f;
            }
            else if ((AttackTimer - (float)num) % (float)num3 == (float)(num3 - 1))
            {
                Point p = (NPC.Center + NPC.ai[2].ToRotationVector2() * 300f).ToPoint();
                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.CountNPCS(250) < num2 && !CalamityUtils.ParanoidTileRetrieval(p.X, p.Y).HasTile)
                {
                    NPC.NewNPC(NPC.GetSource_FromAI(), p.X, p.Y, 250);
                }

                if (!Main.dedServ)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDustDirect(p.ToVector2(), -20, 20, DustID.Cloud);
                    }

                    SoundEngine.PlaySound(in SoundID.Item122, p.ToVector2());
                }

                NPC.ai[2] += MathF.PI * 2f / (float)num2;
            }

            if (AttackTimer >= (float)(num + num3 * num2))
            {
                NPC.ai[2] = 0f;
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                NPC.netUpdate = true;
            }
        }

        public void DoBehavior_LightningBladeSlice()
        {
            int num = 4;
            int num2 = 45;
            int num3 = 15;
            float num4 = 22f;
            if (AttackTimer % (float)(num2 + num3) < (float)num3)
            {
                NPC.velocity *= 0.92f;
            }

            if (AttackTimer % (float)(num2 + num3) == (float)num3)
            {
                NPC.damage = NPC.defDamage * 2;
                NPC.velocity = NPC.SafeDirectionTo(Target.Center) * num4;
                NPC.spriteDirection = (NPC.velocity.X > 0f).ToDirectionInt();
                NPC.netUpdate = true;
            }

            if (AttackTimer >= (float)((num2 + num3) * num))
            {
                NPC.damage = NPC.defDamage;
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                NPC.netUpdate = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D value = ModContent.Request<Texture2D>("CalamityMod/NPCs/NormalNPCs/ThiccWaifuAttack").Value;
            SpriteEffects effects = ((NPC.spriteDirection != -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            if (CurrentAttackState != 0)
            {
                Main.EntitySpriteDraw(value, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, effects);
            }
            else
            {
                value = ModContent.Request<Texture2D>(Texture).Value;
                Main.EntitySpriteDraw(value, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, effects);
            }

            if (Main.zenithWorld)
            {
                Texture2D value2 = ModContent.Request<Texture2D>("CalamityMod/Particles/WulfrumHat").Value;
                SpriteEffects effects2 = ((NPC.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                int num = ((NPC.direction == 1) ? 70 : 20);
                Vector2 vector = new Vector2(num, -10f);
                Main.EntitySpriteDraw(value2, NPC.Center - screenPos + vector, null, NPC.GetAlpha(Color.LightBlue), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, effects2);
            }

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter = NPC.frameCounter + (double)MathHelper.Max(NPC.velocity.Length() * 0.1f, 0.6f) + 1.0;
            if (NPC.frameCounter >= ((CurrentAttackState != 0) ? 16.0 : 8.0))
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0.0;
            }

            if (NPC.frame.Y >= frameHeight * 8)
            {
                NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !Main.hardMode || !Main.raining || !spawnInfo.Player.ZoneSkyHeight)
            {
                return 0f;
            }

            if (NPC.AnyNPCs(NPC.type))
            {
                return 0f;
            }

            return SpawnCondition.Sky.Chance * 0.1f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
            {
                target.AddBuff(144, 180);
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f);
            }

            if (NPC.life <= 0)
            {
                for (int j = 0; j < 50; j++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<EssenceofSunlight>(), 1, 8, 10, 10, 12));
            npcLoot.Add(ModContent.ItemType<EyeoftheStorm>(), 3);
            npcLoot.Add(ModContent.ItemType<StormSaber>(), 5);
        }
    }
}
