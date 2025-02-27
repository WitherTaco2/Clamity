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
        private Attacks CurrentAttack
        {
            get => (Attacks)attack;
            set => attack = (int)value;
        }
        public override void AI()
        {
            Myself = NPC;
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();
            if (hitAmount < 5)
            {
                hitAmount++;
                hasBeenHit = true;
            }

            NPC.chaseable = hasBeenHit;


            //hitAmount = 5;
            if (hitAmount != 5)
            {
                return;
            }

            if (Main.netMode != NetmodeID.Server && !Main.player[NPC.target].dead && Main.player[NPC.target].active)
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


        }
    }
}
