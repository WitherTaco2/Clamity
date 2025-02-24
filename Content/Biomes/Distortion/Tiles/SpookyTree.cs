using Clamity.Content.Biomes.Distortion.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion.Tiles
{
    public class SpookyTree : ModTree
    {
        private Asset<Texture2D> texture;
        private Asset<Texture2D> branchesTexture;
        private Asset<Texture2D> topsTexture;
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };
        private string Texture => (GetType().Namespace + "." + nameof(SpookyTree)).Replace('.', '/');
        public override void SetStaticDefaults()
        {
            // Makes Example Tree grow on ExampleBlock
            GrowsOnTileId = [ModContent.TileType<NightmareGrass>()];
            texture = ModContent.Request<Texture2D>(Texture);
            branchesTexture = ModContent.Request<Texture2D>(Texture + "_Branches");
            topsTexture = ModContent.Request<Texture2D>(Texture + "_Tops");
        }

        // This is the primary texture for the trunk. Branches and foliage use different settings.
        public override Asset<Texture2D> GetTexture()
        {
            return texture;
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<SpookySapling>();
        }

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            // This is where fancy code could go, but let's save that for an advanced example
        }

        // Branch Textures
        public override Asset<Texture2D> GetBranchTextures() => branchesTexture;

        // Top Textures
        public override Asset<Texture2D> GetTopTextures() => topsTexture;

        public override int DropWood()
        {
            return ItemID.SpookyWood;
        }

        public override bool Shake(int x, int y, ref bool createLeaves)
        {
            //Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, ModContent.ItemType<Items.Placeable.ExampleBlock>());
            //TODO - Later make exclusive food and shaking tree

            int type = ItemID.SpookyWood, count = Main.rand.Next(20, 40);
            if (Main.halloween && Main.rand.NextBool(50))
            {
                type = ItemID.RottenEgg;
                count = Main.rand.Next(10, 13);
            }
            if (Main.rand.NextBool(7))
            {
                type = ItemID.Acorn;
                count = Main.rand.Next(1, 3);
            }
            if (Main.rand.NextBool(20))
            {
                type = ModContent.ItemType<PumpkinFetus>();
                count = 1;
            }
            if (Main.rand.NextBool(25))
            {
                if (Main.rand.NextBool(100))
                {
                    type = ItemID.PlatinumCoin;
                    count = 3;
                }
                else if (Main.rand.NextBool(25))
                {
                    type = ItemID.PlatinumCoin;
                    count = 1;
                }
                else if (Main.rand.NextBool(5))
                {
                    type = ItemID.GoldCoin;
                    count = Main.rand.Next(20, 40);
                }
                else
                {
                    type = ItemID.GoldCoin;
                    count = 10;
                }
            }

            Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, type, count);

            return false;
        }

        /*public override int TreeLeaf()
        {
            return ModContent.GoreType<ExampleTreeLeaf>();
        }*/
    }
}
