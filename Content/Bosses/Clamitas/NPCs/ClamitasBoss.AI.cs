using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.NPCs;
using CalamityMod.Particles;
using Clamity.Content.Bosses.Clamitas.Projectiles;
using Clamity.Content.Particles;
using Luminance.Common.Easings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Clamitas.NPCs
{
    public partial class ClamitasBoss : ModNPC
    {
        public enum Attacks : int
        {
            PreFight = 0,
            StartingCutscene = 1,

            FastTeleports,
            CrossSpirits,
            SpiritWave,

            HahaLimboMonent,
        }

        private int attack = (int)Attacks.PreFight;
        public Attacks CurrentAttack
        {
            get => (Attacks)attack;
            set => attack = (int)value;
        }
        public int AttackTimer
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public bool BattleIsStarted => CurrentAttack != Attacks.PreFight && (CurrentAttack != Attacks.StartingCutscene);
        public Player player => Main.player[NPC.target];
        public override void AI()
        {
            Myself = NPC;
            NPC.TargetClosest();
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();

            NPC.chaseable = hasBeenHit;

            if (BattleIsStarted && Main.netMode != NetmodeID.Server && !Main.player[NPC.target].dead && Main.player[NPC.target].active)
            {
                player.AddBuff(ModContent.BuffType<CalamityMod.Buffs.StatDebuffs.Clamity>(), 2);
                player.AddBuff(ModContent.BuffType<BossEffects>(), 2);
            }
            if (Main.player[NPC.target].dead && !Main.player[NPC.target].active)
            {
                NPC.active = false;
            }

            if (!hide)
            {
                Lighting.AddLight(NPC.Center, 0.75f, 0, 0);
            }

            if (!statChange)
            {
                NPC.defense = 35;
                NPC.damage = 200;

                statChange = true;
            }

            //Main.NewText($"{attack} - {AttackTimer}");
            AttackTimer++;
            switch (CurrentAttack)
            {
                case Attacks.PreFight:
                    PreFightState();
                    break;
                case Attacks.StartingCutscene:
                    StartingCutsceneState();
                    break;
                case Attacks.CrossSpirits:
                    CrossSpiritsState();
                    break;
                case Attacks.SpiritWave:
                    SpiritWaveState();
                    break;

            }
        }
        private Attacks SetNextAttack(Attacks nextAttack)
        {
            CurrentAttack = nextAttack;
            AttackTimer = 0;

            return nextAttack;
        }
        private void PreFightState()
        {
            //Music = -1;

            if (NPC.justHit)
            {
                ++hitAmount;
                hasBeenHit = true;
            }

            //NPC.life = NPC.lifeMax;

            if (hitAmount >= 10)
            {
                SetNextAttack(Attacks.StartingCutscene);
            }
        }
        private void StartingCutsceneState()
        {
            hide = true;
            int animTime = 120;

            if (AttackTimer % 10 == 0)
            {
                //ChromaticBurstParticleEasing particle1 = new ChromaticBurstParticleEasing(NPC.Center, Vector2.Zero, Color.Red, (Color.Red * 0.4f) with { A = 255 }, 20, 2f, 0.00f, EasingCurves.Quadratic, EasingType.Out);
                ChromaticBurstParticleEasingAlt particle1 = new ChromaticBurstParticleEasingAlt(NPC.Center, Vector2.Zero, Color.Red, 20, 2f, 0.00f, EasingCurves.Quadratic, EasingType.Out);
                //ChromaticBurstParticle particle1 = new ChromaticBurstParticle(NPC.Center, Vector2.Zero, Color.Red, 20, 1.5f, 0.05f);
                GeneralParticleHandler.SpawnParticle(particle1);
            }
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = Utils.GetLerpValue(0, 1, AttackTimer / (float)60, true);

            if (AttackTimer > animTime)
            {
                hide = false;
                SetNextAttack(Attacks.CrossSpirits);
            }
        }
        private void CrossSpiritsState()
        {
            int delay = 100;
            if (AttackTimer % delay == 0)
            {
                float num = AttackTimer % (delay * 2) == 0 ? MathHelper.PiOver4 : 0;
                Vector2 center = player.Center;
                for (int i = 0; i < 4; i++)
                {
                    float rot = MathHelper.PiOver2 * i + num;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), center + Vector2.UnitX.RotatedBy(rot) * 2000, -Vector2.UnitX.RotatedBy(rot) * 60, ModContent.ProjectileType<BrimstoneSpiritsSpawner>(), 1, 1, NPC.target, 100, 5);
                }
                SoundEngine.PlaySound(SoundID.NPCHit36, player.Center);
            }

            if (AttackTimer > delay * 4 + delay / 2)
            {
                SetNextAttack(Attacks.SpiritWave);
            }
        }
        private void SpiritWaveState()
        {

        }
        private void State()
        {

        }

    }
}
