using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Rogue.FargosCrossover
{
    public class LifebringerDagger : RogueWeapon
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("FargowiltasSouls", out Mod _);
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 42;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 10);

            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.damage = 70;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.knockBack = 5f;

            Item.shoot = ModContent.ProjectileType<LifebringerDaggerProjectile>();
            Item.shootSpeed = 15f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable() && p.WithinBounds(Main.maxProjectiles))
                Main.projectile[p].Calamity().stealthStrike = true;
            return false;
        }
    }
    public class LifebringerDaggerProjectile : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<LifebringerDagger>().Texture;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("FargowiltasSouls", out Mod _);
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 42;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 480;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int rare = 10;
            int timeLeft = Main.rand.Next(1, 4);
            if (Projectile.Calamity().stealthStrike)
            {
                rare = 5;
                timeLeft = Main.rand.Next(2, 6);
            }
            timeLeft *= 60;
            if (Main.rand.NextBool(rare))
            {
                int index = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, target.velocity.SafeNormalize(Vector2.Zero) * 2, ModContent.ProjectileType<LifebringerDaggerLight>(), Projectile.damage / 2, 1f, Projectile.owner, timeLeft);
            }
        }
    }
    public class LifebringerDaggerLight : ExobeamSlash
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.scale = 5f;
            Projectile.width = Projectile.height = 24;
            Projectile.timeLeft = (int)Projectile.ai[0];
        }
        public override void AI()
        {
            base.AI();
            Projectile.rotation = MathHelper.ToRadians(90f);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.timeLeft = (int)Projectile.ai[0];
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.Yellow, Color.White, Projectile.identity / 7f % 1f) * Projectile.Opacity;
        }
    }
}
