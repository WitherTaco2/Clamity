﻿using CalamityMod.Sounds;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria;
using CalamityMod.NPCs.ExoMechs.Ares;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Boss;
using Terraria.ID;
using Clamity.Content.Boss.WoB.Projectiles;
using Clamity.Commons;

namespace Clamity.Content.Boss.WoB.NPCs
{
    public class WallOfBronzeLaser : BaseWoBGunAI
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.damage = 200;
            NPC.width = 172;
            NPC.height = 108;
            NPC.defense = 50;
            NPC.DR_NERD(0.2f);
            NPC.LifeMaxNERB(20000, new int?(30000), new int?(40000));
            NPC.lifeMax += (int)(NPC.lifeMax * CalamityConfig.Instance.BossHealthBoost * 0.01f);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.Opacity = 0.0f;
            NPC.knockBackResist = 0.0f;
            NPC.canGhostHeal = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.DeathSound = new SoundStyle?(CommonCalamitySounds.ExoDeathSound);
            NPC.HitSound = new SoundStyle?(SoundID.NPCHit4);
            NPC.netAlways = true;
            NPC.hide = true;
            NPC.Calamity().VulnerableToSickness = new bool?(false);
            NPC.Calamity().VulnerableToElectricity = new bool?(true);
            if (Main.getGoodWorld)
                NPC.scale = 0.5f;
        }
        public override int MaxParticleTimer => 200;
        public override int MaxTimer => 600 * (Main.expertMode ? 1 : 2);
        public static readonly SoundStyle TelSound = new SoundStyle("CalamityMod/Sounds/Custom/ExoMechs/AresLaserArmCharge")
        {
            Volume = 1.1f
        };

        public static readonly SoundStyle LaserbeamShootSound = new SoundStyle("CalamityMod/Sounds/Custom/ExoMechs/AresLaserArmShoot")
        {
            Volume = 1.1f
        };
        public override void Attack()
        {
            Timer++;
            if (Timer == 1)
            {
                SoundEngine.PlaySound(LaserbeamShootSound, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(),
                                         NPC.Center,
                                         Vector2.UnitX.RotatedBy(NPC.rotation),
                                         ModContent.ProjectileType<WallOfBronzeLaserBeamStart>(),
                                         NPC.GetProjectileDamageClamity(ModContent.ProjectileType<WallOfBronzeLaserBeamStart>()),
                                         0,
                                         Main.myPlayer,
                                         0f,
                                         NPC.whoAmI);
            }
            if (Timer >= 120)
            {
                Timer = 0;
                AIState = 0;
            }
        }
        public override void ExtraAI()
        {
            //AresLaserBeamStart
            if (AIState != 2 && ParticleTimer < MaxParticleTimer - 60)
                NPC.rotation = (Main.player[NPC.target].Center - NPC.Center).ToRotation();
            else if (AIState == 2)
            {
                int num = 1;
                if ((Main.player[NPC.target].Center - NPC.Center).ToRotation() > NPC.rotation)
                    num = -1;
                NPC.rotation -= 0.001f * num;
            }
        }
        public override void AIinParticleState()
        {
            SoundEngine.PlaySound(TelSound, NPC.Center);
        }
        public override void ExtraDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (GetWoB().spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D value6 = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomLine").Value;
            Color color3 = Color.Lerp(Color.OrangeRed, Color.White, ParticleTimer / MaxParticleTimer);
            spriteBatch.Draw(value6,
                             NPC.Center /*- base.NPC.rotation.ToRotationVector2() * base.NPC.spriteDirection * 104f*/ - screenPos,
                             null,
                             color3,
                             base.NPC.rotation + MathF.PI / 2f,
                             new Vector2((float)value6.Width / 2f, value6.Height),
                             new Vector2(1f * ParticleTimer / MaxParticleTimer, 4200f),
                             effects,
                             0f);

        }
    }
}