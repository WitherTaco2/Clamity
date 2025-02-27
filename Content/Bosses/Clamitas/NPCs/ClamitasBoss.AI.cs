using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.NPCs;
using Terraria;
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
        public override void AI()
        {
            Myself = NPC;
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
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

            AttackTimer++;
            switch (CurrentAttack)
            {
                case Attacks.PreFight:
                    PreFightState();

                    break;
                case Attacks.StartingCutscene:

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
            Music = -1;

            if (NPC.justHit && hitAmount < 5)
            {
                hitAmount++;
                hasBeenHit = true;
            }

            if (hitAmount == 10)
            {
                SetNextAttack(Attacks.StartingCutscene);
            }
        }

    }
}
