using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.WoB.Projectiles
{
    public class WallOfBronzeClawProjectile : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public ref float ClawGun => ref Projectile.ai[0];
        public Vector2 StartVelocity
        {
            get => new Vector2(Projectile.ai[1], Projectile.ai[2]);
            set
            {
                Projectile.ai[1] = value.X;
                Projectile.ai[2] = value.Y;
            }
        }
        public ref float StartLength => ref Projectile.Clamity().extraAI[1];
        public Terraria.NPC GetClawGun => Main.npc[(int)ClawGun];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 2;
            Projectile.Calamity().DealsDefenseDamage = true;
        }
        public override void AI()
        {
            if (GetClawGun == null) Projectile.Kill();
            if (!GetClawGun.active) Projectile.Kill();


            //Main.NewText(GetClawGun.Center);
            //Main.player[Main.myPlayer].Center = Projectile.Center;

            if (Projectile.velocity != Vector2.Zero)
                Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.timeLeft <= 550)
                Projectile.tileCollide = true;
            if (Projectile.timeLeft == 100)
            {
                StartLength = (GetClawGun.Center - Projectile.position).Length();
            }
            if (Projectile.timeLeft < 100)
            {
                Projectile.velocity = Vector2.Normalize(GetClawGun.Center - Projectile.position) / 10f * StartLength;
                Projectile.tileCollide = false;
                if (Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Hitbox.BottomRight(), GetClawGun.position, GetClawGun.Hitbox.BottomRight()))
                    Projectile.Kill();
            }
            //Terraria.Collision.CheckAABBvAABBCollision
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft >= 100)
            {
                Projectile.velocity = Vector2.Zero;
                //Projectile.ai[1] = 1;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 mountedCenter = GetClawGun.Center;
            Texture2D value = ModContent.Request<Texture2D>(Texture + "_Chain").Value;
            Vector2 center = Projectile.Center;
            Rectangle? sourceRectangle = null;
            Vector2 origin = new Vector2(value.Width * 0.5f, value.Height * 0.5f);
            float num = value.Height;
            Vector2 vector = mountedCenter - center;
            float rotation = (float)Math.Atan2(vector.Y, vector.X) - MathF.PI / 2f;
            bool flag = true;
            if (float.IsNaN(center.X) && float.IsNaN(center.Y))
            {
                flag = false;
            }

            if (float.IsNaN(vector.X) && float.IsNaN(vector.Y))
            {
                flag = false;
            }

            while (flag)
            {
                if (vector.Length() < num + 1f)
                {
                    flag = false;
                    continue;
                }

                Vector2 vector2 = vector;
                vector2.Normalize();
                center += vector2 * num;
                vector = mountedCenter - center;
                Color color = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16f));
                Main.spriteBatch.Draw(value, center - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
            }
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);

            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.getGoodWorld)
                target.AddBuff(BuffID.Frozen, 180);
        }
    }
}
