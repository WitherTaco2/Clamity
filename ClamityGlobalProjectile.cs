using CalamityMod;
using Clamity.Content.Buffs.Shortstrike;
using Clamity.Content.Cooldowns;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity
{
    public class ClamityGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (!player.HasCooldown(ShortstrikeCooldown.ID))
            {
                /*if (projectile.type == ProjectileID.CopperShortswordStab)
                {
                    //Projectile.NewProjectile(projectile.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<ShortstrikeEffect>(), 0, 0, player.whoAmI);
                    player.AddBuff(ModContent.BuffType<CopperShortstrike>(), 60);
                    player.AddCooldown(ShortstrikeCooldown.ID, CalamityUtils.SecondsToFrames(2));
                }
                if (projectile.type == ProjectileID.TinShortswordStab)
                {
                    player.AddBuff(ModContent.BuffType<TinShortstrike>(), 60);
                    player.AddCooldown(ShortstrikeCooldown.ID, CalamityUtils.SecondsToFrames(2));
                }
                if (projectile.type == ProjectileID.IronShortswordStab)
                {
                    player.AddBuff(ModContent.BuffType<IronShortstrike>(), 60);
                    player.AddCooldown(ShortstrikeCooldown.ID, CalamityUtils.SecondsToFrames(2));
                }*/
                Shortstrike(player, projectile, ModContent.BuffType<CopperShortstrike>(), 1, ProjectileID.CopperShortswordStab);
                Shortstrike(player, projectile, ModContent.BuffType<TinShortstrike>(), 1, ProjectileID.TinShortswordStab);
                Shortstrike(player, projectile, ModContent.BuffType<IronShortstrike>(), 1, ProjectileID.IronShortswordStab);
                Shortstrike(player, projectile, ModContent.BuffType<LeadShortstrike>(), 1, ProjectileID.LeadShortswordStab);
                Shortstrike(player, projectile, ModContent.BuffType<SilverShortstrike>(), 1, ProjectileID.SilverShortswordStab);
                Shortstrike(player, projectile, ModContent.BuffType<TungstenShortstrike>(), 1, ProjectileID.TungstenShortswordStab);
                Shortstrike(player, projectile, ModContent.BuffType<GoldShortstrike>(), 1.2f, ProjectileID.GoldShortswordStab);
                Shortstrike(player, projectile, ModContent.BuffType<PlatinumShortstrike>(), 1.2f, ProjectileID.PlatinumShortswordStab);
                Shortstrike(player, projectile, ModContent.BuffType<GladiusShortstrike>(), 0.5f, ProjectileID.GladiusStab, 1.25f);
            }
        }
        private void Shortstrike(Player player, Projectile proj, int buffID, float timeInSeconds, int projectileID, float percent = 2)
        {
            if (proj.type == projectileID)
            {
                player.AddBuff(buffID, CalamityUtils.SecondsToFrames(timeInSeconds));
                player.AddCooldown(ShortstrikeCooldown.ID, (int)(CalamityUtils.SecondsToFrames(timeInSeconds) * percent));
                for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.PiOver4 / 3)
                {
                    Dust dust = Dust.NewDustPerfect(proj.Center + proj.velocity, DustID.Electric, Vector2.UnitX.RotatedBy(i) * 3f + proj.velocity);
                    dust.noGravity = true;
                }
            }
        }
    }
}
