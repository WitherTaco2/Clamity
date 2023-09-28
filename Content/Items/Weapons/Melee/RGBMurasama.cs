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
            int index = tooltips.FindLastIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Tooltip2"));
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
                .AddIngredient<ExoPrism>(10)
                .AddIngredient<CoreofCalamity>(5)
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
    public class RGBMurasamaProjectile : MurasamaSlash
    {
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
    }
}
