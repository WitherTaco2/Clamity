﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Typeless;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity
{
    public class ClamityGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public float[] extraAI = new float[5];
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];

            UpdateAflameAccesory(projectile, target, hit, damageDone);
        }
        private void UpdateAflameAccesory(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            ClamityPlayer modPlayer = player.Clamity();
            /*if (modPlayer.aflameAccList.Contains(ModContent.ItemType<LuxorsGift>()))
            {

            }*/
            List<int> list = modPlayer.aflameAccList;
            AddVulHexDebuff(list, projectile, target, ItemID.VolatileGelatin, ProjectileID.VolatileGelatinBall);
            AddVulHexDebuff(list, projectile, target, ItemID.BoneGlove, ProjectileID.BoneGloveProj);
            AddVulHexDebuff(list, projectile, target, ItemID.BoneHelm, 964);
            AddVulHexDebuff(list, projectile, target, ItemID.SporeSac, ProjectileID.SporeTrap, ProjectileID.SporeTrap2, ProjectileID.SporeGas, ProjectileID.SporeGas2, ProjectileID.SporeGas3);

            AddVulHexDebuff(list, projectile, target, ModContent.ItemType<LuxorsGift>(), ModContent.ProjectileType<LuxorsGiftMelee>(), ModContent.ProjectileType<LuxorsGiftRanged>(), ModContent.ProjectileType<LuxorsGiftMagic>(), ModContent.ProjectileType<LuxorsGiftRogue>(), ModContent.ProjectileType<LuxorsGiftSummon>());
            AddVulHexDebuff(list, projectile, target, ModContent.ItemType<FungalClump>(), ModContent.ProjectileType<FungalClumpMinion>());
            AddVulHexDebuff(list, projectile, target, new int[] { ModContent.ItemType<WifeinaBottle>(), ModContent.ItemType<HeartoftheElements>() }, ModContent.ProjectileType<SandBolt>(), 32);
            AddVulHexDebuff(list, projectile, target, new int[] { ModContent.ItemType<EyeoftheStorm>(), ModContent.ItemType<HeartoftheElements>() }, ModContent.ProjectileType<CloudElementalMinion>());
            AddVulHexDebuff(list, projectile, target, new int[] { ModContent.ItemType<RoseStone>(), ModContent.ItemType<HeartoftheElements>() }, ModContent.ProjectileType<BrimstoneFireballMinion>(), ModContent.ProjectileType<BrimstoneExplosionMinion>());
            AddVulHexDebuff(list, projectile, target, new int[] { ModContent.ItemType<PearlofEnthrallment>(), ModContent.ItemType<HeartoftheElements>() }, ModContent.ProjectileType<WaterSpearFriendly>(), ModContent.ProjectileType<FrostMistFriendly>(), ModContent.ProjectileType<WaterElementalSong>());

            AddVulHexDebuff(list, projectile, target, new int[] { ModContent.ItemType<ProfanedSoulArtifact>(), ModContent.ItemType<ProfanedSoulCrystal>() }, ModContent.ProjectileType<MiniGuardianDefense>(), /*ModContent.ProjectileType<MiniGuardianSpear>(),*/ ModContent.ProjectileType<MiniGuardianAttack>());
            AddVulHexDebuff(list, projectile, target, ModContent.ItemType<AngelicAlliance>(), ModContent.ProjectileType<AngelicAllianceArchangel>(), ModContent.ProjectileType<AngelRay>());

            AddVulHexDebuff(list, projectile, target, ModContent.ItemType<StatisVoidSash>(), ModContent.ProjectileType<CosmicScythe>());
        }
        private void AddVulHexDebuff(List<int> list, Projectile proj, NPC target, int acc, params int[] projList)
        {
            if (list.Contains(acc))
            {
                foreach (int i in projList)
                {
                    if (proj.type == i)
                    {
                        target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
                        break;
                    }
                }
            }
        }
        private void AddVulHexDebuff(List<int> list, Projectile proj, NPC target, int[] accs, params int[] projList)
        {
            foreach (int item in accs)
            {
                if (list.Contains(item))
                {
                    foreach (int i in projList)
                    {
                        if (proj.type == i)
                        {
                            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
                            break;
                        }
                    }
                }
            }
        }
        /*private void Shortstrike(Player player, Projectile proj, int buffID, float timeInSeconds, int projectileID, float percent = 2)
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
        }*/
    }
}
