/*using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion.Items
{
    public class OneshootRottenEgg : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.RottenEgg);
            Item.shoot = ModContent.ProjectileType<OneshootRottenEggProj>();
        }
        public void DrawBackAfterimage(SpriteBatch spriteBatch, Vector2 baseDrawPosition, Rectangle frame, float baseScale)
        {
            if (Item.velocity.X != 0f)
                return;

            float pulse = (float)Math.Cos(1.61803398875f * Main.GlobalTimeWrappedHourly * 2f) + (float)Math.Cos(Math.E * Main.GlobalTimeWrappedHourly * 1.7f);
            pulse = pulse * 0.25f + 0.5f;

            // Sharpen the pulse with a power to give erratic fire bursts.
            pulse = (float)Math.Pow(pulse, 3D);

            float outwardnessFactor = MathHelper.Lerp(-0.3f, 1.2f, pulse);
            Color drawColor = Color.Lerp(Color.Blue, Color.Purple, pulse);
            drawColor *= MathHelper.Lerp(0.35f, 0.67f, CalamityUtils.Convert01To010(pulse));
            drawColor.A = 25;
            float drawPositionOffset = outwardnessFactor * baseScale * 8f;
            for (int i = 0; i < 8; i++)
            {
                Vector2 drawPosition = baseDrawPosition + (MathHelper.TwoPi * i / 8f).ToRotationVector2() * drawPositionOffset;
                spriteBatch.Draw(TextureAssets.Item[Item.type].Value, drawPosition, frame, drawColor, 0f, Vector2.Zero, baseScale, SpriteEffects.None, 0f);
            }
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Rectangle frame = TextureAssets.Item[Item.type].Value.Frame();
            DrawBackAfterimage(spriteBatch, Item.position - Main.screenPosition, frame, scale);
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.velocity.X = 0f;
            DrawBackAfterimage(spriteBatch, position - frame.Size() * 0.25f, frame, scale);
            return true;
        }
    }
    public class OneshootRottenEggProj : ModProjectile
    {
        //public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RottenEgg;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RottenEgg);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.friendly)
            {
                modifiers.SetInstantKill();
            }
        }
    }
}*/
