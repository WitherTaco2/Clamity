﻿using CalamityMod.Events;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace Clamity.Content.Boss.Pyrogen.Projectiles
{
    public class FireBarrage : BrimstoneBarrage
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 2, Type);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

        }
    }
    public class FireBarrageHoming : FireBarrage
    {
        public override string Texture => "Clamity/Content/Boss/Pyrogen/Projectiles/FireBarrage";
        public int TargetIndex = -1;
        public override void AI()
        {
            base.AI();

            if (TargetIndex >= 0)
            {
                if (!Main.npc[TargetIndex].active || !Main.npc[TargetIndex].CanBeChasedBy())
                {
                    TargetIndex = -1;
                }
                else
                {
                    Vector2 value = Projectile.SafeDirectionTo(Main.npc[TargetIndex].Center)/* * (Projectile.velocity.Length() + 3.5f)*/;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, value, 0.01f);
                }
            }

            if (TargetIndex == -1)
            {
                NPC nPC = Projectile.Center.ClosestNPCAt(1600f);
                if (nPC != null)
                {
                    TargetIndex = nPC.whoAmI;
                }
            }
        }
    }
    public class Fireblast : SCalBrimstoneFireblast
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 2, Type);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(in ImpactSound, base.Projectile.Center);
            bool bossRushActive = BossRushEvent.BossRushActive;
            bool flag = CalamityWorld.death || bossRushActive;
            bool flag2 = CalamityWorld.revenge || bossRushActive;
            bool flag3 = Main.expertMode || bossRushActive;
            if (base.Projectile.ai[1] == 0f && base.Projectile.owner == Main.myPlayer)
            {
                int num = (bossRushActive ? 20 : (flag ? 16 : (flag2 ? 14 : (flag3 ? 12 : 8))));
                float num2 = MathF.PI * 2f / (float)num;
                int type = ModContent.ProjectileType<FireBarrage>();
                float num3 = 7f;
                Vector2 spinningpoint = new Vector2(0f, 0f - num3);
                for (int i = 0; i < num; i++)
                {
                    Vector2 velocity = spinningpoint.RotatedBy(num2 * (float)i);
                    Projectile.NewProjectile(base.Projectile.GetSource_FromThis(), base.Projectile.Center, velocity, type, (int)Math.Round((double)base.Projectile.damage * 0.75), 0f, base.Projectile.owner, 0f, 1f);
                }
            }

            int type2 = 235;

            Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, type2, 0f, 0f, 50);
            for (int j = 0; j < 10; j++)
            {
                int num4 = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, type2, 0f, 0f, 0, default(Color), 1.5f);
                Main.dust[num4].noGravity = true;
                Main.dust[num4].velocity *= 3f;
                num4 = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, type2, 0f, 0f, 50);
                Main.dust[num4].velocity *= 2f;
                Main.dust[num4].noGravity = true;
            }
        }
    }
}
