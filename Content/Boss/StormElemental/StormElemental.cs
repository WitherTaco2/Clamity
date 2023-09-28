using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Projectiles.Enemy;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Terraria.ModLoader;
using Terraria;

namespace Clamity.Content.Boss.StormElemental
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

        public Player Target => Main.player[base.NPC.target];

        public AttackState CurrentAttackState
        {
            get
            {
                return (AttackState)base.NPC.ai[0];
            }
            set
            {
                if (base.NPC.ai[0] != (float)value)
                {
                    base.NPC.ai[0] = (float)value;
                    base.NPC.netUpdate = true;
                }
            }
        }

        public bool Phase2 => (float)base.NPC.life < (float)base.NPC.lifeMax * 0.5f;

        public ref float AttackTimer => ref base.NPC.ai[1];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[base.NPC.type] = 8;
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0);
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
            base.NPC.npcSlots = 3f;
            base.NPC.damage = 38;
            base.NPC.width = 80;
            base.NPC.height = 140;
            base.NPC.defense = 18;
            base.NPC.DR_NERD(0.05f);
            base.NPC.lifeMax = 6000;
            base.NPC.knockBackResist = 0.05f;
            base.NPC.value = Item.buyPrice(0, 1, 50);
            base.NPC.HitSound = SoundID.NPCHit23;
            base.NPC.DeathSound = SoundID.NPCDeath39;
            base.NPC.noGravity = true;
            base.NPC.noTileCollide = true;
            base.NPC.rarity = 2;
            base.Banner = base.NPC.type;
            base.BannerItem = ModContent.ItemType<CloudElementalBanner>();
            base.NPC.Calamity().VulnerableToCold = true;
            base.NPC.Calamity().VulnerableToSickness = false;
            base.NPC.Calamity().VulnerableToElectricity = false;
            base.NPC.Calamity().VulnerableToWater = false;
            base.NPC.Calamity().VulnerableToHeat = false;
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
            Lighting.AddLight((int)(base.NPC.Center.X / 16f), (int)(base.NPC.Center.Y / 16f), 0.375f, 0.5f, 0.625f);
            if (Target.dead || !Target.active || !Main.player.IndexInRange(base.NPC.target))
            {
                base.NPC.TargetClosest();
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
            float num = (float)base.NPC.life / (float)base.NPC.lifeMax;
            int num2 = (int)MathHelper.Lerp(330f, 180f, 1f - num);
            float moveSpeed = MathHelper.Lerp(0.2f, 0.425f, 1f - num);
            Vector2 vector = new Vector2(8.5f, 4.5f);
            if (Main.rand.NextBool(8) && !Main.dedServ)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(base.NPC.position, base.NPC.width, base.NPC.height, 16);
                    dust.velocity = Main.rand.NextVector2CircularEdge(4f, 4f);
                    dust.velocity.Y /= 3f;
                    dust.scale = Main.rand.NextFloat(1.15f, 1.35f);
                    dust.noGravity = true;
                }
            }

            if (AttackTimer < (float)(num2 - 30))
            {
                Vector2 desiredVelocity = base.NPC.SafeDirectionTo(Target.Center) * vector;
                if (Math.Abs(base.NPC.Center.X - Target.Center.X) > 30f)
                {
                    base.NPC.SimpleFlyMovement(desiredVelocity, moveSpeed);
                    base.NPC.spriteDirection = (base.NPC.velocity.X > 0f).ToDirectionInt();
                }
            }
            else
            {
                base.NPC.velocity *= 0.95f;
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
                base.NPC.netUpdate = true;
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
                base.NPC.Opacity = MathHelper.Lerp(1f, 0f, lerpValue);
                if (Main.rand.NextFloat() < num3 && !Main.dedServ)
                {
                    Dust.NewDustDirect(base.NPC.position, base.NPC.width, base.NPC.height, 16);
                    if (Main.rand.NextBool(15) && Main.netMode != 2)
                    {
                        int type = Utils.SelectRandom<int>(Main.rand, 825, 826, 827);
                        Vector2 velocity = Main.rand.NextVector2CircularEdge(6f, 6f);
                        Gore.NewGorePerfect(base.NPC.GetSource_FromAI(), base.NPC.Center + Main.rand.NextVector2Circular(40f, 40f), velocity, type);
                    }
                }
            }

            if (AttackTimer == (float)num)
            {
                float num4 = 420f;
                base.NPC.Center = Target.Center + Main.rand.NextVector2CircularEdge(num4, num4);
                base.NPC.netUpdate = true;
            }

            if (AttackTimer > (float)num && AttackTimer <= (float)(num + num2))
            {
                float lerpValue2 = Utils.GetLerpValue(num, num + num2, AttackTimer, clamped: true);
                float num5 = MathHelper.Clamp(lerpValue2 + 0.6f, 0.5f, 1f);
                base.NPC.Opacity = lerpValue2;
                if (Main.rand.NextFloat() < num5 && !Main.dedServ)
                {
                    Dust.NewDustDirect(base.NPC.position, base.NPC.width, base.NPC.height, 16);
                }
            }

            if (AttackTimer >= (float)(num + num2))
            {
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                base.NPC.netUpdate = true;
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

            base.NPC.velocity *= 0.96f;
            if (AttackTimer > (float)num && Main.netMode != 1 && (AttackTimer - (float)num) % (float)num2 == (float)(num2 - 1))
            {
                int type = ModContent.ProjectileType<LightningCloud>();
                float num5 = (AttackTimer - (float)num) / (float)num2 * 50f;
                Vector2 position = base.NPC.Top + new Vector2(num5, -36f);
                Projectile.NewProjectileDirect(base.NPC.GetSource_FromAI(), position, Vector2.Zero, type, num4, 0f, Main.myPlayer);
                position = base.NPC.Top + new Vector2(0f - num5, -36f);
                Projectile.NewProjectileDirect(base.NPC.GetSource_FromAI(), position, Vector2.Zero, type, num4, 0f, Main.myPlayer);
            }

            if (AttackTimer >= (float)(num + num2 * num3))
            {
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                base.NPC.netUpdate = true;
            }
        }

        public void DoBehavior_TornadoSummon()
        {
            int num = 60;
            int num2 = (Phase2 ? 8 : 5);
            base.NPC.velocity *= 0.96f;
            if (Main.netMode != 1 && AttackTimer == (float)num)
            {
                int type = ModContent.ProjectileType<StormMarkHostile>();
                for (int i = 0; i < num2; i++)
                {
                    float f = MathF.PI * 2f / (float)num2 * (float)i;
                    Vector2 position = Target.Center + f.ToRotationVector2() * 620f;
                    Projectile.NewProjectile(base.NPC.GetSource_FromAI(), position, Vector2.Zero, type, 0, 0f, Main.myPlayer);
                }
            }

            if (AttackTimer >= (float)num + 180f)
            {
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                base.NPC.netUpdate = true;
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
                base.NPC.velocity *= 0.92f;
            }
            else if ((AttackTimer - (float)num) % (float)num3 == (float)(num3 - 1))
            {
                Point p = (base.NPC.Center + base.NPC.ai[2].ToRotationVector2() * 300f).ToPoint();
                if (Main.netMode != 1 && NPC.CountNPCS(250) < num2 && !CalamityUtils.ParanoidTileRetrieval(p.X, p.Y).HasTile)
                {
                    NPC.NewNPC(base.NPC.GetSource_FromAI(), p.X, p.Y, 250);
                }

                if (!Main.dedServ)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDustDirect(p.ToVector2(), -20, 20, 16);
                    }

                    SoundEngine.PlaySound(in SoundID.Item122, p.ToVector2());
                }

                base.NPC.ai[2] += MathF.PI * 2f / (float)num2;
            }

            if (AttackTimer >= (float)(num + num3 * num2))
            {
                base.NPC.ai[2] = 0f;
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                base.NPC.netUpdate = true;
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
                base.NPC.velocity *= 0.92f;
            }

            if (AttackTimer % (float)(num2 + num3) == (float)num3)
            {
                base.NPC.damage = base.NPC.defDamage * 2;
                base.NPC.velocity = base.NPC.SafeDirectionTo(Target.Center) * num4;
                base.NPC.spriteDirection = (base.NPC.velocity.X > 0f).ToDirectionInt();
                base.NPC.netUpdate = true;
            }

            if (AttackTimer >= (float)((num2 + num3) * num))
            {
                base.NPC.damage = base.NPC.defDamage;
                CurrentAttackState = AttackState.Hover;
                AttackTimer = 0f;
                base.NPC.netUpdate = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D value = ModContent.Request<Texture2D>("CalamityMod/NPCs/NormalNPCs/ThiccWaifuAttack").Value;
            SpriteEffects effects = ((base.NPC.spriteDirection != -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            if (CurrentAttackState != 0)
            {
                Main.EntitySpriteDraw(value, base.NPC.Center - screenPos, base.NPC.frame, base.NPC.GetAlpha(drawColor), base.NPC.rotation, base.NPC.frame.Size() * 0.5f, base.NPC.scale, effects);
            }
            else
            {
                value = ModContent.Request<Texture2D>(Texture).Value;
                Main.EntitySpriteDraw(value, base.NPC.Center - screenPos, base.NPC.frame, base.NPC.GetAlpha(drawColor), base.NPC.rotation, base.NPC.frame.Size() * 0.5f, base.NPC.scale, effects);
            }

            if (Main.zenithWorld)
            {
                Texture2D value2 = ModContent.Request<Texture2D>("CalamityMod/Particles/WulfrumHat").Value;
                SpriteEffects effects2 = ((base.NPC.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                int num = ((base.NPC.direction == 1) ? 70 : 20);
                Vector2 vector = new Vector2(num, -10f);
                Main.EntitySpriteDraw(value2, base.NPC.Center - screenPos + vector, null, base.NPC.GetAlpha(Color.LightBlue), base.NPC.rotation, base.NPC.frame.Size() * 0.5f, base.NPC.scale, effects2);
            }

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            base.NPC.frameCounter = base.NPC.frameCounter + (double)MathHelper.Max(base.NPC.velocity.Length() * 0.1f, 0.6f) + 1.0;
            if (base.NPC.frameCounter >= ((CurrentAttackState != 0) ? 16.0 : 8.0))
            {
                base.NPC.frame.Y += frameHeight;
                base.NPC.frameCounter = 0.0;
            }

            if (base.NPC.frame.Y >= frameHeight * 8)
            {
                base.NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !Main.hardMode || !Main.raining || !spawnInfo.Player.ZoneSkyHeight)
            {
                return 0f;
            }

            if (NPC.AnyNPCs(base.NPC.type))
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
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 16, hit.HitDirection, -1f);
            }

            if (base.NPC.life <= 0)
            {
                for (int j = 0; j < 50; j++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 16, hit.HitDirection, -1f);
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
