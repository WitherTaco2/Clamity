using CalamityMod;
using CalamityMod.Particles;
using Clamity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.EntropicSpace.Tiles
{
    public class NightmareGrass : ModTile
    {
        //public virtual string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');

        private const short subsheetWidth = 234;
        private const short subsheetHeight = 90;
        private int extraFrameHeight = 36;
        private int extraFrameWidth = 90;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.SetMerge(Type, ModContent.TileType<EntropicSlagTile>());

            HitSound = SoundID.Dig;
            MinPick = 100;
            RegisterItemDrop(ModContent.ItemType<EntropicSlag>());
            AddMapEntry(Color.Orange);

            //this.RegisterUniversalMerge(ModContent.TileType<BrimstoneSlag>(), "CalamityMod/Tiles/Merges/BrimstoneSlagMerge");
            //this.RegisterUniversalMerge(TileID.Ash, "CalamityMod/Tiles/Merges/AshMerge");
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1.82f;
            g = 1.07f;
            b = 0.56f;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.Stone, 0f, 0f, 1, new Color(100, 100, 100), 1f);
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<EntropicSlagTile>();
            }
        }
        public override void RandomUpdate(int i, int j)
        {
            Tile up = Main.tile[i, j - 1];
            Tile up2 = Main.tile[i, j - 2];

            if (up.LiquidAmount > 0)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<EntropicSlagTile>();
            }

            if (Main.rand.NextBool(20) || true)
            {
                GeneralParticleHandler.SpawnParticle(new NightmareSpark(new Vector2(i * 16, j * 16), -Vector2.UnitX * 5 * Main.rand.NextFloat(1, 1.2f)));
            }
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            bool isPlayerNear = WorldGen.PlayerLOS(i, j);
            Tile Above = Framing.GetTileSafely(i, j - 1);

            if (!Main.gamePaused && Main.instance.IsActive && !Above.HasTile && isPlayerNear)
            {
                if (Main.rand.NextBool(300))
                {
                    //TODO make Nightmare Blossom Dust
                    //int newDust = Dust.NewDust(new Vector2((i - 2) * 16, (j - 1) * 16), 5, 5, ModContent.DustType<CinderBlossomDust>());
                    //Main.dust[newDust].velocity.Y += 0.09f;
                }
            }

            if (Main.tile[i - 1, j - 1].TileType != Type || Main.tile[i, j - 1].TileType != Type || Main.tile[i + 1, j - 1].TileType != Type ||
                Main.tile[i - 1, j - 2].TileType != Type || Main.tile[i, j - 2].TileType != Type || Main.tile[i + 1, j - 2].TileType != Type)
            {
                try
                {
                    Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
                }
                catch { }
            }
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + zero;
            Color drawColour = GetDrawColour(i, j);
            Texture2D leaves = ModContent.Request<Texture2D>("Clamity/Content/Biomes/EntropicSpace/Tiles/NightmareBlossomGrassGrass").Value;

            DrawExtraTop(i, j, leaves, drawOffset, drawColour);
            DrawExtraWallEnds(i, j, leaves, drawOffset, drawColour);
            DrawExtraDrapes(i, j, leaves, drawOffset, drawColour);
        }
        #region 'Extra Drapes' Drawing
        private void DrawExtraTop(int i, int j, Texture2D extras, Vector2 drawOffset, Color drawColour)
        {
            /*
                If the tile directly above this tile is not otherworldly stone, or if it is, there is air to both sides of that tile, draw the Extra surface
            */
            if (
                CheckTile(Type, false, 0, 1, i, j) ||
                (CheckTile(Type, true, 0, 1, i, j) && CheckTile(Type, false, 1, 1, i, j) && CheckTile(Type, false, -1, 1, i, j) && CheckTile(Type, true, 1, 0, i, j) && CheckTile(Type, true, -1, 0, i, j))
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset, new Rectangle?(new Rectangle(GetExtraState("middle") + GetExtraVariant(i, j), GetExtraPattern(i), 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                Main.spriteBatch.Draw(extras, drawOffset + new Vector2(0f, 16f), new Rectangle?(new Rectangle(GetExtraState("middle") + GetExtraVariant(i, j), GetExtraPattern(i) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);

                DrawExtraOverhang(i, j, extras, drawOffset, drawColour);
            }
        }

        private void DrawExtraWallEnds(int i, int j, Texture2D extras, Vector2 drawOffset, Color drawColour)
        {
            /*
                Ending the Extra when a wall is reached
            */

            //Left
            if (
                CheckTile(Type, true, 1, 0, i, j) && CheckTile(Type, false, 1, 1, i, j) && CheckTile(Type, true, 0, 1, i, j) &&
                (CheckTile(Type, true, -1, 1, i, j) || CheckTile(Type, false, -1, 0, i, j))
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset, new Rectangle?(new Rectangle(GetExtraState("wallEndLeft") + GetExtraVariant(i + 1, j), GetExtraPattern(i), 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                Main.spriteBatch.Draw(extras, drawOffset + new Vector2(0f, 16f), new Rectangle?(new Rectangle(GetExtraState("wallEndLeft") + GetExtraVariant(i + 1, j), GetExtraPattern(i) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            //Right
            if (
                CheckTile(Type, true, -1, 0, i, j) && CheckTile(Type, false, -1, 1, i, j) && CheckTile(Type, true, 0, 1, i, j) &&
                (CheckTile(Type, true, 1, 1, i, j) || CheckTile(Type, false, 1, 0, i, j))
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset, new Rectangle?(new Rectangle(GetExtraState("wallEndRight") + GetExtraVariant(i - 1, j), GetExtraPattern(i), 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                Main.spriteBatch.Draw(extras, drawOffset + new Vector2(0f, 16f), new Rectangle?(new Rectangle(GetExtraState("wallEndRight") + GetExtraVariant(i - 1, j), GetExtraPattern(i) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }

        private void DrawExtraOverhang(int i, int j, Texture2D extras, Vector2 drawOffset, Color drawColour)
        {
            /*
                Called from DrawExtraTop(). Ending the Extra when the edge of the tile is reached
            */

            //Left
            if (
                CheckTile(Type, false, -1, 0, i, j)
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset + new Vector2(-16f, 0f), new Rectangle?(new Rectangle(GetExtraState("overhangLeft") + GetExtraVariant(i, j), GetExtraPattern(i - 1), 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                Main.spriteBatch.Draw(extras, drawOffset + new Vector2(-16f, 16f), new Rectangle?(new Rectangle(GetExtraState("overhangLeft") + GetExtraVariant(i, j), GetExtraPattern(i - 1) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            //Right
            if (
                CheckTile(Type, false, 1, 0, i, j)
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset + new Vector2(16f, 0f), new Rectangle?(new Rectangle(GetExtraState("overhangRight") + GetExtraVariant(i, j), GetExtraPattern(i + 1), 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                Main.spriteBatch.Draw(extras, drawOffset + new Vector2(16f, 16f), new Rectangle?(new Rectangle(GetExtraState("overhangRight") + GetExtraVariant(i, j), GetExtraPattern(i + 1) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }

        private void DrawExtraDrapes(int i, int j, Texture2D extras, Vector2 drawOffset, Color drawColour)
        {
            /*
                Hanging 'drapes' of the extra element
            */

            //Base
            if (
                (CheckTile(Type, true, 0, 1, i, j) && CheckTile(Type, false, 0, 2, i, j)) ||
                (CheckTile(Type, true, 0, 2, i, j) && CheckTile(Type, false, 1, 2, i, j) && CheckTile(Type, false, -1, 2, i, j) && CheckTile(Type, true, 1, 1, i, j) && CheckTile(Type, true, -1, 1, i, j))
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset, new Rectangle?(new Rectangle(GetExtraState("middle") + GetExtraVariant(i, j - 1), GetExtraPattern(i) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            //Left Wall
            if (
                CheckTile(Type, true, 1, 1, i, j) && CheckTile(Type, false, 1, 2, i, j) && CheckTile(Type, true, 0, 2, i, j) &&
                (CheckTile(Type, true, -1, 2, i, j) || CheckTile(Type, false, -1, 1, i, j))
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset, new Rectangle?(new Rectangle(GetExtraState("wallEndLeft") + GetExtraVariant(i + 1, j - 1), GetExtraPattern(i) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            //Right Wall
            if (
                CheckTile(Type, true, -1, 1, i, j) && CheckTile(Type, false, -1, 2, i, j) && CheckTile(Type, true, 0, 2, i, j) &&
                (CheckTile(Type, true, 1, 2, i, j) || CheckTile(Type, false, 1, 1, i, j))
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset, new Rectangle?(new Rectangle(GetExtraState("wallEndRight") + GetExtraVariant(i - 1, j - 1), GetExtraPattern(i) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            //Left Overhang
            if (
                CheckTile(Type, true, 1, 1, i, j) && CheckTile(Type, false, 0, 1, i, j) && CheckTile(Type, false, 0, 2, i, j) && CheckTile(Type, false, 1, 2, i, j)
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset, new Rectangle?(new Rectangle(GetExtraState("overhangLeft") + GetExtraVariant(i + 1, j - 1), GetExtraPattern(i) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            //Right Overhang
            if (
                CheckTile(Type, true, -1, 1, i, j) && CheckTile(Type, false, 0, 1, i, j) && CheckTile(Type, false, 0, 2, i, j) && CheckTile(Type, false, -1, 2, i, j)
                )
            {
                Main.spriteBatch.Draw(extras, drawOffset, new Rectangle?(new Rectangle(GetExtraState("overhangRight") + GetExtraVariant(i - 1, j - 1), GetExtraPattern(i) + 18, 18, 18)), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }
        #endregion

        #region Tile Data
        private bool CheckTile(int type, bool equal, int x, int y, int i, int j)
        {
            //Subtract y so that y is vertical for ease of readability
            return Main.tile[i + x, j - y].TileType == type == equal;
        }

        private Color GetDrawColour(int i, int j)
        {
            int colType = Main.tile[i, j].TileColor;
            Color paintCol = WorldGen.paintColor(colType);
            if (colType < 13)
            {
                paintCol.R = (byte)((paintCol.R / 2f) + 128);
                paintCol.G = (byte)((paintCol.G / 2f) + 128);
                paintCol.B = (byte)((paintCol.B / 2f) + 128);
            }
            if (colType == 29)
            {
                paintCol = Color.Black;
            }
            Color col = Lighting.GetColor(i, j);
            col.R = (byte)(paintCol.R / 255f * col.R);
            col.G = (byte)(paintCol.G / 255f * col.G);
            col.B = (byte)(paintCol.B / 255f * col.B);
            return col;
        }

        private int GetExtraState(string type)
        {
            switch (type)
            {
                case "middle":
                    return 36;
                case "overhangLeft":
                    return 18;
                case "overhangRight":
                    return 54;
                case "wallEndLeft":
                    return 0;
                case "wallEndRight":
                    return 72;
                default:
                    Main.NewText(type.ToString() + " is not a valid Extra sheet state");
                    return 0;
            }
        }

        private int GetExtraPattern(int i)
        {
            return i % 3 * extraFrameHeight;
        }

        private int GetExtraVariant(int i, int j)
        {
            return Main.tile[i, j].TileFrameNumber * extraFrameWidth;
        }
        #endregion
    }
}
