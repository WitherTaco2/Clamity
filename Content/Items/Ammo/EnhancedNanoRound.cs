using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Ammo
{
    public class EnhancedNanoRound : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Ammo";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 5.5f;
            Item.value = Item.sellPrice(copper: 16);
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<EnhancedNanoRoundProj>();
            Item.shootSpeed = 8f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            CreateRecipe(250).
                AddIngredient(ItemID.NanoBullet, 250).
                AddIngredient<EssenceofEleum>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class EnhancedNanoRoundProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged.Ammo";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 3;
            AIType = ProjectileID.Bullet;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.25f, 0.25f);

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, 229, 0f, 0f, 0, default, 0.5f);
                    Main.dust[dust].alpha = Projectile.alpha;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesFromEdge(Projectile, 0, lightColor);
            return false;
        }

        // This projectile is always fullbright.
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Confused, 300);
            if (target.life <= 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Nanomachine>(), (int)(Projectile.damage * 0.3), 0f, Projectile.owner, 0f, 0f);
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item93, Projectile.position);
            int rando = Main.rand.Next(5, 10);
            for (int i = 0; i < rando; i++)
            {
                int dusty = Dust.NewDust(Projectile.Center - Projectile.velocity / 2f, 0, 0, 229, 0f, 0f, 100, default, 2f);
                Main.dust[dusty].velocity *= 2f;
                Main.dust[dusty].noGravity = true;
            }
        }
    }
    public class Nanomachine : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged.Ammo";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[1] == 0)
                Projectile.localAI[1] = Main.rand.Next(1, 3);
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Ranged/Nanomachine").Value;
            if (Projectile.localAI[1] == 2)
                texture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Ranged/Nanomachine2").Value;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1, texture);
            return false;
        }

        public override void AI()
        {
            //Animation
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 2)
            {
                Projectile.frame = 0;
            }

            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) + MathHelper.ToRadians(90) * Projectile.direction;


            Vector2 dspeed = -Projectile.velocity * Main.rand.NextFloat(0.5f, 0.8f);
            float x2 = Projectile.Center.X - Projectile.velocity.X / 10f;
            float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f;
            int nanoDust = Dust.NewDust(new Vector2(x2, y2), 1, 1, 229, 0f, 0f, 0, default, 0.8f);
            Main.dust[nanoDust].alpha = Projectile.alpha;
            Main.dust[nanoDust].position.X = x2;
            Main.dust[nanoDust].position.Y = y2;
            Main.dust[nanoDust].velocity = dspeed;
            Main.dust[nanoDust].noGravity = true;

            float velocityAdjust = (float)Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
            float aiTracker = Projectile.localAI[0];
            if (aiTracker == 0f)
            {
                Projectile.localAI[0] = velocityAdjust;
                aiTracker = velocityAdjust;
            }
            float projX = Projectile.position.X;
            float projY = Projectile.position.Y;
            float constant = 800f;
            bool canHit = false;
            int targetID = 0;
            if (Projectile.ai[1] == 0f)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].CanBeChasedBy(Projectile, false) && (Projectile.ai[1] == 0f || Projectile.ai[1] == (float)(i + 1)))
                    {
                        float targetX = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
                        float targetY = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
                        float targetDist = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - targetX) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - targetY);
                        if (targetDist < constant && Collision.CanHit(new Vector2(Projectile.position.X + (float)(Projectile.width / 2), Projectile.position.Y + (float)(Projectile.height / 2)), 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                        {
                            constant = targetDist;
                            projX = targetX;
                            projY = targetY;
                            canHit = true;
                            targetID = i;
                        }
                    }
                }
                if (canHit)
                {
                    Projectile.ai[1] = (float)(targetID + 1);
                }
                canHit = false;
            }
            if (Projectile.ai[1] > 0f)
            {
                int secondTargetID = (int)(Projectile.ai[1] - 1f);
                if (Main.npc[secondTargetID].active && Main.npc[secondTargetID].CanBeChasedBy(Projectile, true) && !Main.npc[secondTargetID].dontTakeDamage)
                {
                    float secondTargetX = Main.npc[secondTargetID].position.X + (float)(Main.npc[secondTargetID].width / 2);
                    float secondTargetY = Main.npc[secondTargetID].position.Y + (float)(Main.npc[secondTargetID].height / 2);
                    float secondTargetDist = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - secondTargetX) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - secondTargetY);
                    if (secondTargetDist < 1000f)
                    {
                        canHit = true;
                        projX = Main.npc[secondTargetID].position.X + (float)(Main.npc[secondTargetID].width / 2);
                        projY = Main.npc[secondTargetID].position.Y + (float)(Main.npc[secondTargetID].height / 2);
                    }
                }
                else
                {
                    Projectile.ai[1] = 0f;
                }
            }
            if (!Projectile.friendly)
            {
                canHit = false;
            }
            if (canHit)
            {
                Vector2 projPos = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float projXSpeed = projX - projPos.X;
                float projYSpeed = projY - projPos.Y;
                float projSpeedAdjust = (float)Math.Sqrt((double)(projXSpeed * projXSpeed + projYSpeed * projYSpeed));
                projSpeedAdjust = aiTracker / projSpeedAdjust;
                projXSpeed *= projSpeedAdjust;
                projYSpeed *= projSpeedAdjust;
                int projVelAdjust = 12;
                Projectile.velocity.X = (Projectile.velocity.X * (float)(projVelAdjust - 1) + projXSpeed) / (float)projVelAdjust;
                Projectile.velocity.Y = (Projectile.velocity.Y * (float)(projVelAdjust - 1) + projYSpeed) / (float)projVelAdjust;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Confused, 180);
        }
    }
}
