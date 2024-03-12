﻿using CalamityMod.MainMenu;

namespace Clamity.Content.Menu
{
    public class ClamityMenu : CalamityMainMenu
    {
        public override string DisplayName => "Clamity Style";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Clamity/Content/Menu/Logo");

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Texture2D value = ModContent.Request<Texture2D>("Clamity/Content/Menu/MenuAlt").Value;
            Vector2 zero = Vector2.Zero;
            float num = Main.screenWidth / (float)value.Width;
            float num2 = Main.screenHeight / (float)value.Height;
            float num3 = num;
            if (num != num2)
            {
                if (num2 > num)
                {
                    num3 = num2;
                    zero.X -= (value.Width * num3 - Main.screenWidth) * 0.5f;
                }
                else
                {
                    zero.Y -= (value.Height * num3 - Main.screenHeight) * 0.5f;
                }
            }
            zero += value.Size() / 4f;
            spriteBatch.Draw(value, zero + Vector2.UnitX * MathF.Sin(Main.GlobalTimeWrappedHourly) * 2f, null, Color.White, MathF.Sin(Main.GlobalTimeWrappedHourly / 2f) / 60f, value.Size() / 2f, num3 * 1.05f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(value, zero, null, Color.White, 0f, value.Size() / 2f, num3, SpriteEffects.None, 0f);
            for (int i = 0; i < 5; i++)
            {
                if (Main.rand.NextBool(4))
                {
                    int lifetime = Main.rand.Next(200, 300);
                    float depth = Main.rand.NextFloat(1.8f, 5f);
                    Vector2 startingPosition = new Vector2(Main.screenWidth * Main.rand.NextFloat(-0.1f, 1.1f), Main.screenHeight * 1.05f);
                    //Vector2 startingVelocity = -Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-0.9f, 0.9f)) * 4f;
                    Color color = selectCinderColor();
                    Cinders.Add(new Cinder(lifetime, Cinders.Count, depth, color, startingPosition, new Vector2(0, -2)));
                }
            }

            for (int j = 0; j < Cinders.Count; j++)
            {
                Cinders[j].Scale = Utils.GetLerpValue(Cinders[j].Lifetime, Cinders[j].Lifetime / 3, Cinders[j].Time, clamped: true);
                Cinders[j].Scale *= MathHelper.Lerp(0.6f, 0.9f, Cinders[j].IdentityIndex % 6f / 6f);
                if (Cinders[j].IdentityIndex % 13 == 12)
                {
                    Cinders[j].Scale *= 2f;
                }

                /*float num4 = MathHelper.Lerp(3.2f, 14f, (float)Cinders[j].IdentityIndex % 21f / 21f);
                Vector2 vector = -Vector2.UnitY.RotatedBy(MathHelper.Lerp(-0.44f, 0.44f, (float)Math.Sin((float)Cinders[j].Time / 16f + (float)Cinders[j].IdentityIndex) * 0.5f + 0.5f));
                vector = (vector + Vector2.UnitX).SafeNormalize(Vector2.UnitY) * num4;
                float amount = MathHelper.Lerp(0.01f, 0.08f, Utils.GetLerpValue(45f, 145f, Cinders[j].Time, clamped: true));
                Cinders[j].Velocity = Vector2.Lerp(Cinders[j].Velocity, vector, amount);*/

                Cinders[j].Velocity *= 1.01f;
                Cinders[j].Time++;
                Cinders[j].Center += Cinders[j].Velocity;
            }

            Cinders.RemoveAll((c) => c.Time >= c.Lifetime);
            Texture2D value2 = ModContent.Request<Texture2D>("CalamityMod/Skies/CalamitasCinder").Value;
            for (int k = 0; k < Cinders.Count; k++)
            {
                Vector2 center = Cinders[k].Center;
                spriteBatch.Draw(value2, center, null, Cinders[k].DrawColor, 0f, value2.Size() * 0.5f, Cinders[k].Scale, SpriteEffects.None, 0f);
            }

            drawColor = Color.White;
            Main.time = 27000.0;
            Main.dayTime = true;
            Vector2 position = new Vector2(Main.screenWidth / 2f, 100f);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            //spriteBatch.Draw(ModContent.Request<Texture2D>("Clamity/Content/Menu/Logo_Back").Value, position, null, Color.Lerp(Color.Blue, Color.Cyan, MathF.Sin(Main.GlobalTimeWrappedHourly) / 2 + 0.5f), logoRotation, Logo.Value.Size() * 0.5f, logoScale * (1.01f + MathF.Sin(Main.GlobalTimeWrappedHourly * 2 / 3) / 100), SpriteEffects.None, 0f);
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("Clamity/Content/Menu/Logo_Back").Value,
                                 position + Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 8 * i) * (1.01f + MathF.Sin(Main.GlobalTimeWrappedHourly * 4 / 3)) * 2,
                                 null, Color.Lerp(Color.LightBlue, Color.Cyan, MathF.Sin(Main.GlobalTimeWrappedHourly) / 2 + 0.5f),
                                 logoRotation, Logo.Value.Size() * 0.5f, logoScale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(Logo.Value, position, null, drawColor, logoRotation, Logo.Value.Size() * 0.5f, logoScale, SpriteEffects.None, 0f);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            return false;
            static Color selectCinderColor()
            {
                if (Main.rand.NextBool(3))
                {
                    return Color.Lerp(Color.DarkGray, Color.LightGray, Main.rand.NextFloat());
                }

                return Color.Lerp(Color.Blue, Color.Cyan, Main.rand.NextFloat(0.9f));
            }
        }
        //public override int Music => MusicLoader.GetMusicSlot("Clamity/Sounds/Music/Title");
        public override int Music => Clamity.mod.GetMusicFromMusicMod("Title") ?? MusicID.Title;
    }
}
