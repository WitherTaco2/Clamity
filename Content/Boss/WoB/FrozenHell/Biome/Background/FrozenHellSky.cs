using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;

namespace Clamity.Content.Boss.WoB.FrozenHell.Biome.Background
{
    public class FrozenHellSky : CustomSky
    {
        public int skyActiveLeeway;
        private bool skyActive;
        private float opacity;
        public override void Deactivate(params object[] args) => this.skyActive = false;
        public override void Reset() => this.skyActive = false;
        public override bool IsActive() => this.skyActive || (double)this.opacity > 0.0;
        public override void Activate(Vector2 position, params object[] args) => this.skyActive = true;
        public override void Update(GameTime gameTime)
        {
            if (!Main.LocalPlayer.Clamity().ZoneFrozenHell || Main.gameMenu)
            {
                skyActive = false;
                if (skyActiveLeeway > 0)
                    --skyActiveLeeway;
            }
            else if (skyActiveLeeway < 60)
                ++skyActiveLeeway;
            if (skyActive && opacity < 1f)
                opacity += 0.02f;
            else if (!skyActive && opacity > 0f)
                opacity -= 0.02f;

            if (Main.LocalPlayer.Clamity().ZoneFrozenHell)
                return;
            Filters.Scene["Clamity:FrozenHellSky"].Deactivate();
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            //throw new NotImplementedException();
        }
    }
}
