using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using Clamity.Content.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Clamity.Content.Items.Weapons.Rogue
{
    public class NuclearFuelBarrel : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = Item.useTime = 22;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;

            Item.damage = 333;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.knockBack = 8f;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<NuclearFuelBarrelProj>();
            Item.shootSpeed = 14f;
        }

        public override void ModifyStatsExtra(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = position - Vector2.UnitY * 12f;
            velocity.Y *= 0.667f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (p.WithinBounds(Main.maxProjectiles))
                Main.projectile[p].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AcidicRainBarrel>()
                .AddIngredient<SpentFuelContainer>()
                .AddIngredient<NuclearEssence>(5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class NuclearFuelBarrelProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public float cooldown = 0f;
        public float oldVelocityX = 0f; public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 480;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.extraUpdates = 2;
        }

        public bool CollideX => Projectile.oldPosition.X == Projectile.position.X;

        public override void AI()
        {
            bool stealthS = Projectile.Calamity().stealthStrike;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.ai[1] = stealthS.ToInt() == 0 ? 1 : 3;
                Projectile.localAI[0] = 1f;
            }
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathHelper.ToRadians(8f);
            if (Projectile.velocity.Y < 15f)
            {
                Projectile.velocity.Y += 0.1f;
            }
            if (CollideX && cooldown == 0)
            {
                BounceEffects();
                Projectile.velocity.X = -oldVelocityX;
            }
            else if (cooldown > 0)
            {
                cooldown -= 1f;
            }
            if (Projectile.velocity.X != 0f)
            {
                oldVelocityX = Math.Sign(Projectile.velocity.X) * 12f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public void BounceEffects()
        {
            bool stealthS = Projectile.Calamity().stealthStrike;
            int projectileCount = 8;
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            if (stealthS)
            {
                projectileCount += 5; //more shit the closer we are to death
            }
            for (int i = 0; i < projectileCount; i++)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 shrapnelVelocity = (Vector2.UnitY * (-16f + Main.rand.NextFloat(-3, 12f))).RotatedByRandom((double)MathHelper.ToRadians(30f));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity + shrapnelVelocity,
                        ModContent.ProjectileType<AcidShrapnel>(), Projectile.damage, 3f, Projectile.owner);
                }
                else
                {
                    Vector2 acidVelocity = (Vector2.UnitY * (-16f + Main.rand.NextFloat(-3, 12f))).RotatedByRandom((double)MathHelper.ToRadians(40f));
                    int acidIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity + acidVelocity,
                        ModContent.ProjectileType<AcidBarrelDrop>(),
                        (int)(Projectile.damage * 0.75f), 1f, Projectile.owner);
                    if (acidIndex.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[acidIndex].DamageType = ModContent.GetInstance<RogueDamageClass>();
                        Main.projectile[acidIndex].timeLeft = 300;
                        Main.projectile[acidIndex].usesLocalNPCImmunity = true;
                        Main.projectile[acidIndex].localNPCHitCooldown = -1;
                        Main.projectile[acidIndex].extraUpdates = 1;
                    }
                }
            }

            if (stealthS)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 acidVelocity = (Vector2.UnitY * (-16f + Main.rand.NextFloat(-3, 12f))).RotatedByRandom((double)MathHelper.ToRadians(40f));
                    int acidIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity + acidVelocity,
                        ModContent.ProjectileType<AcidBarrelDrop>(),
                        (int)(Projectile.damage * 0.667f), 1f, Projectile.owner);
                    if (acidIndex.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[acidIndex].DamageType = ModContent.GetInstance<RogueDamageClass>();
                        Main.projectile[acidIndex].timeLeft = 420;
                        Main.projectile[acidIndex].extraUpdates = 1;
                    }
                }
            }

            Point result;
            if (WorldUtils.Find(Projectile.Top.ToTileCoordinates(), Searches.Chain((GenSearch)new Searches.Down(80), (GenCondition)new Conditions.IsSolid()), out result))
            {
                Vector2 pos = result.ToVector2() * 16f;
                if ((pos - Projectile.Center).Length() > 16 * 25)
                    pos = Projectile.Center + new Vector2(0, 100);

                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<SulphuricNukesplosion>(), (int)(Projectile.damage * .5f), 2f, Projectile.owner);
                if (proj.WithinBounds(Main.maxProjectiles))
                    Main.projectile[proj].Calamity().stealthStrike = Projectile.Calamity().stealthStrike;
            }

            Projectile.ai[1]--;
            cooldown = 15;
            if (Projectile.ai[1] <= 0)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }

    }
}
