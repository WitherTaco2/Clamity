using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Pyrogen.Drop.Weapons
{
    public class MoltenPiercer : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;

            Item.useTime = Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            //Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = ModContent.ProjectileType<ObsidigunBullet>();
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Bullet;

            Item.damage = 57;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 5f;
        }
    }
    public class MoltenPiercerProjectile : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";

    }
}
