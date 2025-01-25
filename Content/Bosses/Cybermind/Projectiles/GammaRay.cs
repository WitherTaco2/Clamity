using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Cybermind.Projectiles
{
    public class GammaRay : BaseLaserbeamProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override float Lifetime => 180;
        public override float MaxLaserLength => 2400;
        public override float MaxScale => 3f;
        private string StandartTexturePath => "Clamity/Content/Bosses/Cybermind/Projectiles/GammaRay";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override Texture2D LaserBeginTexture => ModContent.Request<Texture2D>(StandartTexturePath + "_Start").Value;
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>(StandartTexturePath + "_Middle").Value;
        public override Texture2D LaserEndTexture => ModContent.Request<Texture2D>(StandartTexturePath + "_End").Value;
        public override void ExtraBehavior()
        {
            if (NPCs.Cyberhive.Myself is not null)
                Projectile.Center = NPCs.Cyberhive.Myself.Center;
            else
                Projectile.Kill();
        }
    }
}
