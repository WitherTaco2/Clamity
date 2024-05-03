using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace Clamity.Content.Biomes.FrozenHell.Biome.Background
{
    public class FrozenHellSky : CustomSky
    {
        //public int skyActiveLeeway;
        private bool skyActive;
        private float opacity;
        public override void Deactivate(params object[] args) => skyActive = false;
        public override void Reset() => skyActive = false;
        public override bool IsActive() => skyActive || opacity > 0.0;
        public override void Activate(Vector2 position, params object[] args) => skyActive = true;
        public override void Update(GameTime gameTime)
        {
            if (!Main.LocalPlayer.Clamity().ZoneFrozenHell || Main.gameMenu)
            {
                skyActive = false;
            }
            if (skyActive && opacity < 1f)
                opacity += 0.02f;
            else if (!skyActive && opacity > 0f)
                opacity -= 0.02f;

            //if (!Main.LocalPlayer.Clamity().ZoneFrozenHell)
            //    Filters.Scene["Clamity:FrozenHellSky"].Deactivate();
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            //throw new NotImplementedException();
        }
        public override Color OnTileColor(Color inColor)
        {
            return Color.Lerp(inColor, new Color(0, 148, 255), opacity);
        }
    }
    public class FrozenHellShaderData : ScreenShaderData
    {
        public FrozenHellShaderData(string passName)
        : base(passName)
        {
        }

        public override void Apply()
        {
            Vector3 vector = Main.ColorOfTheSkies.ToVector3();
            vector *= 0.4f;
            //UseOpacity(Math.Max(vector.X, Math.Max(vector.Y, vector.Z)));
            //UseColor(0, 148, 255);
            base.Apply();
        }
    }
}
