using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Particles;

namespace Clamity.Content.Boss.Pyrogen.Projectiles
{
    public class FireBomb : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<FireBarrage>().Texture;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 690;
        }
        public bool Spawned
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        public override void AI()
        {
            //Projectile.velocity.X = MathF.Sqrt(MathF.Sqrt(Projectile.velocity.X));
            //Projectile.velocity.Y = MathF.Sqrt(MathF.Sqrt(Projectile.velocity.Y));
            //Projectile.rotation = Projectile.velocity.ToRotation()
            Projectile.velocity *= 0.85f;
            if (Projectile.velocity.Length() < 0.1f){
                if (!Spawned)
                {
                    Spawned = true;
                    Projectile.velocity = Vector2.Zero;
                    GeneralParticleHandler.SpawnParticle(new DirectionalPulseRing(Projectile.Center, Vector2.Zero, Color.Red, new Vector2(0.5f, 0.5f), Main.rand.NextFloat(12f, 25f), 2f, 2f, 60));
                    GeneralParticleHandler.SpawnParticle(new DirectionalPulseRing(Projectile.Center, Vector2.Zero, Color.Red, new Vector2(0.5f, 0.5f), Main.rand.NextFloat(12f, 25f), 0f, 2f, 60));
                }
                Projectile.ai[1]++;
                if (Projectile.ai[1] >= 60)
                {
                    Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Firethrower>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
        }
        public override bool? CanDamage() => false;
        /*public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FireBombExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }*/
    }
    public class FireBombExplosion : ModProjectile
    {
        public override string Texture => "Clamity/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.MaxUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 14;
            Projectile.scale = 0.2f;
            Projectile.hide = true;
        }
        public override void AI()
        {
            Projectile.scale *= 1.013f;
            Projectile.Opacity = Utils.GetLerpValue(5f, 36f, Projectile.timeLeft, clamped: true);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value2 = ModContent.Request<Texture2D>("CalamityMod/Skies/XerocLight").Value;
            //Rectangle rectangle = new(0, 0, 2, 2);
            Vector2 vector = Projectile.Center - Main.screenPosition;
            //Vector2 origin = rectangle.Size() * 0.5f;

            for (int i = 0; i < 36; i++)
            {
                Vector2 position = vector + (MathF.PI * 2f * i / 36f + Main.GlobalTimeWrappedHourly * 5f).ToRotationVector2() * Projectile.scale * 12f;
                Color value3 = CalamityUtils.MulticolorLerp(Projectile.timeLeft / 144f, Color.Orange, Color.Red);
                value3 = Color.Lerp(value3, Color.White, 0.4f) * Projectile.Opacity * 0.184f;
                Main.spriteBatch.Draw(value2, position, null, value3, 0f, value2.Size() * 0.5f, Projectile.scale * 1.32f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
