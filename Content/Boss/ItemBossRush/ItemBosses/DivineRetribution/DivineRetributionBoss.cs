using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;

namespace Clamity.Content.Boss.ItemBossRush.ItemBosses.DivineRetribution
{
    public class DivineRetributionBoss : BaseItemBoss
    {
        public enum DivineRetributionAttacks : int
        {
            CurcleOfSpears = 0
        }
        public override void SafeSetDefaults()
        {
            NPC.width = NPC.height = 88;
            NPC.value = 0;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.lifeMax = 1000000;
            double num = (double)CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * num);

            NPC.defense = 100;
            NPC.DR_NERD(0.25f);
            NPC.knockBackResist = 0;

            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;

            NPC.aiStyle = -1;
            AIType = -1;
        }
        public override void SafeAI()
        {
            float potentialRotation = NPC.velocity.X / 10;
            if (potentialRotation > MathHelper.TwoPi)
                potentialRotation -= MathHelper.TwoPi;
            if (potentialRotation < MathHelper.TwoPi)
                potentialRotation += MathHelper.TwoPi;
            NPC.rotation += (NPC.rotation - potentialRotation) / 3;
            //NPC.rotation = (NPC.rotation + )
            if (Attack == (int)DivineRetributionAttacks.CurcleOfSpears)
            {

            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            //NPC.damage = (int)(NPC.damage);
        }
    }
}
