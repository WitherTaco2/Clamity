using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Profusion.Projectiles
{
    public class MushroomThorn : ModProjectile
    {
        //private List<int> skins;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 80;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 22;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.scale = 2f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = Main.rand.Next(3);
            Projectile.ai[1] = Main.rand.NextFloat(0, 0.3f);
        }
        public override void AI()
        {
            switch (Projectile.ai[0])
            {
                case 0:
                    Projectile.velocity.RotatedBy(Projectile.ai[1]);
                    break;
                case 1:
                    Projectile.velocity.RotatedBy(-Projectile.ai[1]);
                    break;
                case 2:
                    Projectile.velocity.RotatedBy(MathF.Sin(Projectile.timeLeft) / (10 * Projectile.ai[1]));
                    break;
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            //Rectangle rect = new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 oldPosition = Projectile.oldPos[i];
                //Vector2 oldPosition = Projectile.oldPos[i] + vector + new Vector2(0f, Projectile.gfxOffY);

                Rectangle rect = new((int)oldPosition.X, (int)oldPosition.Y, 10, 10);
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.life > 0 && npc.Hitbox.Intersects(rect))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesFromEdge(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, ModContent.Request<Texture2D>(Texture + "Afterimages").Value);

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            int num = texture.Height / Main.projFrames[Projectile.type];
            int y = num * Projectile.frame;
            float scale = Projectile.scale;
            float rotation = Projectile.rotation;
            Rectangle value = new Rectangle(0, y, texture.Width, num);
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 vector = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Texture2D texture2 = ModContent.Request<Texture2D>(Texture + "Afterimages").Value;
                float rotation2 = Projectile.oldRot[i];
                int num1 = texture2.Height / 2;
                int y1 = num1 * (i % 4 == 0 ? 1 : 0);
                Rectangle rect = new Rectangle(0, y1, texture2.Width, num1);
                SpriteEffects effects2 = ((Projectile.oldSpriteDirection[i] == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                Vector2 position = Projectile.oldPos[i] + vector - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                //Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture2, position, rect, Color.White, rotation2, vector, scale, effects2, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center, null, Color.White, rotation, vector, scale, effects, 1);

            return false;
        }
    }
}
