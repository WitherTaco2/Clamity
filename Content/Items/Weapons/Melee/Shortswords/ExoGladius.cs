
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.DataStructures;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Melee.Shortswords;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Melee.Shortswords
{
    public class ExoGladius : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = Item.height = 64;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;

            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = 13;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 640;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 8.5f;

            Item.shoot = ModContent.ProjectileType<ExoGladiusProjectile>();
            Item.shootSpeed = 2.4f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GalileoGladius>()
                .AddIngredient<CosmicShiv>()
                .AddIngredient<Lucrecia>()
                .AddIngredient<MiracleMatter>()
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
    public class ExoGladiusProjectile : BaseShortswordProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<ExoGladius>().Texture;
        public override void SetDefaults()
        {
            //AquaticDischargeProj
            /*Projectile.width = Projectile.height = 64;
            Projectile.timeLeft = 1000;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();*/

            Projectile.width = Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }
        public override void SetVisualOffsets()
        {
            int num = Projectile.width / 2;
            int num2 = Projectile.height / 2;
            DrawOriginOffsetX = 0f;
            DrawOffsetX = -(32 - num);
            DrawOriginOffsetY = -(32 - num2);
        }


        public override void OnSpawn(IEntitySource source)
        {
            int a = 1;
            for (int i = 1; i < 4; i++)
            {
                int num = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4 / 4f, MathHelper.PiOver4 / 4f)) * i, ModContent.ProjectileType<ExoGladiusBeam>(), (int)(Projectile.damage * 0.33f), Projectile.knockBack / 3f, Projectile.owner, ai2: a);
                if (Main.projectile.IndexInRange(num))
                {
                    Main.projectile[num].timeLeft -= i * 4;
                }
                //a = 0;
            }
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            //SpriteEffects effects = Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, value.Width, value.Height), Projectile.GetAlpha(lightColor), Projectile.rotation + MathHelper.PiOver4, value.Size() / 2f, Projectile.scale, 0);
            return false;
        }*/
        public override void ExtraBehavior()
        {
            if (!Utils.NextBool(Main.rand, 5))
                return;
            Dust.NewDust(new Vector2((float)Projectile.Hitbox.X, (float)Projectile.Hitbox.Y), Projectile.Hitbox.Width, Projectile.Hitbox.Height, 107, 0.0f, 0.0f, 0, new Color(), 1f);
        }

    }
    public class ExoGladiusBeam : Exobeam, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<Exobeam>().Texture;
        /*public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool(2))
            {
                Color newColor = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.9f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(20f, 20f) + Projectile.velocity, 267, Projectile.velocity * -2.6f, 0, newColor);
                dust.scale = 0.3f;
                dust.fadeIn = Main.rand.NextFloat() * 1.2f;
                dust.noGravity = true;
            }

            Projectile.scale = Utils.GetLerpValue(0f, 0.1f, Projectile.timeLeft / 600f, clamped: true);
            if (Projectile.FinalExtraUpdate())
            {
                Time += 1f;
            }
        }*/
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(in Exoblade.BeamHitSound, target.Center);
            if (Projectile.ai[2] == 1)
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<TerratomereExplosion>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0f, 0f, 0f);

            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
    }
    public class ExoGladiusExplosion : ModProjectile, /*IAdditiveDrawer,*/ ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 520;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.MaxUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 14;
            Projectile.scale = 0.2f;
            Projectile.hide = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(in SubsumingVortex.ExplosionSound, Projectile.Center);
                Projectile.localAI[0] = 1f;
            }

            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.5f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 20 == 19)
            {
                Projectile.frame++;
            }

            if (Projectile.frame >= 7)
            {
                Projectile.Kill();
            }

            Projectile.scale *= 1.013f;
            Projectile.Opacity = Utils.GetLerpValue(5f, 36f, Projectile.timeLeft, clamped: true);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D value2 = ModContent.Request<Texture2D>("CalamityMod/Skies/XerocLight").Value;
            Rectangle rectangle = value.Frame(1, 7, Projectile.frame, 0);
            //Rectangle rectangle = new Rectangle(value.Width * Projectile.frame, 0, value.Width, value.Height / Main.projFrames[Type]);
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Vector2 origin = rectangle.Size() * 0.5f;
            for (int i = 0; i < 36; i++)
            {
                Vector2 position = vector + (MathF.PI * 2f * i / 36f + Main.GlobalTimeWrappedHourly * 5f).ToRotationVector2() * Projectile.scale * 12f;
                Color value3 = CalamityUtils.MulticolorLerp(Projectile.timeLeft / 144f, new Color(210, 234, 110), new Color(141, 162, 67));
                value3 = Color.Lerp(value3, Color.White, 0.4f) * Projectile.Opacity * 0.184f;
                Main.spriteBatch.Draw(value2, position, null, value3, 0f, value2.Size() * 0.5f, Projectile.scale * 1.32f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(value, vector, rectangle, Color.White, 0f, origin, 1.6f, SpriteEffects.None, 0f);
            return false;
        }
        /*public void AdditiveDraw(SpriteBatch spriteBatch)
        {
            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D value2 = ModContent.Request<Texture2D>("CalamityMod/Skies/XerocLight").Value;
            //Rectangle rectangle = value.Frame(1, 7, Projectile.frame, 0);
            Rectangle rectangle = new Rectangle(value.Width * Projectile.frame, 0, value.Width, value.Height);
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Vector2 origin = rectangle.Size() * 0.5f;
            for (int i = 0; i < 36; i++)
            {
                Vector2 position = vector + (MathF.PI * 2f * i / 36f + Main.GlobalTimeWrappedHourly * 5f).ToRotationVector2() * Projectile.scale * 12f;
                Color value3 = CalamityUtils.MulticolorLerp(Projectile.timeLeft / 144f, new Color(210, 234, 110), new Color(141, 162, 67));
                value3 = Color.Lerp(value3, Color.White, 0.4f) * Projectile.Opacity * 0.184f;
                Main.spriteBatch.Draw(value2, position, null, value3, 0f, value2.Size() * 0.5f, Projectile.scale * 1.32f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(value, vector, rectangle, Color.White, 0f, origin, 1.6f, SpriteEffects.None, 0f);
        }*/
    }
}
