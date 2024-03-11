using CalamityMod;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Profusion.Drop
{
    public class GodlikeMushroom : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Materials";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 1, 5);
            Item.rare = ModContent.RarityType<PureGreen>();
        }
        public void DrawBackAfterimage(SpriteBatch spriteBatch, Vector2 baseDrawPosition, Rectangle frame, float baseScale)
        {
            if (base.Item.velocity.X == 0f)
            {
                float num = MathF.Cos(1.618034f * Main.GlobalTimeWrappedHourly * 2f) + MathF.Cos(MathF.PI * Main.GlobalTimeWrappedHourly * 1.7f);
                num = num * 0.25f + 0.5f;
                num = MathF.Pow(num, 3);
                float num2 = MathHelper.Lerp(-0.3f, 1.2f, num);
                Color color = Color.Lerp(new Color(255, 218, 99), new Color(249, 134, 44), num);
                color *= MathHelper.Lerp(0.35f, 0.67f, CalamityUtils.Convert01To010(num));
                color.A = 25;
                float num3 = num2 * baseScale * 8f;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 position = baseDrawPosition + (MathF.PI * 2f * (float)i / 4f).ToRotationVector2() * num3;
                    spriteBatch.Draw(TextureAssets.Item[base.Item.type].Value, position, frame, color, 0f, Vector2.Zero, baseScale, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
