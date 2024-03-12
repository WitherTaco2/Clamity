using Terraria.GameContent.Creative;

namespace Clamity.Content.Biomes.FrozenHell.Items
{
    public class CyanSolution : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Ammo.Solution";
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<CyanSolutionProjectile>() - ProjectileID.PureSpray;
            Item.ammo = AmmoID.Solution;
            Item.width = 10;
            Item.height = 12;
            Item.value = 5000;
            Item.rare = ItemRarityID.Orange;
            Item.maxStack = 9999;
            Item.consumable = true;
        }
    }
    public class CyanSolutionProjectile : ModProjectile
    {
        public ref float Progress => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            // Set the dust type to ExampleSolution
            int dustType = ModContent.DustType<CyanSolutionDust>();

            if (Projectile.owner == Main.myPlayer)
            {
                Convert((int)(Projectile.position.X + Projectile.width * 0.5f) / 16, (int)(Projectile.position.Y + Projectile.height * 0.5f) / 16, 2);
            }

            if (Projectile.timeLeft > 133)
            {
                Projectile.timeLeft = 133;
            }

            if (Progress > 7f)
            {
                float dustScale = 1f;

                if (Progress == 8f)
                {
                    dustScale = 0.2f;
                }
                else if (Progress == 9f)
                {
                    dustScale = 0.4f;
                }
                else if (Progress == 10f)
                {
                    dustScale = 0.6f;
                }
                else if (Progress == 11f)
                {
                    dustScale = 0.8f;
                }

                Progress += 1f;

                var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

                dust.noGravity = true;
                dust.scale *= 1.75f;
                dust.velocity.X *= 2f;
                dust.velocity.Y *= 2f;
                dust.scale *= dustScale;
            }
            else
            {
                Progress += 1f;
            }

            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public static void Convert(int i, int j, int size = 4)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        /*if (wall != 0)
                            {
                                Main.tile[k, l].WallType = (ushort)ModContent.WallType<ExampleWall>();
                                WorldGen.SquareWallFrame(k, l);
                                NetMessage.SendTileSquare(-1, k, l, 1);
                            }*/

                        /*if (TileID.Sets.Conversion.Stone[type] || type == TileID.ClayBlock)
                            {
                                Main.tile[k, l].TileType = (ushort)ModContent.TileType<SwampMudNewTile>();
                                WorldGen.SquareTileFrame(k, l);
                                NetMessage.SendTileSquare(-1, k, l, 1);
                            }

                        if (TileID.Sets.Conversion.Dirt[type] || TileID.Sets.Conversion.Grass[type])
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<SwampMudNewTile>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }*/

                        if (Main.tile[k, l].TileType == TileID.Ash || Main.tile[k, l].TileType == TileID.AshGrass)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<FrozenAshTile>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }

                        if (Main.tile[k, l].TileType == TileID.Hellstone)
                        {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<FrozenHellstoneTile>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }

                        if (Main.tile[k, l].LiquidType == LiquidID.Lava)
                        {
                            Main.tile[k, l].LiquidAmount = 0;
                            Main.tile[k, l].TileType = TileID.BreakableIce;
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                    }
                }
            }
        }
    }
    public class CyanSolutionDust : ModDust
    {
        public override void SetStaticDefaults()
        {
            UpdateType = 110;
        }
    }
}
