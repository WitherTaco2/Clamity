using CalamityMod.MainMenu;
using Clamity.Content.Biomes.EntropicSpace;
using Clamity.Content.Biomes.EntropicSpace.Sky;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Menu
{
    public class EntropicSpaceMenu : CalamityMainMenu
    {
        public override string DisplayName => "Entropic Space";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Clamity/Content/Menu/Logo");

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Vector2 position = new Vector2(Main.screenWidth / 2f, 100f);

            Main.spriteBatch.PrepareForShaders();
            EntropicSpaceSky.DrawBackground(new Vector2(0, EntropicSpaceSubworld.SubworldHeight * 16));
            //Main.spriteBatch.ResetToDefault();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);


            //spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            //spriteBatch.Draw(ModContent.Request<Texture2D>("Clamity/Content/Menu/Logo_Back").Value, position, null, Color.Lerp(Color.Blue, Color.Cyan, MathF.Sin(Main.GlobalTimeWrappedHourly) / 2 + 0.5f), logoRotation, Logo.Value.Size() * 0.5f, logoScale * (1.01f + MathF.Sin(Main.GlobalTimeWrappedHourly * 2 / 3) / 100), SpriteEffects.None, 0f);
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("Clamity/Content/Menu/Logo_Back").Value,
                                 position + Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 8 * i) * (1.01f + MathF.Sin(Main.GlobalTimeWrappedHourly * 4 / 3)) * 2,
                                 null, Color.Lerp(Color.Blue, Color.Purple, MathF.Sin(Main.GlobalTimeWrappedHourly) / 2 + 0.5f),
                                 logoRotation, Logo.Value.Size() * 0.5f, logoScale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(Logo.Value, position, null, drawColor, logoRotation, Logo.Value.Size() * 0.5f, logoScale, SpriteEffects.None, 0f);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            return false;
        }
        public override int Music => Clamity.mod.GetMusicFromMusicMod("TheDistortion/ShatteredIslands") ?? MusicID.Title;
    }
}
