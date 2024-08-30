using CalamityMod;
using CalamityMod.Particles;
using Clamity.Content.Bosses.Losbaf.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf.Projectiles
{
    public class ExoScythe : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 48;
            Projectile.hostile = true;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
            Projectile.alpha = 100;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] == 2 && Projectile.ai[1] != 2)
                Projectile.timeLeft = (int)(LosbafSuperboss.DistanceOnRotationAttack / LosbafSuperboss.VelocityOnRotationAttack);
        }
        public override void AI()
        {
            Projectile.rotation += 0.25f;
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    if (Projectile.timeLeft == 1000 - 60)
                    {
                        //Projectile.ai[1] = Projectile.velocity.Length();
                        Projectile.velocity = Vector2.Zero;
                    }
                    if (Projectile.timeLeft == 1000 - 180)
                    {
                        int target = Player.FindClosest(Projectile.Center, 100, 100);
                        Projectile.velocity = (Main.player[target].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
                    }
                    break;
                case 1:
                    if (Projectile.timeLeft == 1000 - 20)
                        Projectile.velocity = Vector2.Zero;
                    if (Projectile.timeLeft > 1000 - LosbafSuperboss.DuratationBetweenDownfallScytheAttack)
                    {
                        Color color3 = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 0.5f + Projectile.whoAmI * 0.12f) % 1, Color.Cyan, Color.Lime, Color.GreenYellow, Color.Goldenrod, Color.Orange);
                        GeneralParticleHandler.SpawnParticle(new BloomLineVFX(Projectile.Center,
                                                                              -Vector2.UnitY * 4000,
                                                                              1f * (1000 - Projectile.timeLeft) / LosbafSuperboss.DuratationBetweenDownfallScytheAttack,
                                                                              color3,
                                                                              1
                                                                              ));

                    }
                    if (Projectile.timeLeft == 1000 - LosbafSuperboss.DuratationBetweenDownfallScytheAttack)
                    {
                        int target = Player.FindClosest(Projectile.Center, 100, 100);
                        Projectile.velocity = new Vector2(0, Projectile.ai[1]);
                    }
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:
                    if (Projectile.timeLeft == 1000 - 20)
                    {
                        //Projectile.ai[1] = Projectile.velocity.Length();
                        Projectile.velocity = Vector2.Zero;
                    }
                    if (Projectile.timeLeft == 1000 - LosbafSuperboss.DuratationBetweenDownfallScytheAttack - 20)
                    {
                        int target = Player.FindClosest(Projectile.Center, 100, 100);
                        Projectile.velocity = (Main.player[target].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
                    }
                    break;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 1 && Projectile.timeLeft > 1000 - LosbafSuperboss.DuratationBetweenDownfallScytheAttack)
            {
                SpriteEffects effects = SpriteEffects.None;
                /*if (Projectile.spriteDirection == 1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }*/
                Texture2D value6 = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomLine").Value;
                Color color3 = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 0.5f + Projectile.whoAmI * 0.12f) % 1, Color.Cyan, Color.Lime, Color.GreenYellow, Color.Goldenrod, Color.Orange); ;
                Main.spriteBatch.Draw(value6,
                                 Projectile.Center /*- base.NPC.rotation.ToRotationVector2() * base.NPC.spriteDirection * 104f*/ - Main.screenPosition,
                                 null,
                                 color3,
                                 MathHelper.Pi,
                                 new Vector2((float)value6.Width / 2f, value6.Height),
                                 new Vector2(1f * (1000 - Projectile.timeLeft) / LosbafSuperboss.DuratationBetweenDownfallScytheAttack, 4200f),
                                 effects,
                                 0f);
            }
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], Color.White, 1);

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 2 && Projectile.ai[1] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 2) / 3, ModContent.ProjectileType<ExoScythe>(), Projectile.damage, 0, Main.myPlayer, 2, 2);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 2 + MathHelper.PiOver4) / 3, ModContent.ProjectileType<ExoScythe>(), Projectile.damage, 0, Main.myPlayer, 2, 2);

            }
        }
    }
}
