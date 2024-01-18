using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Clamity.Content.Boss.WoB.Projectiles
{
    public class WoBDeathrayStart : ThanatosBeamStart
    {
        public override Texture2D LaserMiddleTexture => ModContent.Request<Texture2D>("Clamity/Content/Boss/WoB/Projectiles/").Value;
    }
}
