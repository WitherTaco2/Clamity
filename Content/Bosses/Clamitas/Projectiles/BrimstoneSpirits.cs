using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Clamity.Content.Bosses.Clamitas.Projectiles
{
    public class BrimstoneSpirits : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 48;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.timeLeft = 600;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public ref float SpriteType => ref Projectile.ai[0];
        public override void AI()
        {

        }
        public override void OnSpawn(IEntitySource source)
        {
            WeightedRandom<int> typeDecider = new WeightedRandom<int>((int)Main.GlobalTimeWrappedHourly + Projectile.whoAmI);
            typeDecider.Add(ModContent.ProjectileType<RedirectingLostSoul>(), 0.75f);
            typeDecider.Add(ModContent.ProjectileType<RedirectingVengefulSoul>(), 0.4f);
            typeDecider.Add(ModContent.ProjectileType<RedirectingGildedSoul>(), 0.2f);
            SpriteType = typeDecider.Get();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (SpriteType != -1)
            {
                Texture2D t = TextureAssets.Projectile[(int)SpriteType].Value;
                Rectangle frame = t.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
                SpriteEffects direction = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                for (int j = 0; j < Projectile.oldPos.Length / 2; j++)
                {
                    float fade = (float)Math.Pow(1f - Utils.GetLerpValue(0f, Projectile.oldPos.Length / 2, j, true), 2D);
                    Color drawColor = Projectile.GetAlpha(lightColor) * fade;
                    Vector2 drawPosition = Projectile.oldPos[j] + Projectile.Size * 0.5f - Main.screenPosition;
                    float rotation = Projectile.oldRot[j];

                    Main.EntitySpriteDraw(t, drawPosition, frame, drawColor, rotation, frame.Size() * 0.5f, Projectile.scale, direction, 0);
                }
                //Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, direction, 0);
            }
            return false;
        }
    }
}
