using CalamityMod;
using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.ItemBossRush.ItemBosses
{
    public class BaseItemBoss : ModNPC
    {
        public int LifeMax => 1000000;
        public override void SetDefaults()
        {
            //NPC.lifeMax = LifeMax;
            NPC.LifeMaxNERB(LifeMax, (int)(LifeMax * 1.2), (int)(LifeMax * 1.5));
            SafeSetDefaults();
        }
        public virtual void SafeSetDefaults()
        {

        }
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == byte.MaxValue || Main.player[NPC.target].dead || !Main.player[NPC.target].active || !TwentyTwoBulletTheory.BulletTheoryActive)
            {
                NPC.TargetClosest(true);
                if ((NPC.target == (int)byte.MaxValue || Main.player[NPC.target].dead || !Main.player[NPC.target].active || !TwentyTwoBulletTheory.BulletTheoryActive) && !NPC.despawnEncouraged)
                    NPC.EncourageDespawn(30);
            }
            SafeAI();
        }
        //public ref float Attack => ref NPC.ai[0];
        public int Attack
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = (float)value;
        }
        public int Timer
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = (float)value;
        }
        public virtual void SafeAI()
        {

        }
    }
}
