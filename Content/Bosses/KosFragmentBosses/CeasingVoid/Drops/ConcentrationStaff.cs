using CalamityMod.Buffs.Summon;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.KosFragmentBosses.CeasingVoid.Drops
{
    public class ConcentrationStaff : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 72;

            Item.useTime = Item.useAnimation = 15; // 14 because of useStyle 1
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.DD2_EtherianPortalOpen;

            Item.DamageType = DamageClass.Summon;
            Item.damage = 105;
            Item.knockBack = 4f;

            Item.mana = 10;
            Item.shoot = ModContent.ProjectileType<ConcentrationStaffSummon>();
            Item.shootSpeed = 10f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2)
                return player.ownedProjectileCounts[ModContent.ProjectileType<ConcentrationStaffSummon>()] == 0;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int p = Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
                if (Main.projectile.IndexInRange(p))
                    Main.projectile[p].originalDamage = Item.damage;
                player.AddBuff(ModContent.BuffType<VoidConcentrationBuff>(), 120);
            }
            return false;
        }
    }
    public class ConcentrationStaffSummon : ModProjectile
    {
        //public override string Texture => ModContent.GetInstance<VoidConcentrationAura>().Texture;
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 80;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 3f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            ClamityPlayer modPlayer = owner.Clamity();
            owner.AddBuff(ModContent.BuffType<ConcentrationStaffBuff>(), 2);
            if (Type != ModContent.ProjectileType<ConcentrationStaffSummon>())
                return;

            if (owner.dead)
                modPlayer.concentration = false;
            if (modPlayer.concentration)
                Projectile.timeLeft = 2;

            Projectile.Center = owner.Center;
        }
    }
    public class ConcentrationStaffBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            //Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            ClamityPlayer mp = player.Clamity();
            int count = player.ownedProjectileCounts[ModContent.ProjectileType<ConcentrationStaffSummon>()];
            player.GetDamage<SummonDamageClass>() += 0.1f; //10%
            mp.concentration = true;
            if (!mp.concentration)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
