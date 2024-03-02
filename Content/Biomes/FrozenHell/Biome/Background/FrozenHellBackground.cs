﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.FrozenHell.Biome.Background
{
    public class FrozenHellBackground : ModSystem
    {
        public static float[] UWBGAlpha = new float[2];
        public static int UWBGStyle;
        public Color[] UWBGBottomColor = new Color[2];
        public Asset<Texture2D>[][] UWBGTexture = new Asset<Texture2D>[2][];

        public override void Load()
        {
            IL_Main.DrawBG += UWBGInsert;
            IL_Main.DrawCapture += UWBGInsertCapture;
        }

        public override void Unload()
        {
            IL_Main.DrawBG -= UWBGInsert;
            IL_Main.DrawCapture -= UWBGInsertCapture;
        }

        public override void PostSetupContent()
        {
            if (!Main.dedServ)
            {
                UWBGTexture[0] = new Asset<Texture2D>[14];
                UWBGTexture[1] = new Asset<Texture2D>[14];
                for (int i = 0; i < 14; i++)
                {
                    UWBGTexture[0][i] = TextureAssets.Underworld[i];
                    UWBGTexture[1][i] = ModContent.Request<Texture2D>("Clamity/Content/Biomes/FrozenHell/Biome/Background/Underworld " + i);
                }
                UWBGBottomColor[0] = new Color(11, 3, 7);
                UWBGBottomColor[1] = new Color(13, 13, 28);
            }
        }

        private void UWBGInsert(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdcI4(0), i => i.MatchCall<Main>("DrawUnderworldBackground"));
            c.EmitDelegate(() =>
            {
                DrawUnderworldBackground(false);
            });
        }

        private void UWBGInsertCapture(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdcI4(1), i => i.MatchCall<Main>("DrawUnderworldBackground"));
            c.EmitDelegate(() =>
            {
                DrawUnderworldBackground(true);
            });
        }

        public int UnderworldStyleCalc()
        {
            if (Main.LocalPlayer.InModBiome<FrozenHell>())
            {
                return 1;
            }
            return 0;
        }

        protected void DrawUnderworldBackground(bool flat)
        {
            if (!(Main.screenPosition.Y + Main.screenHeight < (Main.maxTilesY - 220) * 16f))
            {
                UWBGStyle = UnderworldStyleCalc();
                for (var i = 0; i < 2; i++)
                {
                    if (UWBGStyle != i)
                    {
                        UWBGAlpha[i] = Math.Max(UWBGAlpha[i] - 0.05f, 0f);
                    }
                    else
                    {
                        UWBGAlpha[i] = Math.Min(UWBGAlpha[i] + 0.05f, 1f);
                    }
                }
                Vector2 screenOffset = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
                float pushUp = (Main.GameViewMatrix.Zoom.Y - 1f) * 0.5f * 200f;
                SkyManager.Instance.ResetDepthTracker();
                for (int num = 4; num >= 0; num--)
                {
                    bool flag = false;
                    for (int j = 0; j < 2; j++)
                    {
                        if (UWBGAlpha[j] > 0f && j != UWBGStyle)
                        {
                            DrawUnderworldBackgroudLayer(flat, screenOffset, pushUp, num, j, flat ? 1f : UWBGAlpha[j]);
                            flag = true;
                        }
                    }
                    DrawUnderworldBackgroudLayer(flat, screenOffset, pushUp, num, UWBGStyle, flag ? UWBGAlpha[UWBGStyle] : 1f);
                }
                if (!Main.mapFullscreen)
                {
                    SkyManager.Instance.DrawRemainingDepth(Main.spriteBatch);
                }
                //Main.DrawSurfaceBG_DrawChangeOverlay(12);
            }
        }

        private void DrawUnderworldBackgroudLayer(bool flat, Vector2 screenOffset, float pushUp, int layerTextureIndex, int Style, float Alpha)
        {
            if (Style == 0)
            {
                return;
            }
            int num = Main.underworldBG[layerTextureIndex];
            Asset<Texture2D> asset = UWBGTexture[Style][num];
            if (!asset.IsLoaded)
            {
                Main.Assets.Request<Texture2D>(asset.Name);
            }
            Texture2D value = asset.Value;
            Vector2 vec = new Vector2(value.Width, value.Height) * 0.5f;
            float num7 = flat ? 1f : layerTextureIndex * 2 + 3f;
            Vector2 vector = new(1f / num7);
            Rectangle value2 = new(0, 0, value.Width, value.Height);
            float num8 = 1.3f;
            Vector2 zero = Vector2.Zero;
            int num9 = 0;
            switch (num)
            {
                case 1:
                    {
                        int num14 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                        value2 = new((num14 >> 1) * (value.Width >> 1), num14 % 2 * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                        vec *= 0.5f;
                        zero.Y += 175f;
                        break;
                    }
                case 2:
                    zero.Y += 100f;
                    break;
                case 3:
                    zero.Y += 75f;
                    break;
                case 4:
                    num8 = 0.5f;
                    zero.Y -= 0f;
                    break;
                case 5:
                    zero.Y += num9;
                    break;
                case 6:
                    {
                        int num13 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                        value2 = new(num13 % 2 * (value.Width >> 1), (num13 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                        vec *= 0.5f;
                        zero.Y += num9;
                        zero.Y += -60f;
                        break;
                    }
                case 7:
                    {
                        int num12 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                        value2 = new(num12 % 2 * (value.Width >> 1), (num12 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                        vec *= 0.5f;
                        zero.Y += num9;
                        zero.X -= 400f;
                        zero.Y += 90f;
                        break;
                    }
                case 8:
                    {
                        int num11 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                        value2 = new(num11 % 2 * (value.Width >> 1), (num11 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                        vec *= 0.5f;
                        zero.Y += num9;
                        zero.Y += 90f;
                        break;
                    }
                case 9:
                    zero.Y += num9;
                    zero.Y -= 30f;
                    break;
                case 10:
                    zero.Y += 250f * num7;
                    break;
                case 11:
                    zero.Y += 100f * num7;
                    break;
                case 12:
                    zero.Y += 20f * num7;
                    break;
                case 13:
                    {
                        zero.Y += 20f * num7;
                        int num10 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                        value2 = new(num10 % 2 * (value.Width >> 1), (num10 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                        vec *= 0.5f;
                        break;
                    }
            }
            if (flat)
            {
                num8 *= 1.5f;
            }
            vec *= num8;
            SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / vector.X);
            if (flat)
            {
                zero.Y += (UWBGTexture[Style][0].Height() >> 1) * 1.3f - vec.Y;
            }
            zero.Y -= pushUp;
            float num2 = num8 * value2.Width;
            int num3 = (int)((int)(screenOffset.X * vector.X - vec.X + zero.X - (Main.screenWidth >> 1)) / num2);
            vec = vec.Floor();
            int num4 = (int)Math.Ceiling(Main.screenWidth / num2);
            int num5 = (int)(num8 * ((value2.Width - 1) / vector.X));
            Vector2 vec2 = (new Vector2((num3 - 2) * num5, Main.UnderworldLayer * 16f) + vec - screenOffset) * vector + screenOffset - Main.screenPosition - vec + zero;
            vec2 = vec2.Floor();
            while (vec2.X + num2 < 0f)
            {
                num3++;
                vec2.X += num2;
            }
            for (int i = num3 - 2; i <= num3 + 4 + num4; i++)
            {
                Color color = Color.White;
                float num16 = color.R * Alpha;
                float num17 = color.G * Alpha;
                float num18 = color.B * Alpha;
                float num19 = color.A * Alpha;
                color = new((byte)num16, (byte)num17, (byte)num18, (byte)num19);

                Color color2 = UWBGBottomColor[Style];
                float num116 = color2.R * Alpha;
                float num117 = color2.G * Alpha;
                float num118 = color2.B * Alpha;
                float num119 = color2.A * Alpha;
                color2 = new((byte)num116, (byte)num117, (byte)num118, (byte)num119);

                Main.spriteBatch.Draw(value, vec2, (Rectangle?)value2, color, 0f, Vector2.Zero, num8, 0, 0f);
                if (layerTextureIndex == 0)
                {
                    int num6 = (int)(vec2.Y + value2.Height * num8);
                    Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)vec2.X, num6, (int)(value2.Width * num8), Math.Max(0, Main.screenHeight - num6)), color2);
                }
                vec2.X += num2;
            }
        }
    }
}
