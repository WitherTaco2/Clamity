using CalamityMod;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Pyrogen.Drop.Weapons
{
    public class Obsidigun : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;

            Item.useTime = Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<ObsidigunBullet>();
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Bullet;

            Item.damage = 48;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 6f;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.Bullet)
                type = ModContent.ProjectileType<ObsidigunBullet>();
        }
        public override Vector2? HoldoutOffset() => new Vector2(-10f, -5f);
    }
    public class ObsidigunBullet : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 3;
            AIType = 14;
            //Projectile.Calamity().pointBlankShotDuration = 18;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2CircularEdge(20, 20), ModContent.ProjectileType<ObsidigunBulletShard>(), (int)(Projectile.damage / 2), Projectile.knockBack / 5, Projectile.owner);

            base.OnKill(timeLeft);
        }
    }
    public class ObsidigunBulletShard : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CrystalShard);
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] == 1)
                Projectile.DamageType = DamageClass.Melee;
        }
    }
}
