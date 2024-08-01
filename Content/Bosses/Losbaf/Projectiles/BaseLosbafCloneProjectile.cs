using Clamity.Content.Bosses.Losbaf.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf.Projectiles
{
    public class BaseLosbafCloneProjectile : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override string Texture => ModContent.GetInstance<LosbafSuperboss>().Texture;
        public override Color? GetAlpha(Color lightColor)
        {
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    return new Color(0, 255, 255, 128);
                //return Color.Cyan;
                case 1:
                    return new Color(255, 255, 0, 128);
                //return Color.Yellow;
                case 2:
                    return new Color(255, 0, 255, 128);
                    //return Color.Magenta;
            }
            return base.GetAlpha(lightColor);
        }
    }
}
