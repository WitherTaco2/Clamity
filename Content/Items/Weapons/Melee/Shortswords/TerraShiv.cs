using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Buffs.DamageOverTime;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityMod;
using Clamity.Content.Cooldowns;

namespace Clamity.Content.Items.Weapons.Melee.Shortswords
{
    public class TerraShiv : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementItem", 10, Type);
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 42;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;

            Item.useAnimation = Item.useTime = 13;
            Item.useStyle = 13;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 140;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 6.5f;

            Item.shoot = ModContent.ProjectileType<TerraShivProjectile>();
            Item.shootSpeed = 2.4f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<TrueCaliburn>()
                .AddIngredient(ItemID.GoldShortsword)
                .AddIngredient<LivingShard>(6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient<TrueCaliburn>()
                .AddIngredient(ItemID.PlatinumShortsword)
                .AddIngredient<LivingShard>(6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class TerraShivProjectile : BaseShortswordProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<TerraShiv>().Texture;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 10, Type);
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            /*int a = 1;
            for (int i = 1; i < 4; i++)
            {
                int num = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4 / 4f, MathHelper.PiOver4 / 4f)) * i, ModContent.ProjectileType<ExoGladiusBeam>(), (int)(Projectile.damage * 0.33f), Projectile.knockBack / 3f, Projectile.owner, ai2: a);
                if (Main.projectile.IndexInRange(num))
                {
                    Main.projectile[num].timeLeft -= i * 4;
                }
                //a = 0;
            }*/
            int a = 0;
            if (!Main.player[Projectile.owner].HasCooldown(ShortstrikeCooldown.ID))
            {
                a = 1;
                Main.player[Projectile.owner].AddCooldown(ShortstrikeCooldown.ID, 120);
            }
            int num = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 0.1f, ModContent.ProjectileType<TerraShivSlash>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, a, 4);

        }
        public override void ExtraBehavior()
        {
            if (!Utils.NextBool(Main.rand, 5))
                return;
            Dust.NewDust(new Vector2((float)Projectile.Hitbox.X, (float)Projectile.Hitbox.Y), Projectile.Hitbox.Width, Projectile.Hitbox.Height, 107, 0.0f, 0.0f, 0, new Color(), 1f);
        }
    }
    public class TerraShivSlash : ExobeamSlash
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 10, Type);
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.scale = 5f;
            Projectile.width = Projectile.height = 24;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
            //Projectile.ai[1] = 4;
            if (Projectile.ai[0] == 1)
            {
                //Projectile.ai[1] = 1;
                for (int i = 0; i < Projectile.ai[1]; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) * 0.1f, ModContent.ProjectileType<TerraShivSlash2>(), (int)(Projectile.damage * 0.1f), Projectile.knockBack, Projectile.owner);
                if (Projectile.ai[1] > 1)
                {
                    Projectile.ai[1]--;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.Lime, Color.Green, Projectile.identity / 7f % 1f) * Projectile.Opacity;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(1000, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center - new Vector2(1000, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint);
        }
    }
    public class TerraShivSlash2 : ExobeamSlash
    {
        public override string Texture => ModContent.GetInstance<TerraShivSlash>().Texture;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 10, Type);
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = Projectile.height = 24;
            Projectile.usesLocalNPCImmunity = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = Projectile.MaxUpdates * 12;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.Lime, Color.Green, Projectile.identity / 7f % 1f) * Projectile.Opacity;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(200, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center - new Vector2(200, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
    }
}
