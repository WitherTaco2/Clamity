using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using CalamityMod.Events;
using CalamityMod.Projectiles.Boss;
using Clamity.Content.Boss.WoB.NPCs;
using Terraria.Audio;

namespace Clamity.Content.Boss.WoB
{
    public class AncientConsole : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Placeables";

        public override void SetDefaults()
        {
            this.Item.createTile = ModContent.TileType<AncientConsoleTile>();
            this.Item.useTurn = true;
            this.Item.useAnimation = 15;
            this.Item.useTime = 10;
            this.Item.autoReuse = true;
            this.Item.consumable = true;
            this.Item.width = 38;
            this.Item.height = 32;
            this.Item.maxStack = 9999;
            this.Item.useStyle = 1;
            this.Item.rare = ModContent.RarityType<Violet>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MartianConduitPlating, 30)
                .AddIngredient<AuricBar>(5)
                .AddIngredient<CoreofCalamity>()
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
    public class AncientConsoleTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[3]
            {
                16,
                16,
                16
            };
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            this.AddMapEntry(new Color(43, 19, 42), CalamityUtils.GetItemName<AncientConsole>());
            TileID.Sets.DisableSmartCursor[Type] = true;
        }

        public static readonly SoundStyle SummonSound = new SoundStyle("CalamityMod/Sounds/Custom/SCalSounds/SepulcherSpawn")
        {
            Volume = 1.1f
        };
        public override bool CanExplode(int i, int j) => false;
        public override bool RightClick(int i, int j)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<WallOfBronze>()) || BossRushEvent.BossRushActive || !Main.LocalPlayer.ZoneUnderworldHeight)
                return true;

            Player player = Main.LocalPlayer;
            int center = Main.maxTilesX * 16 / 2;
            NPC.NewNPC(player.GetSource_ItemUse(player.HeldItem), (int)player.Center.X - 1000 * (player.Center.X > center ? -1 : 1), (int)player.Center.Y, ModContent.NPCType<WallOfBronze>());
            SoundEngine.PlaySound(SummonSound, new Vector2(i, j) * 16);

            /*if (Main.netMode != 1)
                NPC.SpawnOnPlayer(Main.LocalPlayer.whoAmI, ModContent.NPCType<WallOfBronze>());
            else
                NetMessage.SendData(61, number: Main.LocalPlayer.whoAmI, number2: (float)ModContent.NPCType<WallOfBronze>());*/

            return true;
        }
    }
}
