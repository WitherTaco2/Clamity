using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Clamity.Content.Biomes.FrozenHell.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Rogue
{
    public class FrozenStarShuriken : RogueWeapon
    {
        public override float StealthDamageMultiplier => 0.5f;
        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 1;
            Item.height = 1;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 13f;
            Item.shoot = ModContent.ProjectileType<FrozenStarShurikenProjectile>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable())
                Main.projectile[index].Calamity().stealthStrike = true;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BlazingStar>()
                .AddIngredient<StarfishFromTheDepth>()
                .AddIngredient(ItemID.Trimarang)
                .AddIngredient<EnchantedMetal>(8)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
    public class FrozenStarShurikenProjectile : ModProjectile, ILocalizedModType, IModType
    {
        public override string Texture => ModContent.GetInstance<FrozenStarShuriken>().Texture;
        public new string LocalizationCategory => "Projectiles.Rogue";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = BlazingStarProj.Lifetime;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            switch ((int)Projectile.ai[0])
            {
                case 0: //Vortex

                    break;
                case 1: //Fast slices

                    break;
                case 2: //Dash with projectiles

                    break;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = 1f;
            Projectile.tileCollide = false;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 0)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Clamity/Assets/Textures/SlicerVortex").Value;

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.LightSkyBlue, Main.GlobalTimeWrappedHourly, texture.Size() / 2, Projectile.ai[2], SpriteEffects.None, 0);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.LightSkyBlue, -Main.GlobalTimeWrappedHourly / 3 * 2, texture.Size() / 2, Projectile.ai[2], SpriteEffects.FlipHorizontally, 0);
            }

            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor);
            return false;
        }
    }
}
