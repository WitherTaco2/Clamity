using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Rogue.FargosCrossover
{
    public class ToothyBomb : RogueWeapon
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("FargowiltasSouls", out Mod _);
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 58;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = ItemRarityID.LightRed;

            Item.useTime = Item.useAnimation = 22;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 35;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.knockBack = 8f;

            Item.shoot = ModContent.ProjectileType<ToothyBombProjectile>();
            Item.shootSpeed = 12f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = position - Vector2.UnitY * 12f;
            velocity.Y *= 0.85f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable() && p.WithinBounds(Main.maxProjectiles))
                Main.projectile[p].Calamity().stealthStrike = true;
            return false;
        }
    }
    public class ToothyBombProjectile : ModProjectile, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("FargowiltasSouls", out Mod _);
        }
        public new string LocalizationCategory => "Projectiles.Rogue";
        //public override string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');
        public float OldVelocityX = 0f;
        public float RemainingBounces
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public bool CollideX => Projectile.oldPosition.X == Projectile.position.X;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 480;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void AI()
        {

            if (++Projectile.frameCounter > 8)
            {
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
                Projectile.frameCounter = 0;
            }


            if (Projectile.localAI[0] == 0f)
            {
                RemainingBounces = Projectile.Calamity().stealthStrike ? 3 : 1;
                Projectile.localAI[0] = 1f;
            }
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathHelper.ToRadians(8f);
            if (Projectile.velocity.Y < 10f)
                Projectile.velocity.Y += 0.2f;

            if (CollideX)
            {
                Projectile.velocity.X = -OldVelocityX;


                int max = 5;
                if (Projectile.Calamity().stealthStrike)
                    max += 3;
                for (int i = 0; i < max; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(40f)) * Main.rand.NextFloat(-19f, -4f), ModContent.ProjectileType<ToothyBombShrapnel>(), Projectile.damage, 3f, Projectile.owner);
                }
                RemainingBounces--;
                if (RemainingBounces <= 0)
                {
                    Projectile.Kill();
                }
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            }

            if (Projectile.velocity.X != 0f)
                OldVelocityX = Math.Sign(Projectile.velocity.X) * 12f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
        /*public override void OnKill(int timeLeft)
        {
            int max = 5;
            if (Projectile.Calamity().stealthStrike)
                max += 3;
            float random = Main.rand.NextFloat(MathHelper.TwoPi);
            for (int i = 0; i < max; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(40f)) * Main.rand.NextFloat(-19f, -4f), ModContent.ProjectileType<ToothyBombShrapnel>(), Projectile.damage, 3f, Projectile.owner);
            }
        }*/
    }
    public class ToothyBombShrapnel : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("FargowiltasSouls", out Mod _);
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.extraUpdates = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 480;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = -1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
