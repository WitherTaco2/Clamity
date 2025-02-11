using CalamityMod.Rarities;
using Clamity.Content.Bosses.Yharim.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Yharim
{
    public class DragonAeriaTeleport : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 10;
            Item.rare = ModContent.RarityType<Violet>();

            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool? UseItem(Player player)
        {
            if (!SubworldSystem.IsActive<DragonAeria>())
                SubworldSystem.Enter<DragonAeria>();
            else
                SubworldSystem.Exit();
            return true;
        }
    }
}
