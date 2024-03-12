using Clamity.Content.Biomes.FrozenHell.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Renewals
{
    public class FrozenHellRenewalSupreme : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("Fargowiltas", out Mod _);
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault(name);
            // Tooltip.SetDefault(tooltip);
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.value = Item.buyPrice(0, 0, 3);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = 1;
            Item.shootSpeed = 5f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FrozenHellRenewalSupremeProjectile>(), 0, 0, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FrozenHellRenewal>(10)
                .AddIngredient(ItemID.ChlorophyteBar, 5)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
    public class FrozenHellRenewalSupremeProjectile : ModProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("Fargowiltas", out Mod _);
        }
        public override string Texture => ModContent.GetInstance<FrozenHellRenewalSupreme>().Texture;
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 170;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);

            float[] speedX = { 0, 0, 5, 5, 5, -5, -5, -5 };
            float[] speedY = { 5, -5, 0, 5, -5, 0, 5, -5 };

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                for (int i = 0; i < 8; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speedX[i], speedY[i], ModContent.ProjectileType<CyanSolutionProjectile>(), 0, 0, Main.myPlayer);
                }
            }
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (Main.tile[x, y].TileType == TileID.Ash || Main.tile[x, y].TileType == TileID.AshGrass)
                    {
                        Main.tile[x, y].TileType = (ushort)ModContent.TileType<FrozenAshTile>();
                        WorldGen.SquareTileFrame(x, y);
                        NetMessage.SendTileSquare(-1, x, y, 1);
                    }

                    if (Main.tile[x, y].TileType == TileID.Hellstone)
                    {
                        Main.tile[x, y].TileType = (ushort)ModContent.TileType<FrozenHellstoneTile>();
                        WorldGen.SquareTileFrame(x, y);
                        NetMessage.SendTileSquare(-1, x, y, 1);
                    }
                }
            }
        }
    }
}
