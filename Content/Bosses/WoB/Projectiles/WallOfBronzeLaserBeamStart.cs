using Clamity.Content.Bosses.WoB.NPCs;
using Terraria.Enums;

namespace Clamity.Content.Bosses.WoB.Projectiles
{
    public class WallOfBronzeLaserBeamStart : ModProjectile
    {
        private const int maxFrames = 5;
        private int frameDrawn;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }
        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
            Projectile.Calamity().DealsDefenseDamage = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(frameDrawn);
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            frameDrawn = reader.ReadInt32();
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public ref float WoBLaser => ref Projectile.ai[1];
        public Terraria.NPC GetWoBLaser() => Main.npc[(int)Projectile.ai[1]];
        public override void AI()
        {
            Vector2? vector = null;
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }

            /*bool flag = false;
            if (CalamityGlobalNPC.draedonExoMechPrime != -1 && Main.npc[CalamityGlobalNPC.draedonExoMechPrime].active)
            {
                flag = Main.npc[CalamityGlobalNPC.draedonExoMechPrime].Calamity().newAI[1] == 2f || Main.npc[CalamityGlobalNPC.draedonExoMechPrime].Calamity().newAI[0] == 1f;
            }*/

            if (GetWoBLaser().active && GetWoBLaser().type == ModContent.NPCType<WallOfBronzeLaser>()/* && !flag*/)
            {
                //float num = 84f;
                //float num2 = 16f;
                float num = 0;
                float num2 = 0;
                Vector2 vector2 = GetWoBLaser().Calamity().newAI[3] == 0f ? new Vector2(GetWoBLaser().Center.X - num2 * GetWoBLaser().direction, GetWoBLaser().Center.Y + num)
                                                                             : new Vector2(GetWoBLaser().Center.X + num * GetWoBLaser().direction, GetWoBLaser().Center.Y + num2);
                Projectile.position = vector2 - new Vector2(Projectile.width, Projectile.height) / 2f;
                //Projectile.rotation = GetWoBLaser().rotation;
                Projectile.velocity = Vector2.UnitX.RotatedBy(GetWoBLaser().rotation);
            }
            else
            {
                Projectile.Kill();
            }

            float num3 = 1f;
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= 120f)
            {
                Projectile.Kill();
                return;
            }
            //Projectile.localAI[0] = 60f;

            Projectile.scale = (float)Math.Sin(Projectile.localAI[0] * MathF.PI / 120f) * 10f * num3;
            if (Projectile.scale > num3)
            {
                Projectile.scale = num3;
            }

            float num4 = Projectile.velocity.ToRotation();
            Projectile.rotation = num4 - MathF.PI / 2f;
            Projectile.velocity = num4.ToRotationVector2();
            float num5 = Projectile.width;
            Vector2 samplingPoint = Projectile.Center;
            if (vector.HasValue)
            {
                samplingPoint = vector.Value;
            }

            float[] array = new float[3];
            Collision.LaserScan(samplingPoint, Projectile.velocity, num5 * Projectile.scale, 2400f, array);
            float num6 = 0f;
            for (int i = 0; i < array.Length; i++)
            {
                num6 += array[i];
            }

            num6 /= 3f;
            if (!Collision.CanHitLine(GetWoBLaser().Center, 1, 1, Main.player[GetWoBLaser().target].Center, 1, 1))
            {
                num6 = 2400f;
            }
            //num6 = 2400f;

            float amount = 0.5f;
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], num6, amount);
            int type = 235;
            Vector2 vector3 = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 14f);
            for (int j = 0; j < 2; j++)
            {
                float num7 = Projectile.velocity.ToRotation() + (Main.rand.NextBool(2) ? -1f : 1f) * (MathF.PI / 2f);
                float num8 = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 vector4 = new Vector2((float)Math.Cos(num7) * num8, (float)Math.Sin(num7) * num8);
                int num9 = Dust.NewDust(vector3, 0, 0, type, vector4.X, vector4.Y);
                Main.dust[num9].noGravity = true;
                Main.dust[num9].scale = 1.7f;
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 vector5 = Projectile.velocity.RotatedBy(1.5707963705062866) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
                int num10 = Dust.NewDust(vector3 + vector5 - Vector2.One * 4f, 8, 8, type, 0f, 0f, 100, default, 1.5f);
                Main.dust[num10].velocity *= 0.5f;
                Main.dust[num10].velocity.Y = 0f - Math.Abs(Main.dust[num10].velocity.Y);
            }

            DelegateMethods.v3_1 = new Vector3(0.9f, 0.3f, 0.3f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], Projectile.width * Projectile.scale, DelegateMethods.CastLight);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }

            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D value2 = ModContent.Request<Texture2D>("Clamity/Content/Bosses/WoB/Projectiles/WallOfBronzeLaserBeamMiddle", AssetRequestMode.ImmediateLoad).Value;
            Texture2D value3 = ModContent.Request<Texture2D>("Clamity/Content/Bosses/WoB/Projectiles/WallOfBronzeLaserBeamEnd", AssetRequestMode.ImmediateLoad).Value;
            float num = Projectile.localAI[1];
            Color color = new Color(250, 250, 250, 100);
            if (Projectile.localAI[0] % 5f == 0f)
            {
                frameDrawn++;
                if (frameDrawn >= 5)
                {
                    frameDrawn = 0;
                }
            }

            Vector2 position = Projectile.Center - Main.screenPosition;
            Rectangle? sourceRectangle = new Rectangle(0, value.Height / 5 * frameDrawn, value.Width, value.Height / 5);
            Main.EntitySpriteDraw(value, position, sourceRectangle, color, Projectile.rotation, new Vector2(value.Width, value.Height / 5) / 2f, Projectile.scale, SpriteEffects.None);
            num -= (value.Height / 5 / 2 + value3.Height / 5) * Projectile.scale;
            Vector2 center = Projectile.Center;
            center += Projectile.velocity * Projectile.scale * value.Height / 5f / 2f;
            if (num > 0f)
            {
                float num2 = 0f;
                int num3 = frameDrawn;
                while (num2 + 1f < num)
                {
                    Rectangle value4 = new Rectangle(0, value2.Height / 5 * num3, value2.Width, value2.Height / 5);
                    if (num - num2 < value4.Height)
                    {
                        value4.Height = (int)(num - num2);
                    }

                    Main.EntitySpriteDraw(value2, center - Main.screenPosition, value4, color, Projectile.rotation, new Vector2(value4.Width / 2f, 0f), Projectile.scale, SpriteEffects.None);
                    num3++;
                    if (num3 >= 5)
                    {
                        num3 = 0;
                    }

                    num2 += value4.Height * Projectile.scale;
                    center += Projectile.velocity * value4.Height * Projectile.scale;
                    value4.Y += value2.Height / 5;
                    if (value4.Y + value4.Height > value2.Height / 5)
                    {
                        value4.Y = 0;
                    }
                }
            }

            Vector2 position2 = center - Main.screenPosition;
            sourceRectangle = new Rectangle(0, value3.Height / 5 * frameDrawn, value3.Width, value3.Height / 5);
            Main.EntitySpriteDraw(value3, position2, sourceRectangle, color, Projectile.rotation, new Vector2(value3.Width, value3.Height / 5) / 2f, Projectile.scale, SpriteEffects.None);
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 velocity = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + velocity * Projectile.localAI[1], Projectile.width * Projectile.scale, DelegateMethods.CutTiles);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            float collisionPoint = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], 30f * Projectile.scale, ref collisionPoint))
            {
                return true;
            }

            return false;
        }

        /*public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage > 0)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 150);
            }
        }*/

        public override bool CanHitPlayer(Player target)
        {
            return Projectile.scale >= 0.5f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.getGoodWorld)
                target.AddBuff(BuffID.Frozen, 180);
        }
    }
}
