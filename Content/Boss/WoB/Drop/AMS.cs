using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.Drop
{
    public class AMS : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Classless";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatCountAsBombsForDemolitionistToSpawn[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 34;
            Item.value = Terraria.Item.sellPrice(0, 30, 24);
            Item.rare = ModContent.RarityType<Violet>();

            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.damage = 100;
            Item.knockBack = 0;
            Item.mana = 12;

            Item.shoot = ModContent.ProjectileType<AMSProj>();
            Item.shootSpeed = 5f;
            Item.UseSound = new SoundStyle?(SoundID.Item1);
        }
    }
    public class AMSProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bomb);
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.Bomb;
        }
        public override void PostAI()
        {
            if (this.Projectile.timeLeft < 2)
            {
                this.Projectile.damage = 100;
                this.Projectile.knockBack = 10f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            base.Projectile.ExpandHitboxBy(100);
            base.Projectile.maxPenetrate = (base.Projectile.penetrate = -1);
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 10;
            base.Projectile.Damage();
            SoundEngine.PlaySound(in SoundID.Item14, base.Projectile.Center);
            for (int i = 0; i < 40; i++)
            {
                int num = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }

            for (int j = 0; j < 70; j++)
            {
                int num2 = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity *= 5f;
                num2 = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num2].velocity *= 2f;
            }

            if (Main.netMode != 2)
            {
                Vector2 center = base.Projectile.Center;
                int num3 = 3;
                Vector2 position = new Vector2(center.X - 24f, center.Y - 24f);
                for (int k = 0; k < num3; k++)
                {
                    float num4 = 0.33f;
                    if (k < num3 / 3)
                    {
                        num4 = 0.66f;
                    }

                    if (k >= 2 * num3 / 3)
                    {
                        num4 = 1f;
                    }

                    int type = Main.rand.Next(61, 64);
                    int num5 = Gore.NewGore(base.Projectile.GetSource_Death(), position, default(Vector2), type);
                    Gore obj = Main.gore[num5];
                    obj.velocity *= num4;
                    obj.velocity.X += 1f;
                    obj.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    num5 = Gore.NewGore(base.Projectile.GetSource_Death(), position, default(Vector2), type);
                    Gore obj2 = Main.gore[num5];
                    obj2.velocity *= num4;
                    obj2.velocity.X -= 1f;
                    obj2.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    num5 = Gore.NewGore(base.Projectile.GetSource_Death(), position, default(Vector2), type);
                    Gore obj3 = Main.gore[num5];
                    obj3.velocity *= num4;
                    obj3.velocity.X += 1f;
                    obj3.velocity.Y -= 1f;
                    type = Main.rand.Next(61, 64);
                    num5 = Gore.NewGore(base.Projectile.GetSource_Death(), position, default(Vector2), type);
                    Gore obj4 = Main.gore[num5];
                    obj4.velocity *= num4;
                    obj4.velocity.X -= 1f;
                    obj4.velocity.Y -= 1f;
                }
            }

            base.Projectile.ExpandHitboxBy(15);
            if (base.Projectile.owner == Main.myPlayer)
            {
                base.Projectile.ExplodeTiles(5, false);
            }
        }
    }
}
