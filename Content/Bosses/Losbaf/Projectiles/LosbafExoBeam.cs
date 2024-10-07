using CalamityMod;
using CalamityMod.Graphics.Primitives;
using Clamity.Content.Bosses.Losbaf.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf.Projectiles
{
    public class LosbafExoBeam : ExoScythe
    {
        public static float MaxWidth = 30;
        public static Asset<Texture2D> BloomTex;
        public static Asset<Texture2D> SlashTex;
        public static Asset<Texture2D> TrailTex;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.alpha = 255;
        }

        /*public override void OnSpawn(IEntitySource source)
        {
            //Color color3 = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 0.5f + Projectile.whoAmI * 0.12f) % 1, Color.Cyan, Color.Lime, Color.GreenYellow, Color.Goldenrod, Color.Orange);
            //GeneralParticleHandler.SpawnParticle(new BloomLineVFX(Projectile.Center, -Vector2.UnitY, 1f, color3, (int)LosbafSuperboss.DuratationBetweenDownfallScytheAttack));
        }*/

        public override void AI()
        {
            Projectile.rotation -= 0.25f;
            base.AI();
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool())
            {
                Color dustColor = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.9f);
                Dust must = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(20f, 20f) + Projectile.velocity, 267, Projectile.velocity * -2.6f, 0, dustColor);
                must.scale = 0.3f;
                must.fadeIn = Main.rand.NextFloat() * 1.2f;
                must.noGravity = true;
            }

        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 2 && Projectile.ai[1] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 2) / 3, ModContent.ProjectileType<LosbafExoBeam>(), Projectile.damage, 0, Main.myPlayer, 2, 2);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 2 + MathHelper.PiOver4) / 3, ModContent.ProjectileType<LosbafExoBeam>(), Projectile.damage, 0, Main.myPlayer, 2, 2);

            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.White with { A = 0 } * Projectile.Opacity;

        public float TrailWidth(float completionRatio)
        {
            float width = Utils.GetLerpValue(1f, 0.4f, completionRatio, true) * (float)Math.Sin(Math.Acos(1 - Utils.GetLerpValue(0f, 0.15f, completionRatio, true)));

            width *= Utils.GetLerpValue(0f, 0.1f, Projectile.timeLeft / 600f, true);

            return width * MaxWidth;
        }
        public Color TrailColor(float completionRatio)
        {
            Color baseColor = Color.Lerp(Color.Cyan, new Color(0, 0, 255), completionRatio);

            return baseColor;
        }

        public float MiniTrailWidth(float completionRatio) => TrailWidth(completionRatio) * 0.8f;
        public Color MiniTrailColor(float completionRatio) => Color.White;


        public override bool PreDraw(ref Color lightColor)
        {
            //if (Projectile.timeLeft > 595)
            //    return false;



            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (Projectile.ai[0] == 1 && Projectile.timeLeft > 1000 - LosbafSuperboss.DuratationBetweenDownfallScytheAttack)
            {
                SpriteEffects effects = SpriteEffects.None;
                Texture2D value6 = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomLine").Value;
                Color color3 = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 0.5f + Projectile.whoAmI * 0.12f) % 1, Color.Cyan, Color.Lime, Color.GreenYellow, Color.Goldenrod, Color.Orange);
                Main.spriteBatch.Draw(value6,
                                 Projectile.Center - Main.screenPosition,
                                 null,
                                 color3,
                                 MathHelper.Pi,
                                 new Vector2((float)value6.Width / 2f, value6.Height),
                                 new Vector2(1f * (1000 - Projectile.timeLeft) / LosbafSuperboss.DuratationBetweenDownfallScytheAttack, 4200f),
                                 effects,
                                 0f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);




            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            float bladeScale = Utils.GetLerpValue(3f, 13f, Projectile.velocity.Length(), true) * 1.2f;

            //Draw the blade.
            Main.EntitySpriteDraw(texture, Projectile.oldPos[2] + Projectile.Size / 2f - Main.screenPosition, null, Color.White with { A = 0 }, Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2f, bladeScale * Projectile.scale, 0, 0);

            if (BloomTex == null)
                BloomTex = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle");
            Texture2D bloomTex = BloomTex.Value;

            Color mainColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 0.5f + Projectile.whoAmI * 0.12f) % 1, Color.Cyan, Color.Lime, Color.GreenYellow, Color.Goldenrod, Color.Orange);
            Color secondaryColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 0.5f + Projectile.whoAmI * 0.12f + 0.2f) % 1, Color.Cyan, Color.Lime, Color.GreenYellow, Color.Goldenrod, Color.Orange);

            //Draw the bloom unde the trail
            Main.EntitySpriteDraw(bloomTex, Projectile.oldPos[2] + Projectile.Size / 2f - Main.screenPosition, null, (mainColor * 0.1f) with { A = 0 }, 0, bloomTex.Size() / 2f, 1.3f * Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(bloomTex, Projectile.oldPos[1] + Projectile.Size / 2f - Main.screenPosition, null, (mainColor * 0.5f) with { A = 0 }, 0, bloomTex.Size() / 2f, 0.34f * Projectile.scale, 0, 0);

            Main.spriteBatch.EnterShaderRegion();

            if (TrailTex == null)
                TrailTex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/BasicTrail");

            GameShaders.Misc["CalamityMod:ExobladePierce"].SetShaderTexture(TrailTex);
            GameShaders.Misc["CalamityMod:ExobladePierce"].UseImage2("Images/Extra_189");
            GameShaders.Misc["CalamityMod:ExobladePierce"].UseColor(mainColor);
            GameShaders.Misc["CalamityMod:ExobladePierce"].UseSecondaryColor(secondaryColor);
            GameShaders.Misc["CalamityMod:ExobladePierce"].Apply();

            GameShaders.Misc["CalamityMod:ExobladePierce"].Apply();

            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(TrailWidth, TrailColor, (_) => Projectile.Size * 0.5f, shader: GameShaders.Misc["CalamityMod:ExobladePierce"]), 30);

            GameShaders.Misc["CalamityMod:ExobladePierce"].UseColor(Color.White);
            GameShaders.Misc["CalamityMod:ExobladePierce"].UseSecondaryColor(Color.White);

            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(MiniTrailWidth, MiniTrailColor, (_) => Projectile.Size * 0.5f, shader: GameShaders.Misc["CalamityMod:ExobladePierce"]), 30);

            Main.spriteBatch.ExitShaderRegion();

            //Draw the bloom above the trail
            Main.EntitySpriteDraw(bloomTex, Projectile.oldPos[2] + Projectile.Size / 2f - Main.screenPosition, null, (Color.White * 0.2f) with { A = 0 }, 0, bloomTex.Size() / 2f, 0.78f * Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(bloomTex, Projectile.oldPos[1] + Projectile.Size / 2f - Main.screenPosition, null, (Color.White * 0.5f) with { A = 0 }, 0, bloomTex.Size() / 2f, 0.2f * Projectile.scale, 0, 0);
            return false;
        }
    }
}
