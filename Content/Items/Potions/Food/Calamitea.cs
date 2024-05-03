using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Potions.Food
{
    public class Calamitea : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Potions.Foods";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(24, 26, BuffID.WellFed3, 15 * 60 * 60, true);
            Item.value += Terraria.Item.sellPrice(gold: 24);
            Item.rare = ModContent.RarityType<Turquoise>();
        }
    }
}
