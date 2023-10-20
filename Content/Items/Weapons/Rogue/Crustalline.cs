using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using Clamity.Content.Boss.Pyrogen.Drop.Weapons;
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

namespace Clamity.Content.Items.Weapons.Rogue
{
    public class Crustalline : RogueWeapon
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetDefaults()
        {
            //Malachite
            Item.width = 74; Item.height = 68;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Yellow;

            Item.useTime = Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            //Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = ModContent.ProjectileType<CrustallineProjecile>();
            Item.shootSpeed = 12f;

            Item.damage = 55;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.knockBack = 2f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
            if (player.Calamity().StealthStrikeAvailable())
            {
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.type == ModContent.ProjectileType<CrustallineProjecile>())
                        proj.ai[0] = 1;
                }
            }
            return false;
        }
    }
    public class CrustallineProjecile : ModProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override string Texture => ModContent.GetInstance<Crustalline>().Texture;
        public int TargetIndex = -1;
        //public ref float Time => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 3;
            Projectile.aiStyle = 93;
            AIType = 514;
        }
        /*public override bool PreAI()
        {
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.ai[0] == 1)
            {
                Projectile.aiStyle = -1;
                NewAI();
                return false;
            }
            else
            {
                return true;
            }
        }*/
        public override void PostAI()
        {
            NewAI();
        }
        public void NewAI()
        {
            //Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
            {
                if (TargetIndex >= 0)
                {
                    if (!Main.npc[TargetIndex].active || !Main.npc[TargetIndex].CanBeChasedBy())
                    {
                        TargetIndex = -1;
                    }
                    else
                    {
                        Vector2 value = Projectile.SafeDirectionTo(Main.npc[TargetIndex].Center) * (Projectile.velocity.Length() + 3.5f);
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, value, 0.5f);
                    }
                }

                if (TargetIndex == -1)
                {
                    NPC nPC = Projectile.Center.ClosestNPCAt(1600f, ignoreTiles: false);
                    if (nPC != null)
                    {
                        TargetIndex = nPC.whoAmI;
                    }
                    /*else
                    {
                        Projectile.velocity *= 0.99f;
                    }*/
                }
            }
            /*else
            {
                Vector2 value = Projectile.SafeDirectionTo(Main.player[Projectile.owner].Center) * (Projectile.velocity.Length() + 3.5f);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, value, 0.05f);
            }*/
        }
    }
}
