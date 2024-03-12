using CalamityMod.Projectiles.BaseProjectiles;
using Clamity.Content.Cooldowns;

namespace Clamity.Content.Items.Weapons.Melee.Shortswords
{
    public class Tomutus : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementItem", 2, Type);
        }
        public override void SetDefaults()
        {
            Item.width = 36; Item.height = 38;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;

            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 35;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5.5f;

            Item.shoot = ModContent.ProjectileType<TomutusProjectile>();
            Item.shootSpeed = 2.4f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class TomutusProjectile : BaseShortswordProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<Tomutus>().Texture;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 2, Type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 36; Projectile.height = 38;
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 60);
            if (!Main.player[Projectile.owner].HasCooldown(ShortstrikeCooldown.ID))
            {
                Main.player[Projectile.owner].AddCooldown(ShortstrikeCooldown.ID, 30);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ProjectileID.Volcano, hit.Damage, hit.Knockback, Projectile.owner);
            }
        }
        public override void ExtraBehavior()
        {
            //Lighting.AddLight(Projectile.Center, )
            if (!Utils.NextBool(Main.rand, 5))
                return;
            Dust.NewDust(new Vector2((float)Projectile.Hitbox.X, (float)Projectile.Hitbox.Y), Projectile.Hitbox.Width, Projectile.Hitbox.Height, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 3f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, new Rectangle(0, 0, Projectile.width, Projectile.height), new Color(255, 255, 255, 200));
            return base.PreDraw(ref lightColor);
        }
    }
}
