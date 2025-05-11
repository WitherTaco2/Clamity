using Clamity.Content.Particles;
using Microsoft.Xna.Framework;
using Terraria;

namespace Clamity.Content.Bosses.Ihor.Particles
{
    public class IhorChargeChromaticBurstParticle : ChromaticBurstParticle
    {
        public int npcIndex;
        public IhorChargeChromaticBurstParticle(int npc)
            : base(Vector2.Zero, Vector2.Zero, Color.LightBlue, 10, 2f, 0f)
        {
            this.npcIndex = npc;
        }
        public override void Update()
        {
            base.Update();
            NPC npc = Main.npc[npcIndex];
            Position = npc.Center + Vector2.UnitX.RotatedBy(npc.rotation + MathHelper.PiOver2) * npc.width / 2;
        }
    }
}
