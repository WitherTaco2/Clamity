using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
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

namespace Clamity.Content.Boss.Pyrogen.Drop
{
    public class SoulOfPyrogen : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories"; 
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 10));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 34;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<MeleeDamageClass>() += 0.2f;
            player.GetDamage<RogueDamageClass>() += 0.2f;
            player.Clamity().pyroSpear = true;
        }
    }
    public class SoulOfPyrogenSpear : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            AIType = -1;
        }
        public override void AI()
        {
            NPC target = Main.npc[(int)Projectile.ai[0]];
            if (target == null) Projectile.Kill();
            if (!target.active) Projectile.Kill();
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (Projectile.Center.Y < target.Center.Y)
                {
                    Vector2 value = Projectile.SafeDirectionTo(target.Center) * (Projectile.velocity.Length() + 3.5f);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, value, 0.025f);
                }
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Flare, Projectile.velocity.RotatedByRandom(0.3f) / 4f, Scale: 2f);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 120);
        }
    }
}
