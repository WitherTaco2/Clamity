using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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

            Item.shoot = ModContent.ProjectileType<MoltenPiercerProjectile>();
            Item.shootSpeed = 12f;

            Item.damage = 57;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.knockBack = 5f;
        }
    }
    public class MoltenPiercerProjectile : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 40;
            Projectile.aiStyle = -1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 5;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            AIType = -1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.5f;
            //if (Projectile.timeLeft < 540) 
            //    Projectile.rotation
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //CursedDaggerProj

        }
    }
}
