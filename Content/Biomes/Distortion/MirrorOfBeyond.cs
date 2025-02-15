using CalamityMod.Rarities;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion
{
    public class MirrorOfBeyond : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 10;
            Item.rare = ModContent.RarityType<Turquoise>();

            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool? UseItem(Player player)
        {
            if (!SubworldSystem.IsActive<TheDistortion>())
                SubworldSystem.Enter<TheDistortion>();
            else
                SubworldSystem.Exit();
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            //TODO - make animation later
            //if (player.ownedProjectileCounts[Item.shoot] >= 1)
            //    return false;

            // Entering/exiting subworlds appears to reset the mouse item for some reason, meaning that if you use this item
            // that way it'll be functionally distroyed, which we don't want.
            if (!Main.mouseItem.IsAir && Main.mouseItem.type == Type)
                return false;

            return true;
        }
    }
}
