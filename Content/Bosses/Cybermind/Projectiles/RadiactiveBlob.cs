using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Clamity.Content.Bosses.Cybermind.Projectiles
{
    public class RadiactiveBlob : IchorBlob
    {
        public override void AI()
        {
            if (Projectile.position.Y > Projectile.ai[1] - 48f)
                Projectile.tileCollide = true;

            // Deal no damage and increment the variable used to kill the projectile after 15 seconds
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 900f)
            {
                Projectile.localAI[0] += 10f;
                Projectile.damage = 0;
            }

            // Kill the projectile 26 frames after it stops dealing damage
            if (Projectile.localAI[0] > 255f)
            {
                Projectile.Kill();
                Projectile.localAI[0] = 255f;
            }

            // Add yellow light based on alpha
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.22f / 255f, (255 - Projectile.alpha) * 0.9f / 255f, (255 - Projectile.alpha) * 0.42f / 255f);

            // Adjust projectile visibility based on the kill timer
            Projectile.alpha = (int)(100.0 + Projectile.localAI[0] * 0.7);

            if (Projectile.velocity.Y != 0f && Projectile.ai[0] == 0f)
            {
                // Rotate based on velocity, only do this here, because it's falling
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.PiOver2;

                Projectile.frameCounter++;
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 1)
                    Projectile.frame = 0;
            }
            else
            {
                // Prevent sliding
                Projectile.velocity.X = 0f;

                // Do not animate falling frames
                Projectile.ai[0] = 1f;

                if (Projectile.frame < 2)
                {
                    // Set frame to blob and frame counter to 0
                    Projectile.frame = 2;
                    Projectile.frameCounter = 0;

                    // Play squish sound
                    SoundEngine.PlaySound(SoundID.NPCDeath21, Projectile.Center);

                    // Emit dust
                    Vector2 dustRotation = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();
                    Vector2 dustVelocity = dustRotation * Projectile.velocity.Length();
                    for (int i = 0; i < 10; i++)
                    {
                        int ichorDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, 0f, 0f, 200, default, 1.6f);
                        Dust dust = Main.dust[ichorDust];
                        dust.position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                        dust.noGravity = true;
                        dust.velocity.Y -= 2f;
                        dust.velocity *= 3f;
                        dust.velocity += dustVelocity * Main.rand.NextFloat();
                        ichorDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, 0f, 0f, 100, default, 0.8f);
                        dust.position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                        dust.velocity.Y -= 2f;
                        dust.velocity *= 2f;
                        dust.noGravity = true;
                        dust.fadeIn = 1f;
                        dust.velocity += dustVelocity * Main.rand.NextFloat();
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        int ichorDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, 0f, 0f, 0, default, 2f);
                        Dust dust = Main.dust[ichorDust2];
                        dust.position = Projectile.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 3f;
                        dust.noGravity = true;
                        dust.velocity.Y -= 2f;
                        dust.velocity *= 0.5f;
                        dust.velocity += dustVelocity * (0.6f + 0.6f * Main.rand.NextFloat());
                    }
                }

                Projectile.rotation = 0f;
                Projectile.gfxOffY = 4f;

                Projectile.frameCounter++;
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 5)
                    Projectile.frame = 5;
            }

            // Do velocity code after the frame code, to avoid messing anything up
            // Reduce x velocity every frame
            Projectile.velocity.X *= 0.995f;

            // Stop falling if water or lava is hit
            if (Projectile.wet || Projectile.lavaWet)
            {
                Projectile.velocity.Y = 0f;
            }
            else
            {
                // Fall
                Projectile.velocity.Y += 0.1f;
                if (Projectile.velocity.Y > 6f)
                    Projectile.velocity.Y = 6f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

        }
    }
}
