using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Particles;
using CalamityMod.Projectiles;
using CalamityMod.Rarities;
using Clamity.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Ranged.Bows
{
    public class IrradiatedShellshooter : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;

            Item.damage = 400;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 6f;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;

        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (CalamityUtils.CheckWoodenAmmo(type, player))
                type = ModContent.ProjectileType<IrradiatedShellshooterProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -2; i < 2; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(0.1f * i), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CorrodedCaustibow>()

                .AddIngredient<NuclearEssence>(5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();

        }
    }
    public class IrradiatedShellshooterProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 6;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter > 5)
            {
                if (++Projectile.frame >= Main.projFrames[Type])
                {
                    Projectile.frame = 0;
                }
                Projectile.frameCounter = 0;
            }

            Projectile.velocity *= 0.9996f;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.timeLeft % 5 == 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    int aura = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<IrradiatedShellshooterProjTrail>(), (int)(Projectile.damage * 0.15), Projectile.knockBack, Projectile.owner);
                    if (aura.WithinBounds(Main.maxProjectiles))
                    {
                        //Main.projectile[aura].DamageType = DamageClass.Ranged;
                        //Main.projectile[aura].timeLeft = 20;
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 32;
            Projectile.position = Projectile.position - Projectile.Size / 2f;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
            int count = Main.rand.Next(6, 15);
            for (int i = 0; i < count; i++)
            {
                int idx = Dust.NewDust(Projectile.Center - Projectile.velocity / 2f, 0, 0, (int)CalamityDusts.SulphurousSeaAcid, 0f, 0f, 100, default, 3f);
                Main.dust[idx].velocity *= 3f;
                Main.dust[idx].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 480);
        }

        /*public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, 0, lightColor);
            return base.PreDraw(ref lightColor);
        }*/
    }
    public class IrradiatedShellshooterProjTrail : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 40;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.4f / 255f);

            if (Main.rand.NextBool(3))
            {
                MediumMistParticle mist = new MediumMistParticle(Projectile.Center, Vector2.Zero, Color.Lime, Color.Green, 2f, 0.5f, Main.rand.NextFloat(-0.1f, 0.1f));
                GeneralParticleHandler.SpawnParticle(mist);
            }
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? (int)CalamityDusts.SulphurousSeaAcid : 89, Alpha: 50);
                if (Main.dust[dust].type == 89)
                {
                    Main.dust[dust].scale *= 0.35f;
                }
                Main.dust[dust].velocity *= 0f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<Irradiated>(), 480);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<Irradiated>(), 480);

    }
}
