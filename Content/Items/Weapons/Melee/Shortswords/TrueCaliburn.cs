using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.BaseProjectiles;
using Clamity.Content.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Melee.Shortswords
{
    public class TrueCaliburn : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementItem", 8, Type);
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 42;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;

            Item.useAnimation = Item.useTime = 12;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 130;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5.75f;

            Item.shoot = ModContent.ProjectileType<TrueCaliburnProjectile>();
            Item.shootSpeed = 2.4f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Caliburn>()
                .AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class TrueCaliburnProjectile : BaseShortswordProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<TrueCaliburn>().Texture;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 8, Type);
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
            int a = 0;
            if (!Main.player[Projectile.owner].HasCooldown(ShortstrikeCooldown.ID))
            {
                a = 1;
                Main.player[Projectile.owner].AddCooldown(ShortstrikeCooldown.ID, 120);
            }
            int num = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 0.1f, ModContent.ProjectileType<TrueCaliburnSlash>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, a, 2);

        }
        public override void ExtraBehavior()
        {
            if (!Utils.NextBool(Main.rand, 5))
                return;
            Dust.NewDust(new Vector2((float)Projectile.Hitbox.X, (float)Projectile.Hitbox.Y), Projectile.Hitbox.Width, Projectile.Hitbox.Height, 57, 0.0f, 0.0f, 0, new Color(), 1f);
        }
    }
    public class TrueCaliburnSlash : BaseSlash
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 8, Type);
        }
        public override float Scale => 2f;
        public override Color FirstColor => Color.Purple;
        public override Color SecondColor => Color.LightPink;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 1)
            {
                //Projectile.ai[1] = 1;
                for (int i = 0; i < Projectile.ai[1]; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) * 0.1f, ModContent.ProjectileType<TrueCaliburnSlash2>(), (int)(Projectile.damage * 0.1f), Projectile.knockBack, Projectile.owner);
                if (Projectile.ai[1] > 1)
                {
                    Projectile.ai[1]--;
                }
            }
        }
    }
    public class TrueCaliburnSlash2 : TrueCaliburnSlash
    {
        public override float Scale => 1f;
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.usesLocalNPCImmunity = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = Projectile.MaxUpdates * 12;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
    }
    /*public class TrueCaliburnSlash : ExobeamSlash
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 8, Type);
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.scale = 2f;
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
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) * 0.1f, ModContent.ProjectileType<TrueCaliburnSlash2>(), (int)(Projectile.damage * 0.1f), Projectile.knockBack, Projectile.owner);
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
            return Color.Lerp(Color.Purple, Color.LightPink, Projectile.identity / 7f % 1f) * Projectile.Opacity;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(500, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center - new Vector2(500, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint);
        }
    }
    public class TrueCaliburnSlash2 : ExobeamSlash
    {
        public override string Texture => ModContent.GetInstance<TerraShivSlash>().Texture;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 8, Type);
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
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.Purple, Color.LightPink, Projectile.identity / 7f % 1f) * Projectile.Opacity;
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
    }*/
}
