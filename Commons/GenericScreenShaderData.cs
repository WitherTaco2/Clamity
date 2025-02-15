using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace Clamity.Commons
{
    internal class GenericScreenShaderData : ScreenShaderData
    {
        public GenericScreenShaderData(string passName)
            : base(passName)
        {
        }

        public GenericScreenShaderData(Ref<Effect> shader, string passName)
            : base(shader, passName)
        {
        }

        public override void Apply()
        {
            UseTargetPosition(Main.LocalPlayer.Center);
            UseColor(Color.Transparent);
            base.Apply();
        }
    }
}
