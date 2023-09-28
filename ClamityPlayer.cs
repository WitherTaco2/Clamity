using CalamityMod;
using CalamityMod.Cooldowns;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace Clamity
{
    public class ClamityPlayer : ModPlayer
    {
        public bool realityRelocator;
        public override void ResetEffects()
        {
            realityRelocator = false;
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (base.Player.dead)
            {
                return;
            }
            if (CalamityKeybinds.NormalityRelocatorHotKey.JustPressed && realityRelocator && Main.myPlayer == Player.whoAmI && !Player.CCed)
            {
                Vector2 vector = default(Vector2);
                vector.X = (float)Main.mouseX + Main.screenPosition.X;
                if (base.Player.gravDir == 1f)
                {
                    vector.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)base.Player.height;
                }
                else
                {
                    vector.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                }

                vector.X -= base.Player.width / 2;
                if (vector.X > 50f && vector.X < (float)(Main.maxTilesX * 16 - 50) && vector.Y > 50f && vector.Y < (float)(Main.maxTilesY * 16 - 50) && !Collision.SolidCollision(vector, base.Player.width, base.Player.height))
                {
                    base.Player.Teleport(vector, 4);
                    NetMessage.SendData(65, -1, -1, null, 0, base.Player.whoAmI, vector.X, vector.Y, 1);
                }
            }

        }
    }
}
