using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.Drop
{
    public class LargeFather : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Tools";

        public override void SetDefaults()
        {
            Item.damage = 465;
            Item.ArmorPenetration = 5;
            Item.knockBack = 0f;
            Item.useTime = 1;
            Item.useAnimation = 25;
            Item.pick = 280;
            Item.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
            Item.width = 62;
            Item.height = 36;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LargeFatherProj>();
            Item.shootSpeed = 40f;
            Item.tileBoost = 15;
        }

        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
        }
    }
    public class LargeFatherProj : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<LargeFather>().Texture;


        public static Asset<Texture2D> GlowmaskTex;

        public static Asset<Texture2D> BloomTex;

        internal PrimitiveTrail TrailDrawer;

        public override LocalizedText DisplayName => CalamityUtils.GetItemName<LargeFather>();

        public Player Owner => Main.player[base.Projectile.owner];

        public ref float MoveInIntervals => ref base.Projectile.localAI[0];

        public ref float SpeenBeams => ref base.Projectile.localAI[1];

        public ref float Timer => ref base.Projectile.ai[0];

        public override void SetDefaults()
        {
            base.Projectile.width = 14;
            base.Projectile.height = 14;
            base.Projectile.friendly = true;
            base.Projectile.penetrate = -1;
            base.Projectile.tileCollide = false;
            base.Projectile.hide = true;
            base.Projectile.ownerHitCheck = true;
            base.Projectile.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            Timer += 1f;
            SpeenBeams += ((Timer > 140f) ? 1f : (1f + 2f * (float)Math.Pow(1f - Timer / 140f, 2.0)));
            if (base.Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(in MarniteObliterator.UseSound, base.Projectile.Center);
                base.Projectile.soundDelay = 23;
            }

            if ((Owner.Center - base.Projectile.Center).Length() >= 5f)
            {
                if ((Owner.MountedCenter - base.Projectile.Center).Length() >= 30f)
                {
                    DelegateMethods.v3_1 = Color.Blue.ToVector3() * 0.5f;
                    Utils.PlotTileLine(Owner.MountedCenter + Owner.MountedCenter.DirectionTo(base.Projectile.Center) * 30f, base.Projectile.Center, 8f, DelegateMethods.CastLightOpen);
                }

                Lighting.AddLight(base.Projectile.Center, Color.Blue.ToVector3() * 0.7f);
            }

            if (MoveInIntervals > 0f)
            {
                MoveInIntervals -= 1f;
            }

            if (!Owner.channel || Owner.noItems || Owner.CCed)
            {
                base.Projectile.Kill();
            }
            else if (MoveInIntervals <= 0f && Main.myPlayer == base.Projectile.owner)
            {
                Vector2 vector = Owner.Calamity().mouseWorld - Owner.MountedCenter;
                if (Main.tile[Player.tileTargetX, Player.tileTargetY].HasTile)
                {
                    vector = new Vector2(Player.tileTargetX, Player.tileTargetY) * 16f + Vector2.One * 8f - Owner.MountedCenter;
                    MoveInIntervals = 2f;
                }

                vector = Vector2.Lerp(vector, base.Projectile.velocity, 0.7f);
                if (float.IsNaN(vector.X) || float.IsNaN(vector.Y))
                {
                    vector = -Vector2.UnitY;
                }

                if (vector.Length() < 50f)
                {
                    vector = vector.SafeNormalize(-Vector2.UnitY) * 50f;
                }

                int tileBoost = Owner.inventory[Owner.selectedItem].tileBoost;
                int num = (Player.tileRangeX + tileBoost - 1) * 16 + 11;
                int num2 = (Player.tileRangeY + tileBoost - 1) * 16 + 11;
                vector.X = Math.Clamp(vector.X, -num, num);
                vector.Y = Math.Clamp(vector.Y, -num2, num2);
                if (vector != base.Projectile.velocity)
                {
                    base.Projectile.netUpdate = true;
                }

                base.Projectile.velocity = vector;
            }

            Owner.heldProj = base.Projectile.whoAmI;
            Owner.ChangeDir(Math.Sign(base.Projectile.velocity.X));
            Owner.SetCompositeArmFront(enabled: true, Player.CompositeArmStretchAmount.Full, base.Projectile.velocity.ToRotation() * Owner.gravDir - MathF.PI / 2f);
            Owner.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.Full, base.Projectile.velocity.ToRotation() * Owner.gravDir - MathF.PI / 2f - MathF.PI / 8f * (float)Owner.direction);
            Owner.SetDummyItemTime(2);
            base.Projectile.rotation = base.Projectile.velocity.ToRotation();
            base.Projectile.Center = Owner.MountedCenter + base.Projectile.velocity;
        }

        internal Color ColorFunction(float completionRatio)
        {
            float num = (float)Math.Sqrt(1f - completionRatio);
            return Color.DeepSkyBlue * num;
        }

        internal float WidthFunction(float completionRatio)
        {
            return 29.4f * completionRatio;
        }

        public void DrawBeam(Texture2D beamTex, Vector2 direction, int beamIndex)
        {
            Vector2 startPos = Owner.MountedCenter + direction * 17f + direction.RotatedBy(1.5707963705062866) * (float)Math.Cos(MathF.PI * 2f * (float)beamIndex / 3f + SpeenBeams * 0.06f) * 13f;
            float rotation = (base.Projectile.Center - startPos).ToRotation();
            Vector2 beamOrigin = new Vector2((float)beamTex.Width / 2f, beamTex.Height);
            Vector2 beamScale = new Vector2(5.4f, (startPos - base.Projectile.Center).Length() / (float)beamTex.Height);
            CalamityUtils.DrawChromaticAberration(direction.RotatedBy(1.5707963705062866), 4f, delegate (Vector2 offset, Color colorMod)
            {
                Color firstColor = Color.Lerp(Color.Magenta, Color.DarkGoldenrod, 0.5f + 0.5f * (float)Math.Sin(SpeenBeams * 0.2f));
                firstColor *= 0.54f;
                firstColor = firstColor.MultiplyRGB(colorMod);
                Main.EntitySpriteDraw(beamTex, startPos + offset - Main.screenPosition, null, firstColor, rotation + MathF.PI / 2f, beamOrigin, beamScale, SpriteEffects.None);
                beamScale.X = 2.4f;
                firstColor = Color.Lerp(Color.Purple, Color.Chocolate, 0.5f + 0.5f * (float)Math.Sin(SpeenBeams * 0.2f + 1.2f));
                firstColor = firstColor.MultiplyRGB(colorMod);
                Main.EntitySpriteDraw(beamTex, startPos + offset - Main.screenPosition, null, firstColor, rotation + MathF.PI / 2f, beamOrigin, beamScale, SpriteEffects.None);
            });
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!base.Projectile.active)
            {
                return false;
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Vector2 vector = base.Projectile.velocity.SafeNormalize(Vector2.Zero);
            Texture2D value = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/SimpleGradient").Value;
            for (int i = 0; i < 3; i++)
            {
                if ((float)Math.Sin(MathF.PI * 2f * (float)i / 3f + SpeenBeams * 0.06f) < 0f)
                {
                    DrawBeam(value, vector, i);
                }
            }

            if (BloomTex == null)
            {
                BloomTex = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle");
            }

            Texture2D value2 = BloomTex.Value;
            Main.EntitySpriteDraw(value2, base.Projectile.Center - Main.screenPosition, null, Color.DeepSkyBlue * 0.3f, MathF.PI / 2f, value2.Size() / 2f, 0.3f * base.Projectile.scale, SpriteEffects.None);
            if (TrailDrawer == null)
            {
                TrailDrawer = new PrimitiveTrail(WidthFunction, ColorFunction, null, GameShaders.Misc["CalamityMod:TrailStreak"]);
            }

            GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/DoubleTrail"));
            TrailDrawer.Draw(new Vector2[2]
            {
                base.Projectile.Center,
                Owner.MountedCenter - vector * 13f
            }, -Main.screenPosition, 30);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D value3 = TextureAssets.Projectile[base.Projectile.type].Value;
            Vector2 origin = new Vector2(9f, (float)value3.Height / 2f);
            SpriteEffects effects = SpriteEffects.None;
            if ((float)Owner.direction * Owner.gravDir < 0f)
            {
                effects = SpriteEffects.FlipVertically;
            }

            Main.EntitySpriteDraw(value3, Owner.MountedCenter + vector * 10f - Main.screenPosition, null, base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, origin, base.Projectile.scale, effects);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            /*if (GlowmaskTex == null)
            {
                GlowmaskTex = ModContent.Request<Texture2D>("CalamityMod/Items/Tools/MarniteObliteratorBloom");
            }

            Texture2D value4 = GlowmaskTex.Value;
            float num = (float)Math.Pow(Math.Clamp(Timer / 100f, 0f, 1f), 2.0) * (0.85f + (0.5f + 0.5f * (float)Math.Sin(Main.GlobalTimeWrappedHourly))) * 0.8f;
            Main.EntitySpriteDraw(color: Color.Lerp(Color.DeepSkyBlue, Color.Chocolate, 0.5f + 0.5f * (float)Math.Sin(SpeenBeams * 0.2f + 1.2f)) * num, texture: value4, position: Owner.MountedCenter + vector * 10f - Main.screenPosition, sourceRectangle: null, rotation: base.Projectile.rotation, origin: origin, scale: base.Projectile.scale, effects: effects);
            */
            for (int j = 0; j < 3; j++)
            {
                if ((float)Math.Sin(MathF.PI * 2f * (float)j / 3f + SpeenBeams * 0.06f) >= 0f)
                {
                    DrawBeam(value, vector, j);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
