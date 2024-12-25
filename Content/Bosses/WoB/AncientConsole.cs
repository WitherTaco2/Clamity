using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Clamity.Content.Bosses.WoB.NPCs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Clamity.Content.Bosses.WoB
{
    public class AncientConsole : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.SummonBoss";

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<AncientConsoleTile>();
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 38;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ModContent.RarityType<Violet>();
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
            Volume = 1.1f,
            Pitch = 0.2f
        };
        public override bool CanExplode(int i, int j) => false;
        public override bool RightClick(int i, int j)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<WallOfBronze>()) || BossRushEvent.BossRushActive || !Main.LocalPlayer.ZoneUnderworldHeight)
                return true;

            //CalamityUtils.
            Vector2 tilePosInWorld = new Vector2(i * 16, j * 16);
            Dictionary<int, float> distance = new Dictionary<int, float>();
            foreach (Player p in Main.player)
            {
                if (p == null) continue;
                if (!p.active || p.dead) continue;
                distance.Add(p.whoAmI, Vector2.Distance(p.Center, tilePosInWorld));
            }
            float min = float.MaxValue; int thisPlayer = -1;
            foreach (var d in distance)
            {
                if (d.Value < min)
                {
                    min = d.Value;
                    thisPlayer = d.Key;
                }
            }
            if (thisPlayer != -1)
            {
                Player player = Main.player[thisPlayer];
                int center = Main.maxTilesX * 16 / 2;
                NPC.NewNPC(player.GetSource_ItemUse(new Item(ModContent.ItemType<WoBSummonItem>())), (int)player.Center.X - 1000 * (player.Center.X > center ? -1 : 1), (int)player.Center.Y, ModContent.NPCType<WallOfBronze>());

                SoundEngine.PlaySound(SummonSound, new Vector2(i, j) * 16);

                /*if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(Main.myPlayer, ModContent.NPCType<WallOfBronze>());
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, Main.myPlayer, (int)ModContent.NPCType<WallOfBronze>());*/
            }
            return true;
        }
    }
}
