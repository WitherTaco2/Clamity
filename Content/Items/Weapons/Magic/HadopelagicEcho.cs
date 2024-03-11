using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace Clamity.Content.Items.Weapons.Magic
{
    public class HadopelagicEcho : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        private int counter;
        public override void SetDefaults()
        {
            Item.damage = 500;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 8;
            Item.reuseDelay = 20;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(2, 50);
            Item.rare = ModContent.RarityType<Violet>();
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<HadopelagicEchoProjectile>();
        }

        public override Vector2? HoldoutOffset() => new Vector2?(new Vector2(-5f, 0f));

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, (float)counter);
            ++this.counter;
            if (this.counter >= 5)
                this.counter = 0;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<EidolicWail>()
                .AddIngredient<ReaperTooth>(6)
                .AddIngredient<DepthCells>(20)
                .AddIngredient<Lumenyl>(20)
                .AddIngredient<AuricBar>(5)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
    public class HadopelagicEchoProjectile : ModProjectile
    {
        private int echoCooldown;
        private bool playedSound;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.scale = 0.005f;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
            Projectile.timeLeft = 450;
            //Projectile.Calamity().PierceResistHarshness = 0.06f;
            //Projectile.Calamity().PierceResistCap = 0.4f;
        }

        public override void AI()
        {
            if ((Projectile.ai[0] == 0 || Projectile.ai[0] == 4) && !playedSound)
            {
                //Main.PlaySound(Utils.NextBool(Main.rand, 100) ? mod.GetLegacySoundSlot((SoundType)4, "Sounds/NPCKilled/Sunskater") : this.mod.GetLegacySoundSlot((SoundType)50, "Sounds/Custom/WyrmScream"), (int)Projectile.Center.X, (int)Projectile.Center.Y);
                SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/WyrmScream"), Projectile.Center);
                playedSound = true;
            }
            if (Projectile.localAI[0] < 1f)
            {
                Projectile.localAI[0] += 0.05f;
                Projectile.scale += 0.05f;
                Projectile.width = (int)(36f * Projectile.scale);
                Projectile.height = (int)(36f * Projectile.scale);
            }
            else
            {
                Projectile.scale += MathF.Sin(Projectile.timeLeft) * 0.1f;
                Projectile.width = 36;
                Projectile.height = 36;
            }
            if (echoCooldown > 0)
                --echoCooldown;
            Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= Projectile.localAI[0];
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage *= Projectile.localAI[0];
        }
        //public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => damage = (int)((double)damage * (double)Projectile.localAI[0]);

        //public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 240);
            if (echoCooldown > 0)
                return;
            echoCooldown = 120;
            int num1 = ModContent.ProjectileType<HadopelagicEchoProjectile2>();
            int num2 = (int)(0.2f * Projectile.damage);
            float num3 = Projectile.knockBack / 3f;
            int num4 = 2;
            for (int index = 0; index < num4; ++index)
            {
                float num5 = Utils.NextFloat(Main.rand, 260f, 270f);
                Vector2 vector2_1 = Utils.NextVector2Unit(Main.rand, 0.0f, 6.28318548f);
                Vector2 vector2_2 = target.Center + vector2_1 * num5;
                float num6 = Utils.NextFloat(Main.rand, 15f, 18f);
                Vector2 vector2_3 = vector2_1 * (-num6);
                if (Projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(Projectile.GetSource_OnHit(target), vector2_2, vector2_3, num1, num2, num3, Projectile.owner, 0.0f, 0.0f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft >= 85)
                return new Color?(new Color((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, 100));
            byte num1 = (byte)(Projectile.timeLeft * 3);
            byte num2 = (byte)(100 * ((float)num1 / (float)byte.MaxValue));
            return new Color?(new Color((int)num1, (int)num1, (int)num1, (int)num2));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], Color.LightCyan * 0.9f);
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Color newColor = Color.LightCyan;
            Main.spriteBatch.Draw(texture, Projectile.Center + Vector2.UnitX.RotatedBy(Projectile.rotation) * 2f * Projectile.scale - Main.screenPosition, null, newColor * 0.75f, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1.15f, effects, 0);
            return false;
        }
    }
    public class HadopelagicEchoProjectile2 : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<HadopelagicEchoProjectile>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.scale = 0.85f;
            Projectile.alpha = (int)byte.MaxValue;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.timeLeft = 30;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.alpha > 100)
                Projectile.alpha -= 25;
            if (Projectile.alpha < 100)
                Projectile.alpha = 100;
            Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 240);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 240);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft >= 85)
                return new Color?(new Color((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, 100));
            byte num1 = (byte)(Projectile.timeLeft * 3);
            byte num2 = (byte)(100.0 * ((double)num1 / (double)byte.MaxValue));
            return new Color?(new Color((int)num1, (int)num1, (int)num1, (int)num2));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }
    }
}
