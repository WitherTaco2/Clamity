using CalamityMod.Particles;
using Clamity.Commons;
using Clamity.Content.Bosses.Profusion.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Profusion.NPCs
{
    public partial class ProfusionBoss : ModNPC
    {
        private void AwakanAnimation()
        {
            if (StateTimer == 1)
            {
                NPC.Center = Target.Center - Vector2.UnitY * 200;
                SoundStyle roar = SoundID.Roar;
                roar.Pitch = -2;
                SoundEngine.PlaySound(roar, NPC.Center);
            }
            if (StateTimer % 15 == 0 /*&& Projectile.timeLeft > 40*/)
            {
                GeneralParticleHandler.SpawnParticle(new DirectionalPulseRing(NPC.Center, Vector2.Zero, Color.DarkBlue, new Vector2(0.5f, 0.5f), Main.rand.NextFloat(12f, 25f), 0f, 40f, 60));
            }
            if (StateTimer >= 60)
                SetState(ProfusionAIState.ThormAttack);
        }
        private void ThornAttack(float enrageScale)
        {
            BasicMovement(enrageScale, -Vector2.UnitY * 400);

            if (StateTimer % (int)(30 / enrageScale) == 0 && StateTimer <= 30)
            {
                SoundStyle summon = SoundID.NPCDeath13;
                summon.Pitch = -2;
                //summon.Volume = 0.8f;
                SoundEngine.PlaySound(summon, NPC.Center);

                int type = ModContent.ProjectileType<MushroomThorn>();
                int damage = NPC.GetProjectileDamageClamity(type);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(Target.Center).SafeNormalize(Vector2.Zero).RotatedByRandom(1) * 5, type, damage, 1f, Main.myPlayer);
            }
            if (StateTimer >= 60)
                SetState(ProfusionAIState.ThormAttack);

        }
        private void BasicMovement(float enrageScale, Vector2 offset)
        {
            Vector2 targetPosition = Target.Center + offset;
            if ((targetPosition - NPC.Center).Length() > 10)
                NPC.velocity = Vector2.Lerp((targetPosition - NPC.Center), NPC.velocity, 0.90f) * enrageScale;
            else
                NPC.velocity *= 0.5f;
            NPC.rotation = MathF.Sqrt(MathF.Abs(NPC.velocity.X / 100));
            if (NPC.velocity.X < 0)
                NPC.rotation = MathHelper.TwoPi - NPC.rotation;

        }
    }
}
