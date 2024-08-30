using CalamityMod.Skies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityMod.Skies.ExoMechsSky;

namespace Clamity.Content.Subworlds.Exosphere
{
    public class ExosphereSky : CustomSky
    {
        public float LightningIntensity;
        public float BackgroundIntensity;
        public List<ExoMechsSky.Lightning> LightningBolts = new List<ExoMechsSky.Lightning>();
        public static void CreateLightningBolt(int count = 1, bool playSound = false)
        {
            if (Main.netMode == 2)
                return;
            for (int index = 0; index < count; ++index)
            {
                ExoMechsSky.Lightning lightning = new ExoMechsSky.Lightning()
                {
                    Lifetime = 30,
                    Depth = Utils.NextFloat(Main.rand, 1.5f, 10f),
                    Position = new Vector2(Main.LocalPlayer.Center.X + Utils.NextFloatDirection(Main.rand) * 5000f, Utils.NextFloat(Main.rand, 4850f))
                };
                (((EffectManager<CustomSky>)SkyManager.Instance)["Clamity:Exosphere"] as ExosphereSky).LightningBolts.Add(lightning);
            }
            if (count >= 10)
            {
                (((EffectManager<CustomSky>)SkyManager.Instance)["Clamity:Exosphere"] as ExosphereSky).LightningIntensity = 1f;
                playSound = true;
            }
            if (!playSound || Main.gamePaused)
                return;
            SoundStyle thunder1 = SoundID.Thunder;
            thunder1.Volume *= 0.5f;
            SoundEngine.PlaySound(thunder1, Main.LocalPlayer.Center);
        }
        public override void Update(GameTime gameTime)
        {
            if (!SubworldSystem.IsActive<ExosphereSubworld>())
            {
                LightningIntensity = 0f;
                BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity - 0.08f, 0f, 1f);
                LightningBolts.Clear();
                Deactivate();
                return;
            }
            LightningIntensity = MathHelper.Clamp(LightningIntensity * 0.95f - 0.025f, 0.0f, 1f);

            for (int index = 0; index < LightningBolts.Count; ++index)
                --LightningBolts[index].Lifetime;

            if (Main.rand.NextBool(5))
                ExosphereSky.CreateLightningBolt();

            if (Main.rand.NextBool(25))
            {
                LightningIntensity = 1f;
                ExosphereSky.CreateLightningBolt(4);
                if (!Main.gamePaused)
                {
                    SoundStyle style = SoundID.Thunder with
                    {
                        Volume = SoundID.Thunder.Volume * 0.5f
                    };
                    SoundEngine.PlaySound(in style, Main.LocalPlayer.Center);
                }
            }
            Opacity = 1;
        }
        public override Color OnTileColor(Color inColor)
        {
            Color drawColor = ExoMechsSky.DrawColor;
            return new Color(Vector4.Lerp(drawColor.ToVector4(), inColor.ToVector4(), 1f - BackgroundIntensity));
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (!SubworldSystem.IsActive<ExosphereSubworld>())
                return;

            if (maxDepth >= float.MaxValue)
            {
                Vector2 scale = new Vector2((float)Main.screenWidth * 1.1f / (float)TextureAssets.MagicPixel.Value.Width, (float)Main.screenHeight * 1.1f / (float)TextureAssets.MagicPixel.Value.Height);
                Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
                Color color = Color.White * MathHelper.Lerp(0f, 0.24f, LightningIntensity) * BackgroundIntensity;
                Vector2 origin = TextureAssets.MagicPixel.Value.Size() * 0.5f;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, position, null, OnTileColor(Color.Transparent), 0f, origin, scale, SpriteEffects.None, 0f);
                for (int i = 0; i < 2; i++)
                {
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, position, null, color, 0f, origin, scale, SpriteEffects.None, 0f);
                }
            }

            Texture2D value = ModContent.Request<Texture2D>("Terraria/Images/Misc/VortexSky/Flash").Value;
            Texture2D value2 = ModContent.Request<Texture2D>("Terraria/Images/Misc/VortexSky/Bolt").Value;
            float num = Math.Min(1f, (Main.screenPosition.Y - 300f) / 300f);
            Vector2 vector = Main.screenPosition + new Vector2((float)Main.screenWidth * 0.5f, (float)Main.screenHeight * 0.5f);
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            LightningBolts.RemoveAll((Lightning l) => l.Lifetime <= 0);
            for (int j = 0; j < LightningBolts.Count; j++)
            {
                if (!(LightningBolts[j].Depth > minDepth) || !(LightningBolts[j].Depth < maxDepth))
                {
                    continue;
                }

                Vector2 vector2 = new Vector2(1f / LightningBolts[j].Depth, 0.9f / LightningBolts[j].Depth);
                Vector2 position2 = (LightningBolts[j].Position - vector) * vector2 + vector - Main.screenPosition;
                if (rectangle.Contains((int)position2.X, (int)position2.Y))
                {
                    Texture2D texture = value2;
                    int lifetime = LightningBolts[j].Lifetime;
                    if (lifetime > 24 && lifetime % 2 == 0)
                    {
                        texture = value;
                    }

                    float num2 = (float)lifetime * num / 20f;
                    spriteBatch.Draw(texture, position2, null, Color.White * num2, 0f, Vector2.Zero, vector2.X * 5f, SpriteEffects.None, 0f);
                }
            }
        }
        public override float GetCloudAlpha() => 0.0f;

        public override void Reset()
        {
        }

        public override void Activate(Vector2 position, params object[] args)
        {
        }

        public override void Deactivate(params object[] args)
        {
        }
        public override bool IsActive() => SubworldSystem.IsActive<ExosphereSubworld>() && !Main.gameMenu;
    }
    public class ExosphereFilter : ScreenShaderData
    {
        public ExosphereFilter(string passName)
            : base(passName)
        {
        }

        /*public override void Apply()
        {
            if (SubworldSystem.IsActive<ExosphereSubworld>())
            {
                UseTargetPosition(Main.npc[CalamityGlobalNPC.draedon].Center);
            }

            base.Apply();
        }*/

        /*public override void Update(GameTime gameTime)
        {
            if (!SubworldSystem.IsActive<ExosphereSubworld>())
            {
                Filters.Scene["Clamity:Exosphere"].Deactivate();
            }
        }*/
    }
}
