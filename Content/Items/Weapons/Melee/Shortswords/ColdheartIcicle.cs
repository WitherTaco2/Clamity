using CalamityMod;
using CalamityMod.Items;
using CalamityMod.NPCs.Providence;
using CalamityMod.Projectiles.BaseProjectiles;
using Clamity.Content.Cooldowns;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Melee.Shortswords
{
    public class ColdheartIcicle : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = Item.height = 24;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;

            Item.useAnimation = Item.useTime = 27;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 1;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.knockBack = 3f;
            Item.crit = 0;

            Item.shoot = ModContent.ProjectileType<ColdheartIcicleProjectile>();
            Item.shootSpeed = 2.4f;
        }
        public override void UpdateInventory(Player player)
        {
            Item.crit = 0;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit = 1f;
        }
    }
    public class ColdheartIcicleProjectile : BaseShortswordProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<ColdheartIcicle>().Texture;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.CritChance = 0;
        }
        public override void PostAI()
        {
            base.PostAI();
            Projectile.CritChance = 0;
        }
        /*public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!Main.player[Projectile.owner].HasCooldown(ShortstrikeCooldown.ID))
            {
                Main.player[Projectile.owner].AddCooldown(ShortstrikeCooldown.ID, 240);
            }
        }*/
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.player[Projectile.owner].HasCooldown(ShortstrikeCooldown.ID))
            {
                Main.player[Projectile.owner].AddCooldown(ShortstrikeCooldown.ID, 240);
                if (target.type != NPCID.TargetDummy && target.type != ModContent.NPCType<Providence>())
                    target.life -= target.lifeMax / 50;
                target.checkDead();
                //target.HealEffect(-target.life / 50);
                //target.HitEffect(dmg: target.life / 50);
            }
        }
    }
}
