using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.Localization;
using System;
using System.Collections.Generic;
using CalamityMod.Particles;
using static Humanizer.In;
using Terraria.Audio;
using Terraria.ID;

namespace Clamity.Content.Items.Weapons.Melee
{
    public class RGBMurasama : Murasama
    {
        private string GetHashCodeNew(Color color)
        {
            string[] list = new string[16]
            {
                "0", "1", "2", "3",
                "4", "5", "6", "7",
                "8", "9", "A", "B",
                "C", "D", "E", "F"
            };
            int[] list2 = new int[3] { color.R, color.G, color.B };
            string hash = "";
            foreach (int i in list2)
            {
                hash += list[i / 16] + list[i % 16];
            }
            //Main.NewText(hash);
            return hash;
        }
        //public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(GetHashCodeNew(Main.DiscoColor));
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindLastIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Tooltip3"));
            if (index != -1)
            {
                //tooltips[index].Text = LangHelper.GetText("Items.Weapons.Melee.RGBMurasama.Tooltip", right, potionList, favotite);
                tooltips[index].Text = "[c/" + GetHashCodeNew(Main.DiscoColor) + ":" + tooltips[index].Text + "]";
            }
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<RGBMurasamaProjectile>();
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (IDUnlocked(Main.LocalPlayer))
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                spriteBatch.Draw(value, position, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13), Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
                value = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
                spriteBatch.Draw(value, position, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13), Main.DiscoColor, 0f, origin, scale, SpriteEffects.None, 0f);
            }
            else
            {
                Texture2D value = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/MurasamaSheathed").Value;
                spriteBatch.Draw(value, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            }

            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (IDUnlocked(Main.LocalPlayer))
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                spriteBatch.Draw(value, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                value = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
                spriteBatch.Draw(value, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13), Main.DiscoColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Texture2D value = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/MurasamaSheathed").Value;
                spriteBatch.Draw(value, Item.position - Main.screenPosition, null, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (IDUnlocked(Main.LocalPlayer))
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
                spriteBatch.Draw(value, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13, frameCounterUp: false), Main.DiscoColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Murasama>()
                .AddIngredient(ItemID.RainbowBrick, 50)
                .AddIngredient<CoreofCalamity>(1)
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
    public class RGBMurasamaProjectile : MurasamaSlash
    {

        private Player Owner => Main.player[base.Projectile.owner];
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.frameCounter <= 1)
            {
                return false;
            }

            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = value.Size() / new Vector2(2f, 7f) * 0.5f;
            Rectangle value2 = value.Frame(2, 7, frameX, frameY);
            Main.EntitySpriteDraw(effects: Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, texture: value, position: Projectile.Center - Main.screenPosition, sourceRectangle: value2, color: Color.White, rotation: Projectile.rotation, origin: origin, scale: Projectile.scale);
            value = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Main.EntitySpriteDraw(effects: Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, texture: value, position: Projectile.Center - Main.screenPosition, sourceRectangle: value2, color: Main.DiscoColor, rotation: Projectile.rotation, origin: origin, scale: Projectile.scale);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.Organic())
            {
                SoundStyle style = Murasama.OrganicHit;
                style.Pitch = ((CurrentFrame == 0) ? (-0.1f) : ((CurrentFrame == 6) ? 0.1f : ((CurrentFrame == 10) ? (-0.15f) : 0f)));
                SoundEngine.PlaySound(in style, base.Projectile.Center);
            }
            else
            {
                SoundStyle style = Murasama.InorganicHit;
                style.Pitch = ((CurrentFrame == 0) ? (-0.1f) : ((CurrentFrame == 6) ? 0.1f : ((CurrentFrame == 10) ? (-0.15f) : 0f)));
                SoundEngine.PlaySound(in style, base.Projectile.Center);
            }

            for (int i = 0; i < 3; i++)
            {
                Color color = ((CurrentFrame != 6) ? (Main.rand.NextBool(4) ? Color.LightCoral : Color.Crimson) : (Main.rand.NextBool(3) ? Color.LightCoral : Color.White));
                float num = Main.rand.NextFloat(1f, 1.75f);
                if (CurrentFrame == 6)
                {
                    GeneralParticleHandler.SpawnParticle(new SparkleParticle(target.Center + Main.rand.NextVector2Circular((float)target.width * 0.75f, (float)target.height * 0.75f), Vector2.Zero, Color.White, Main.DiscoColor, num * 1.2f, 8, 0f, 4.5f));
                }

                GeneralParticleHandler.SpawnParticle(new SparkleParticle(target.Center + Main.rand.NextVector2Circular((float)target.width * 0.75f, (float)target.height * 0.75f), Vector2.Zero, color, Main.DiscoColor, num, 8, 0f, 2.5f));
            }

            float num2 = MathHelper.Clamp((CurrentFrame == 6) ? (18 - base.Projectile.numHits * 3) : (5 - base.Projectile.numHits * 2), 0f, 18f);
            for (int j = 0; (float)j < num2; j++)
            {
                Vector2 vector = base.Projectile.velocity.RotatedBy((CurrentFrame == 0) ? (-0.45f * (float)Owner.direction) : ((CurrentFrame == 6) ? 0f : ((CurrentFrame == 10) ? (0.45f * (float)Owner.direction) : 0f))).RotatedByRandom(0.34999999403953552) * Main.rand.NextFloat(0.5f, 1.8f);
                int num3 = Main.rand.Next(23, 35);
                float num4 = Main.rand.NextFloat(0.95f, 1.8f);
                if (Main.rand.NextBool())
                {
                    GeneralParticleHandler.SpawnParticle(new AltSparkParticle(target.Center + Main.rand.NextVector2Circular((float)target.width * 0.5f, (float)target.height * 0.5f) + base.Projectile.velocity * 1.2f, vector * ((CurrentFrame == 6) ? 1f : 0.65f), affectedByGravity: false, (int)((float)num3 * ((CurrentFrame == 6) ? 1.2f : 1f)), num4 * ((CurrentFrame == 6) ? 1.4f : 1f), Main.DiscoColor));
                }
                else
                {
                    GeneralParticleHandler.SpawnParticle(new LineParticle(target.Center + Main.rand.NextVector2Circular((float)target.width * 0.5f, (float)target.height * 0.5f) + base.Projectile.velocity * 1.2f, vector * ((CurrentFrame == 7) ? 1f : 0.65f), affectedByGravity: false, (int)((float)num3 * ((CurrentFrame == 7) ? 1.2f : 1f)), num4 * ((CurrentFrame == 7) ? 1.4f : 1f), Main.DiscoColor));
                }
            }

            float num5 = MathHelper.Clamp((CurrentFrame == 6) ? (25 - base.Projectile.numHits * 3) : (12 - base.Projectile.numHits * 2), 0f, 25f);
            for (int k = 0; (float)k <= num5; k++)
            {
                int type = (Main.rand.NextBool(3) ? 182 : (Main.rand.NextBool() ? ((CurrentFrame == 6) ? 309 : 296) : 90));
                Dust dust = Dust.NewDustPerfect(target.Center + Main.rand.NextVector2Circular((float)target.width * 0.5f, (float)target.height * 0.5f), type, base.Projectile.velocity.RotatedBy((CurrentFrame == 0) ? (-0.45f * (float)Owner.direction) : ((CurrentFrame == 6) ? 0f : ((CurrentFrame == 10) ? (0.45f * (float)Owner.direction) : 0f))).RotatedByRandom(0.550000011920929) * Main.rand.NextFloat(0.3f, 1.1f));
                dust.scale = Main.rand.NextFloat(0.9f, 2.4f);
                dust.noGravity = true;
                dust.color = Main.DiscoColor;
            }
        }
    }
}
