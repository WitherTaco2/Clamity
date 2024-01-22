using CalamityMod;
using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Clamity.Content.Cooldowns
{
    public class ShortstrikeCharge : CooldownHandler
    {
        public new static string ID => nameof(ShortstrikeCharge);
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Mods.Clamity.UI.Cooldowns." + ShortstrikeCharge.ID);
        public override string Texture => "Clamity/Content/Cooldowns/ShortstrikeCooldown";
        public override Color OutlineColor => instance.timeLeft == 100 ? Main.DiscoColor : new Color(255, 226, 217);
        public override Color CooldownStartColor => new Color(205, 134, 71) * (instance.timeLeft == 100 ? (MathF.Sin(Main.GlobalTimeWrappedHourly * 4f) / 4f + 0.75f) : 1f);
        public override Color CooldownEndColor => new Color(235, 166, 135) * (instance.timeLeft == 100 ? (MathF.Sin(Main.GlobalTimeWrappedHourly * 4f) / 4f + 0.75f) : 1f);

        private float AdjustedCompletion => (float)instance.timeLeft / 100f;
        public override bool SavedWithPlayer => false;
        public override bool PersistsThroughDeath => false;
        public override void ApplyBarShaders(float opacity)
        {
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseOpacity(opacity);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSaturation(this.AdjustedCompletion);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseColor(this.CooldownStartColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSecondaryColor(this.CooldownEndColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].Apply(new DrawData?());
        }
        public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            base.DrawExpanded(spriteBatch, position, opacity, scale);
            float x = ((instance.timeLeft > 9) ? (-10f) : (-5f));
            CalamityUtils.DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, instance.timeLeft.ToString(), position + new Vector2(x, 4f) * scale, Color.Lerp(CooldownStartColor, Color.OrangeRed, 1f - instance.Completion), Color.Black, scale);
        }

        public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
            Texture2D value3 = ModContent.Request<Texture2D>(OverlayTexture).Value;
            spriteBatch.Draw(value2, position, null, OutlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            int num = (int)Math.Ceiling((float)value3.Height * AdjustedCompletion);
            spriteBatch.Draw(sourceRectangle: new Rectangle(0, num, value3.Width, value3.Height - num), texture: value3, position: position + Vector2.UnitY * num * scale, color: OutlineColor * opacity * 0.9f, rotation: 0f, origin: value.Size() * 0.5f, scale: scale, effects: SpriteEffects.None, layerDepth: 0f);
            float x = ((instance.timeLeft > 9) ? (-10f) : (-5f));
            CalamityUtils.DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, instance.timeLeft.ToString(), position + new Vector2(x, 4f) * scale, Color.Lerp(CooldownStartColor, Color.OrangeRed, 1f - instance.Completion), Color.Black, scale);
        }
    }
}
