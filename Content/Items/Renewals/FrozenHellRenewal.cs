using Clamity.Content.Biomes.FrozenHell.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Renewals
{
    public class FrozenHellRenewal : ModItem
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
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FrozenHellRenewalProjectile>(), 0, 0, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle)
                .AddIngredient<CyanSolution>(100)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
    public class FrozenHellRenewalProjectile : ModProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("Fargowiltas", out Mod _);
        }
        public override string Texture => ModContent.GetInstance<FrozenHellRenewal>().Texture;
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

            int radius = 150;
            float[] speedX = { 0, 0, 5, 5, 5, -5, -5, -5 };
            float[] speedY = { 5, -5, 0, 5, -5, 0, 5, -5 };

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                for (int i = 0; i < 8; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speedX[i], speedY[i], ModContent.ProjectileType<CyanSolutionProjectile>(), 0, 0, Main.myPlayer);
                }
            }
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + Projectile.Center.X / 16.0f);
                    int yPosition = (int)(y + Projectile.Center.Y / 16.0f);

                    // Circle
                    if (Math.Sqrt(x * x + y * y) <= radius + 0.5)
                    {
                        if (Main.tile[xPosition, yPosition].TileType == TileID.Ash || Main.tile[xPosition, yPosition].TileType == TileID.AshGrass)
                        {
                            Main.tile[xPosition, yPosition].TileType = (ushort)ModContent.TileType<FrozenAshTile>();
                            WorldGen.SquareTileFrame(xPosition, yPosition);
                            NetMessage.SendTileSquare(-1, xPosition, yPosition, 1);
                        }

                        if (Main.tile[xPosition, yPosition].TileType == TileID.Hellstone)
                        {
                            Main.tile[xPosition, yPosition].TileType = (ushort)ModContent.TileType<FrozenHellstoneTile>();
                            WorldGen.SquareTileFrame(xPosition, yPosition);
                            NetMessage.SendTileSquare(-1, xPosition, yPosition, 1);
                        }
                    }
                }
            }
        }
    }
}
