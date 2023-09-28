using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace Clamity.Content.Boss.Clamitas.Other
{
    public class ClamitasMusicbox : ModItem
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ClamitasTheme"), ModContent.ItemType<ClamitasMusicbox>(), ModContent.TileType<ClamitasMusicboxTile>(), 0);
        }
        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ClamitasMusicboxTile>();
            Item.width = 32;
            Item.height = 28;
            Item.rare = 4;
            Item.value = 100000;
            Item.accessory = true;
        }
    }
    public class ClamitasMusicboxTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            //TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            TileID.Sets.DisableSmartCursor[Type] = true;
            //ModTranslation mapEntryName = ((ModBlockType)this).CreateMapEntryName((string)null);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(191, 142, 111), name);
        }
        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.cursorItemIconEnabled = true;
            localPlayer.cursorItemIconID = ModContent.ItemType<ClamitasMusicbox>();
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (frameX > 30)
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<ClamitasMusicbox>());
        }

        public override bool CreateDust(int i, int j, ref int type) => false;
    }
}
