using Clamity.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Tools
{
    public class RodOfMisery : ModItem, ILocalizedModType, IModType
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
        public new string LocalizationCategory => "Items.Misc";
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 42;
            Item.rare = ItemRarityID.Orange;

            Item.useTime = Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
        }
        public override bool CanUseItem(Player player)
        {
            return !player.chaosState;
        }
        public override bool? UseItem(Player player)
        {
            int random = Main.rand.Next(3);
            Vector2 pos = player.Center;
            switch (random)
            {
                case 0:
                    pos = player.Center - (Main.MouseWorld - player.Center);
                    break;
                case 1:
                    pos.X = player.Center.X - (Main.MouseWorld - player.Center).X;
                    break;
                case 2:
                    pos.Y = player.Center.Y - (Main.MouseWorld - player.Center).Y;
                    break;
                default:
                    pos = player.Center - (Main.MouseWorld - player.Center);
                    break;
            }

            player.Teleport(pos, 4, 0);
            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, pos.X, pos.Y, 1, 0, 0);
            player.AddBuff(BuffID.ChaosState, 15 * 60, true);

            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("AnyGemHook")
                .AddIngredient<MediocreMatter>()
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
