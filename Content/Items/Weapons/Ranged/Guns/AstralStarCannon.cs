using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Ranged.Guns
{
    public class AstralStarCannon : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 74; Item.height = 24;
            Item.rare = ItemRarityID.Lime;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.value = Item.sellPrice(0, 12);

            Item.useAnimation = Item.useTime = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;

            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 7f;

            Item.useAmmo = AmmoID.FallenStar;
            Item.shoot = ModContent.ProjectileType<AstralStarCannonProjectile>();
            Item.shootSpeed = 40;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StarCannon)
                .AddIngredient<AureusCell>(10)
                .AddIngredient<StarblightSoot>(25)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class AstralStarCannonProjectile : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.scale = 2f;
            Projectile.penetrate = 5;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
        }
        public override void AI()
        {
            Projectile.rotation += 0.1f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
            for (int i = 0; i < 5; i++)
            {
                Vector2 vec1 = Vector2.UnitY.RotateRandom(0.2f);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center - vec1 * 300, vec1 * 20, ModContent.ProjectileType<AstralStarCannonProjectile2>(), Projectile.damage / 10, Projectile.knockBack, Projectile.owner);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
            for (int i = 0; i < 5; i++)
            {
                Vector2 vec1 = Vector2.UnitY.RotateRandom(0.2f);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center - vec1 * 300, vec1 * 20, ModContent.ProjectileType<AstralStarCannonProjectile2>(), Projectile.damage / 10, Projectile.knockBack, Projectile.owner);
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawStarTrail(Projectile, Color.Lerp(new Color(255, 164, 94), new Color(109, 242, 196), MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.Pi)), Color.White);

            Texture2D t = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, t.Size() / 2, Projectile.scale, SpriteEffects.None, 1);
            return false;
        }
    }
    public class AstralStarCannonProjectile2 : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
        }
        public override void AI()
        {
            Projectile.rotation += 0.1f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawStarTrail(Projectile, Color.White, Color.Yellow);

            Texture2D t = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, t.Size() / 2, Projectile.scale, SpriteEffects.None, 1);
            return false;
        }
    }
}
