using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Ihor.Items
{
    public class JawOfTheFrostMeleeEffect : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => "Terraria/Images/Projectile_984";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0] = (float)player.direction * player.gravDir;
            float num = (player.itemAnimationMax - player.itemAnimation) / Projectile.ai[1];
            float num2 = Projectile.ai[0];
            float num3 = Projectile.velocity.ToRotation();
            float num4 = (float)Math.PI * num2 * num + num3 + num2 * (float)Math.PI + player.fullRotation;
            Projectile.rotation = num4;
            float num5 = 0.5f;
            float num6 = 1.0f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = num6 + num * num5;

            Projectile.scale *= Projectile.ai[2];
            if (Projectile.localAI[0] >= Projectile.ai[1])
                Projectile.Kill();

            // Enchantment visual bullshit
            Vector2 boxPosition = Projectile.position;
            int boxWidth = Projectile.width;
            int boxHeight = Projectile.height;
            for (float i = -MathHelper.PiOver4; i <= MathHelper.PiOver4; i += MathHelper.PiOver2)
            {
                Rectangle rect = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + i).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
                Projectile.EmitEnchantmentVisualsAt(rect.TopLeft(), rect.Width, rect.Height);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Texture2D asset = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = asset.Frame(1, 4);
            Vector2 origin = rectangle.Size() / 2f;
            float num = Projectile.scale * 1.1f;
            SpriteEffects effects = ((!(Projectile.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);
            float num2 = (player.itemAnimationMax - player.itemAnimation) / Projectile.ai[1];
            float num3 = Utils.Remap(num2, 0f, 0.6f, 0f, 1f) * Utils.Remap(num2, 0.6f, 1f, 1f, 0f);
            float num4 = 0.975f;
            float fromValue = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3D);
            fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
            Color color = Color.LightBlue * num2;
            Main.spriteBatch.Draw(asset, vector, rectangle, color * fromValue * num3, Projectile.rotation + Projectile.ai[0] * MathHelper.PiOver4 * -1f * (1f - num2), origin, num, effects, 0f);
            Color color2 = Color.Blue * num2;
            Color color3 = Color.Blue * num2;
            Color color4 = Color.White * num2;
            Color color5 = color4 * fromValue * 0.5f;
            Main.spriteBatch.Draw(asset, vector, rectangle, color5 * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, rectangle, color3 * fromValue * num3 * 0.3f, Projectile.rotation, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, rectangle, color2 * fromValue * num3 * 0.5f, Projectile.rotation, origin, num * num4, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, asset.Frame(1, 4, 0, 3), Color.Cyan * 0.6f * num3, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, asset.Frame(1, 4, 0, 3), Color.Cyan * 0.5f * num3, Projectile.rotation + Projectile.ai[0] * -0.05f, origin, num * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, asset.Frame(1, 4, 0, 3), Color.Cyan * 0.4f * num3, Projectile.rotation + Projectile.ai[0] * -0.1f, origin, num * 0.6f, effects, 0f);
            float num5 = num * 0.75f;
            Texture2D value = TextureAssets.Extra[98].Value;
            Color shineColor = color3 * Projectile.Opacity * 0.5f;
            Vector2 origin2 = value.Size() / 2f;
            float num9 = Utils.GetLerpValue(0f, 0.5f, num2, clamped: true) * Utils.GetLerpValue(1f, 0.5f, num2, clamped: true);
            Vector2 vector3 = new Vector2((Vector2.One * num5).X * 0.5f, (new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * num5).X) * num9;
            Vector2 vector2 = new Vector2((Vector2.One * num5).Y * 0.5f, (new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * num5).Y) * num9;
            shineColor *= num9;
            for (float num6 = 0f; num6 < 12f; num6 += 1f)
            {
                float num7 = Projectile.rotation + Projectile.ai[0] * num6 * -MathHelper.TwoPi * 0.025f + Utils.Remap(num2, 0f, 0.6f, 0f, 0.95504415f) * Projectile.ai[0];
                Vector2 drawpos = vector + num7.ToRotationVector2() * ((float)asset.Width * 0.5f - 6f) * num;
                float num8 = num6 / 12f;

                Color color6 = new Color(255, 255, 255, 0) * num3 * num8 * 0.5f;
                color6 *= num9;
                Main.EntitySpriteDraw(value, drawpos, null, shineColor, MathHelper.PiOver2 + num7, origin2, vector3, SpriteEffects.None);
                Main.EntitySpriteDraw(value, drawpos, null, shineColor, 0f + num7, origin2, vector2, SpriteEffects.None);
                Main.EntitySpriteDraw(value, drawpos, null, color6, MathHelper.PiOver2 + num7, origin2, vector3 * 0.6f, SpriteEffects.None);
                Main.EntitySpriteDraw(value, drawpos, null, color6, 0f + num7, origin2, vector2 * 0.6f, SpriteEffects.None);
            }

            Vector2 drawpos2 = vector + (Projectile.rotation + Utils.Remap(num2, 0f, 0.6f, 0f, 0.95504415f) * Projectile.ai[0]).ToRotationVector2() * ((float)asset.Width * 0.5f - 4f) * num;
            Color color7 = new Color(255, 255, 255, 0) * num3 * 0.5f;
            color7 *= num9;
            Main.EntitySpriteDraw(value, drawpos2, null, shineColor, MathHelper.PiOver2 + Projectile.rotation, origin2, vector3, SpriteEffects.None);
            Main.EntitySpriteDraw(value, drawpos2, null, shineColor, 0f + Projectile.rotation, origin2, vector2, SpriteEffects.None);
            Main.EntitySpriteDraw(value, drawpos2, null, color7, MathHelper.PiOver2 + Projectile.rotation, origin2, vector3 * 0.6f, SpriteEffects.None);
            Main.EntitySpriteDraw(value, drawpos2, null, color7, 0f + Projectile.rotation, origin2, vector2 * 0.6f, SpriteEffects.None);

            return false;
        }

        public override void CutTiles()
        {
            Vector2 startPoint = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            Vector2 endPoint = (Projectile.rotation + MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            float projectileSize = 60f * Projectile.scale;
            Utils.PlotTileLine(Projectile.Center + startPoint, Projectile.Center + endPoint, projectileSize, DelegateMethods.CutTiles);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : -1;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float coneLength = 94f * Projectile.scale;
            float scale = MathHelper.TwoPi / 25f * Projectile.ai[0];
            float maximumAngle = MathHelper.PiOver4;
            float coneRotation = Projectile.rotation + scale;
            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation, maximumAngle))
                return true;

            float rotation = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
            if (rotation > 0f)
            {
                float coneRotation2 = coneRotation - MathHelper.PiOver4 * Projectile.ai[0] * rotation;
                if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation2, maximumAngle))
                    return true;
            }

            return false;
        }
    }
}
