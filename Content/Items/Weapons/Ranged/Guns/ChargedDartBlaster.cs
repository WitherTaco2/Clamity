using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Ranged.Guns
{
    public class ChargedDartBlaster : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 68; Item.height = 26;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.Rarity10BuyPrice;

            Item.useAnimation = Item.useTime = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item38;
            Item.autoReuse = true;

            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 7f;

            Item.useAmmo = AmmoID.Dart;
            Item.shoot = ProjectileID.Seed;
            Item.shootSpeed = 10;

            Item.Calamity().MaxCharge = 100f;
            Item.Calamity().UsesCharge = true;
            Item.Calamity().ChargePerUse = 0.05f;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return !Main.rand.NextBool(4);
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargedBlast3>(), (int)(damage * 2.0), knockback, player.whoAmI, 0.0f, 0.0f);
                return false;
            }
            int num1 = Main.rand.Next(2, 5);
            for (int index = 0; index < num1; ++index)
            {
                float num2 = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                float num3 = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, num2, num3, type, damage / 2, knockback, player.whoAmI, 0.0f, 0.0f);
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargedBlast>(), damage, knockback, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2?(new Vector2(-5f, 0f));
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DartRifle)
                .AddIngredient<MysteriousCircuitry>(10)
                .AddIngredient<DubiousPlating>(15)
                .AddIngredient(ItemID.MartianConduitPlating, 25)
                .AddIngredient(ItemID.FragmentVortex, 3)
                .AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(3, out Func<bool> condition), condition)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.DartPistol)
                .AddIngredient<MysteriousCircuitry>(10)
                .AddIngredient<DubiousPlating>(15)
                .AddIngredient(ItemID.MartianConduitPlating, 25)
                .AddIngredient(ItemID.FragmentVortex, 3)
                .AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(3, out Func<bool> condition2), condition2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class ChargedBlast3 : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/LaserProj";
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.extraUpdates = 3;
            Projectile.alpha = byte.MaxValue;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 25;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.0f, 0.3f, 0.7f);
            float num1 = 50f;
            float num2 = 1.5f;
            if (Projectile.ai[1] == 0)
            {
                Projectile.localAI[0] += num2;
                if (Projectile.localAI[0] <= num1)
                    return;
                Projectile.localAI[0] = num1;
            }
            else
            {
                Projectile.localAI[0] -= num2;
                if (Projectile.localAI[0] > 0)
                    return;
                Projectile.Kill();
            }
        }
        public override Color? GetAlpha(Color lightColor) => new Color?(new Color(100, 100, byte.MaxValue, 0));
        public override bool PreDraw(ref Color lightColor)
        {
            return Projectile.DrawBeam(100f, 3f, lightColor);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0.0f, 0.0f, ModContent.ProjectileType<ChargedDartExplosion>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner, 0.0f, 0.0f);
            //Main.PlaySound(SoundID.Item62, (int)Projectile.position.X, (int)Projectile.position.Y);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            --Projectile.penetrate;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0.0f, 0.0f, ModContent.ProjectileType<ChargedDartExplosion>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner, 1f, 0.0f);
            //Main.PlaySound(SoundID.Item62, (int)Projectile.position.X, (int)Projectile.position.Y);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

        }

        public virtual void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 2;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0.0f, 0.0f, ModContent.ProjectileType<ChargedDartExplosion>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner, 0.0f, 0.0f);
        }
    }
    public class ChargedDartExplosion : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 5;
        }

        public override void AI()
        {
            if (Projectile.timeLeft != 5)
                return;
            if (Projectile.ai[1] == 0.0)
            {
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagnetSphere, 0.0f, 0.0f, 100, new Color(), 2f);
                    Dust dust = Main.dust[index2];
                    dust.velocity = dust.velocity * 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[index2].scale = 0.5f;
                        Main.dust[index2].fadeIn = (float)(1.0 + Main.rand.Next(10) * 0.10000000149011612);
                    }
                }
                for (int index3 = 0; index3 < 10; ++index3)
                {
                    int index4 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagnetSphere, 0.0f, 0.0f, 100, new Color(), 3f);
                    Main.dust[index4].noGravity = true;
                    Dust dust1 = Main.dust[index4];
                    dust1.velocity = dust1.velocity * 5f;
                    int index5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagnetSphere, 0.0f, 0.0f, 100, new Color(), 2f);
                    Dust dust2 = Main.dust[index5];
                    dust2.velocity = dust2.velocity * 2f;
                }
            }
            else
            {
                Projectile.height = Projectile.width = 100;
                for (int index6 = 0; index6 < 10; ++index6)
                {
                    int index7 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagnetSphere, 0.0f, 0.0f, 100, new Color(), 2f);
                    Dust dust = Main.dust[index7];
                    dust.velocity = dust.velocity * 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[index7].scale = 0.5f;
                        Main.dust[index7].fadeIn = (float)(1.0 + Main.rand.Next(10) * 0.10000000149011612);
                    }
                }
                for (int index8 = 0; index8 < 20; ++index8)
                {
                    int index9 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagnetSphere, 0.0f, 0.0f, 100, new Color(), 3f);
                    Main.dust[index9].noGravity = true;
                    Dust dust3 = Main.dust[index9];
                    dust3.velocity = dust3.velocity * 5f;
                    int index10 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.MagnetSphere, 0.0f, 0.0f, 100, new Color(), 2f);
                    Dust dust4 = Main.dust[index10];
                    dust4.velocity = dust4.velocity * 2f;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 240);
        }
    }
}
