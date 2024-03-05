using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Classless
{
    public class TrashOfMagnus : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 56;
            Item.value = Item.sellPrice(0, 6);
            Item.rare = ItemRarityID.Yellow;

            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;

            Item.damage = 24;
            Item.DamageType = ModContent.GetInstance<AverageDamageClass>();
            Item.knockBack = 2f;
        }
    }
}
