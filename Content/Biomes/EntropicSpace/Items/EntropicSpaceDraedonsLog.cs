using CalamityMod;
using CalamityMod.Rarities;
using CalamityMod.UI;
using CalamityMod.UI.DraedonLogs;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.EntropicSpace.Items
{
    public class EntropicSpaceDraedonsLog : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.DraedonItems";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
                PopupGUIManager.FlipActivityOfGUIWithType(typeof(EntropicSpaceDraedonsLogGui));
            return true;
        }
    }
    public class EntropicSpaceDraedonsLogGui : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            return CalamityUtils.GetTextValueFromModItem<EntropicSpaceDraedonsLog>("ContentPage" + (Page + 1));
        }
        public override Texture2D GetTextureByPage()
        {
            var image = (GetType().Namespace + "." + nameof(EntropicSpaceDraedonsLogGui)).Replace('.', '/');
            image = "Clamity/Content/Biomes/EntropicSpace/Items/EntropicSpaceDraedonsLogGui";
            return ModContent.Request<Texture2D>(image).Value;
        }
    }
}