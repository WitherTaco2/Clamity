using CalamityMod.Items;
using CalamityMod.NPCs.Providence;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod;
using Clamity.Content.Cooldowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

namespace Clamity.Content.Items.Weapons.Melee.Shortswords
{
    public class Caliburn : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementItem", 8, Type);
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;

            Item.useAnimation = Item.useTime = 10;
            Item.useStyle = 13;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5.25f;

            Item.shoot = ModContent.ProjectileType<CaliburnProjectile>();
            Item.shootSpeed = 2.4f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CaliburnProjectile : BaseShortswordProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => ModContent.GetInstance<Caliburn>().Texture;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 8, Type);
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 38;
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
                Main.player[Projectile.owner].AddCooldown(ShortstrikeCooldown.ID, 30);
                for (int i = 0; i < 3; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4 / 2) / 10, ModContent.ProjectileType<CaliburnSlash>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            }
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            //CalamityUtils.DrawBackglow(Projectile, Color.Yellow, 1.2f, Main.spriteBatch, null, Main.screenPosition, TextureAssets.Projectile[Projectile.type].Value)
            Texture2D texture = ((overrideTexture == null) ? TextureAssets.Npc[npc.type].Value : overrideTexture);
            Vector2 vector = npc.Center - screenPos;
            Vector2 origin = frame.Size() * 0.5f;
            Color color = backglowColor * npc.Opacity;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * backglowArea;
                Main.spriteBatch.Draw(texture, vector + vector2, frame, color, npc.rotation, origin, npc.scale, spriteEffects, 0f);
            }
            return base.PreDraw(ref lightColor);
        }*/
        public override void ExtraBehavior()
        {
            if (!Utils.NextBool(Main.rand, 5))
                return;
            Dust.NewDust(new Vector2((float)Projectile.Hitbox.X, (float)Projectile.Hitbox.Y), Projectile.Hitbox.Width, Projectile.Hitbox.Height, 57, 0.0f, 0.0f, 0, new Color(), 1f);
        }
    }
    public class CaliburnSlash : ExobeamSlash
    {
        public override string Texture => ModContent.GetInstance<TerraShivSlash>().Texture;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 8, Type);
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = Projectile.height = 24;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.Yellow, Color.White, Projectile.identity / 7f % 1f) * Projectile.Opacity;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(200, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint)
                || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center - new Vector2(200, 0).RotatedBy(Projectile.rotation), Projectile.Size.Length() * Projectile.scale / 10f, ref collisionPoint);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
    }
}
