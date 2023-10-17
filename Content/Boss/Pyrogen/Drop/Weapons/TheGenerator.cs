using CalamityMod;
using CalamityMod.Cooldowns;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.Pyrogen.Drop.Weapons
{
    public class TheGenerator : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementItem", 2, Type);
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 46;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;

            Item.useTime = Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<TheGeneratorSigil>();
            Item.shootSpeed = 5f;

            Item.damage = 67;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.mana = 17;
        }
    }
    public class TheGeneratorSigil : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementItem", 2, Type);
        }
        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.width = Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 500;
            //Projectile.extraUpdates = 3;
            AIType = 14;
            //Projectile.Calamity().pointBlankShotDuration = 18;
        }
        public int TargetIndex = -1;
        public override void AI()
        {
            if (TargetIndex >= 0)
            {
                if (!Main.npc[TargetIndex].active || !Main.npc[TargetIndex].CanBeChasedBy())
                {
                    TargetIndex = -1;
                }
                else
                {
                    Vector2 value = Projectile.SafeDirectionTo(Main.npc[TargetIndex].Center)/* * (Projectile.velocity.Length() + 3.5f)*/;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, value, 0.01f);
                }
            }

            if (TargetIndex == -1)
            {
                NPC nPC = Projectile.Center.ClosestNPCAt(1600f, ignoreTiles: false);
                if (nPC != null)
                {
                    TargetIndex = nPC.whoAmI;
                }
            }
            Projectile.rotation = -Projectile.velocity.X * 0.05f;
        }
    }
}
