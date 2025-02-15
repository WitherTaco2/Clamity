using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion.Sky
{
    public class DistortionSky : CustomSky
    {
        private bool skyActive;
        public override bool IsActive() => skyActive;
        public override void Reset() => skyActive = false;
        public override void Activate(Vector2 position, params object[] args) => skyActive = true;
        public override void Deactivate(params object[] args) => skyActive = false;
        public override void Update(GameTime gameTime)
        {
            if (!Main.LocalPlayer.Clamity().ZoneDistortion || Main.gameMenu)
            {
                skyActive = false;
            }
            else
                skyActive = true;

            if (skyActive)
                SkyManager.Instance["Ambience"].Deactivate();
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {

            Main.spriteBatch.PrepareForShaders();
            DrawBackground();
        }
        public static void DrawBackground()
        {
            Texture2D WhitePixel = ModContent.Request<Texture2D>("Clamity/Assets/Textures/Pixel").Value;

            Vector2 screenArea = new(Main.instance.GraphicsDevice.Viewport.Width, Main.instance.GraphicsDevice.Viewport.Height);
            Vector2 textureArea = screenArea / WhitePixel.Size() * 2f;

            var backgroundShader = ShaderManager.GetShader("Clamity.TheDistortion");
            backgroundShader.TrySetParameter("globalTimer", Main.GlobalTimeWrappedHourly);
            backgroundShader.TrySetParameter("backgroundColor1", Color.Black.ToVector4());
            backgroundShader.TrySetParameter("backgroundColor2", Color.Purple.ToVector4());
            backgroundShader.SetTexture(ModContent.Request<Texture2D>("Clamity/Assets/Textures/Noice/Mist2"), 1, SamplerState.LinearWrap);
            backgroundShader.Apply();

            Main.spriteBatch.Draw(WhitePixel, screenArea * 0.5f, null, Color.White, 0f, WhitePixel.Size() * 0.5f, textureArea, 0, 0f);

        }
        public override float GetCloudAlpha() => 0;
    }
}
