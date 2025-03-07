using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions.Food
{
    public class ClamChowder : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Foods";
        public override void SetDefaults()
        {
            Item.DefaultToFood(30, 22, BuffID.WellFed2, 8 * 60 * 60);
            Item.value = Terraria.Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Pink;
        }
    }
}
