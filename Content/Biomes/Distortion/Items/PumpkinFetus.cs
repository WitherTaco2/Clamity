using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion.Items
{
    public class PumpkinFetus : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Apple);
            Item.value = 50000;
            Item.buffType = BuffID.WellFed3;
        }
    }
}
