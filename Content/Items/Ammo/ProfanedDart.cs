using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Ammo
{
    public class ProfanedDart : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Ammo.Dart";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CrystalDart);
            Item.damage = 28;
            Item.shoot = ModContent.ProjectileType<ProfanedDartProjectile>();
            Item.rare = ItemRarityID.Purple;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100)
                .AddIngredient(ItemID.CrystalDart, 100)
                .AddIngredient<UnholyEssence>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class ProfanedDartProjectile : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<ProfanedDart>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.CrystalDart);
            Projectile.alpha = (int)byte.MaxValue;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 7;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != oldVelocity.Y || Projectile.velocity.X != oldVelocity.X)
            {
                --Projectile.penetrate;
                if (Projectile.penetrate <= 0)
                    Projectile.Kill();
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
            }
            if (Projectile.penetrate > 0 && Projectile.owner == Main.myPlayer)
            {
                int[] numArray = new int[10];
                int maxValue = 0;
                int num25 = 700;
                int num26 = 20;
                for (int index = 0; index < 200; ++index)
                {
                    if (Main.npc[index].CanBeChasedBy((object)this))
                    {
                        float num27 = (Projectile.Center - Main.npc[index].Center).Length();
                        if (num27 > num26 && num27 < num25 && Collision.CanHitLine(Projectile.Center, 1, 1, Main.npc[index].Center, 1, 1))
                        {
                            numArray[maxValue] = index;
                            ++maxValue;
                            if (maxValue >= 9)
                                break;
                        }
                    }
                }
                if (maxValue > 0)
                {
                    int index = Main.rand.Next(maxValue);
                    Vector2 vector2 = Main.npc[numArray[index]].Center - Projectile.Center;
                    float num28 = Projectile.velocity.Length();
                    vector2.Normalize();
                    Projectile.velocity = vector2 * num28;
                    Projectile.netUpdate = true;
                }
            }
            return false;
        }
        public override void AI() //477
        {
            ++Projectile.localAI[0];
            if (Projectile.localAI[0] > 3f)
                Projectile.alpha = 0;
            if (Projectile.ai[0] >= 20f)
            {
                Projectile.ai[0] = 20f;
            }

            if (Projectile.localAI[1] < 5.0)
            {
                Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
                ++Projectile.localAI[1];
            }
            else
                Projectile.rotation = (Projectile.rotation * 2f + MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f) / 3f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Projectile.alpha == 0 ? new Color((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, 200) : new Color(0, 0, 0, 0);
        }
        public override void PostAI()
        {
            int index = Dust.NewDust(Projectile.Center, 1, 1, 244, 0.0f, 0.0f, 0, new Color(), 1f);
            Main.dust[index].position = ((Entity)this.Projectile).Center;
            Main.dust[index].noGravity = true;
            Main.dust[index].scale = Utils.NextFloat(Main.rand, 0.6f, 1.6f);
            //Vector2 vector2 = Utils.RotatedBy(Projectile.velocity, (double)Utils.NextFloat(Main.rand, -0.17f, 0.17f), new Vector2()) * 0.6f;
            Main.dust[index].velocity = Utils.RotatedBy(Projectile.velocity, (double)Utils.NextFloat(Main.rand, -0.17f, 0.17f), new Vector2()) * 0.6f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //ModContent.GetInstance<HolyFireBulletProj>().Kill(Projectile.timeLeft);

            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
            if (Projectile.owner == Main.myPlayer)
            {
                int damage = (int)(Projectile.damage * 0.33f);
                float ai = 0.85f + Main.rand.NextFloat() * 1.15f;
                int num = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FuckYou>(), damage, Projectile.knockBack, Projectile.owner, 0f, ai);
                if (num.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[num].DamageType = Projectile.DamageType;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                float scale = Main.rand.NextFloat(1.4f, 1.8f);
                int num2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244);
                Main.dust[num2].noGravity = false;
                Main.dust[num2].scale = scale;
                float num3 = 0.25f;
                float num4 = Main.rand.NextFloat(0f - num3, num3);
                float num5 = Main.rand.NextFloat(0.08f, 0.14f);
                Vector2 velocity = Projectile.oldVelocity.RotatedBy(num4) * num5;
                Main.dust[num2].velocity = velocity;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesFromEdge(this.Projectile, 0, lightColor);
            return false;
        }
    }
}
