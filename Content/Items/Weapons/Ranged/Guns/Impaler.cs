using CalamityMod.Items.Materials;

namespace Clamity.Content.Items.Weapons.Ranged.Guns
{
    public class Impaler : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 56; Item.height = 36;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;

            Item.useAnimation = Item.useTime = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item38;
            Item.autoReuse = true;

            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 8f;

            Item.useAmmo = AmmoID.Stake;
            Item.shoot = ProjectileID.Stake;
            Item.shootSpeed = 20;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return !Main.rand.NextBool(4);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //type = ModContent.ProjectileType<ImpalerProjectile>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.PiOver4 / 12f), type, damage, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset() => new Vector2?(new Vector2(-5f, 0f));
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StakeLauncher)
                .AddIngredient<ScoriaBar>(10)
                .AddIngredient(ItemID.HellstoneBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class ImpalerProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Stake);
        }
        public override void AI()
        {
            if (!Main.rand.NextBool(5))
                return;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, new Color(), 1f);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.type != NPCID.Vampire && target.type != NPCID.VampireBat)
                return;
            modifiers.SetInstantKill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, new Rectangle(0, 0, 36, 14), Projectile.GetAlpha(lightColor));
            return false;
        }
    }
}
