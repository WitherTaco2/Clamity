using CalamityMod.Events;
using CalamityMod.Rarities;
using Clamity.Content.Bosses.WoB.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.WoB
{
    public class WoBSummonItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.value = Terraria.Item.sellPrice(0, 10, 0);
            Item.rare = ModContent.RarityType<Violet>();

            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item1;
            Item.consumable = false;
        }
        public static readonly SoundStyle SummonSound = new SoundStyle("CalamityMod/Sounds/Custom/SCalSounds/SepulcherSpawn")
        {
            Volume = 1.1f
        };
        public override bool? UseItem(Player player)
        {
            //player.Teleport(new Vector2(100, Main.UnderworldLayer + 40) * 16);
            //NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X - 100, (int)player.Center.Y, ModContent.NPCType<WallOfBronze>());
            //NPC.SpawnWOF

            int center = Main.maxTilesX * 16 / 2;
            NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X - 1000 * (player.Center.X > center ? -1 : 1), (int)player.Center.Y, ModContent.NPCType<WallOfBronze>());
            SoundEngine.PlaySound(SummonSound, player.Center);

            /*if (Main.netMode != 1)
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<WallOfBronze>());
            else
                NetMessage.SendData(61, number: player.whoAmI, number2: (float)ModContent.NPCType<WallOfBronze>());*/

            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ZoneUnderworldHeight && !NPC.AnyNPCs(ModContent.NPCType<WallOfBronze>()) && !BossRushEvent.BossRushActive;
        }
        /*public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MartianConduitPlating, 30)
                .AddIngredient<AuricBar>(5)
                .AddIngredient<CoreofCalamity>()
                .AddTile<CosmicAnvil>()
                .Register();
        }*/
    }
}
