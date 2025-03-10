﻿using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Pyrogen.Drop.Weapons
{
    public class SearedShredder : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementItem", 2, Type);
        }
        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 68;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;

            Item.useTime = Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<SearedShredderProjectile>();
            Item.shootSpeed = 10f;

            Item.damage = 62;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 7.5f;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 180);
        }
    }
    public class SearedShredderProjectile : ModProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public int TargetIndex = -1;
        public ref float Time => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", 2, Type);
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DeathSickle);
            Projectile.width = 74;
            Projectile.height = 68;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(TargetIndex);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            TargetIndex = reader.ReadInt32();
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Time >= 24f)
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
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, value, 0.05f);
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
            }
        }
    }
}
