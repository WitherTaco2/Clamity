using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Clamitas.Drop
{
    public class SlagspitterPauldron : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Foods";
        public override void SetDefaults()
        {
            Item.DefaultToFood(28, 26, BuffID.WellFed3, 16 * 60 * 60);
            Item.value = 0;
            Item.rare = ItemRarityID.Lime;
        }
    }
}
