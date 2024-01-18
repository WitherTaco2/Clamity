using CalamityMod.Cooldowns;
using CalamityMod;
using Clamity.Content.Cooldowns;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Clamity.Content.Items.Weapons.Melee.Shortswords;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Accessories;

namespace Clamity
{
    public class ShortstrikeInfo
    {
        public int item;
        public int proj;
        public int hitcount;
        public float damageMult;
        public ShortstrikeInfo(int item, int proj, int hitcount, float damageMult)
        {
            this.item = item;
            this.proj = proj;
            this.hitcount = hitcount;
            this.damageMult = damageMult;
        }
        public ShortstrikeInfo(int item, int hitcount, float damageMult)
        {
            this.item = item;
            this.proj = -1;
            this.hitcount = hitcount;
            this.damageMult = damageMult;
        }
    }
    public static class ShortstrikeUtils
    {
        public static bool ContainItem(this List<ShortstrikeInfo> list, int type)
        {
            foreach (ShortstrikeInfo info in list)
            {
                if (info.item == type)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool ContainProjectile(this List<ShortstrikeInfo> list, int type)
        {
            foreach (ShortstrikeInfo info in list)
            {
                if (info.proj == type)
                {
                    return true;
                }
            }
            return false;
        }
        public static float GetDamageMult(this List<ShortstrikeInfo> list, int type)
        {
            foreach (ShortstrikeInfo info in list)
            {
                if (info.proj == type)
                {
                    return info.damageMult;
                }
            }
            return 0;
        }
        public static int GetHitCount(this List<ShortstrikeInfo> list, int type)
        {
            foreach (ShortstrikeInfo info in list)
            {
                if (info.proj == type)
                {
                    return info.hitcount;
                }
            }
            return 0;
        }
    }
    public class ShortstrikePlayer : ModPlayer
    {
        /*public static List<int> shortswords = new List<int>()
        {
            ItemID.CopperShortsword, ItemID.TinShortsword,
            ItemID.IronShortsword, ItemID.LeadShortsword,
            ItemID.SilverShortsword, ItemID.TungstenShortsword,
            ItemID.GoldShortsword, ItemID.PlatinumShortsword,
            ItemID.Gladius
        };*/
        public static List<ShortstrikeInfo> shortswords = new List<ShortstrikeInfo>()
        {
            new ShortstrikeInfo(ItemID.CopperShortsword, ProjectileID.CopperShortswordStab, 3, 0.1f),
            new ShortstrikeInfo(ItemID.TinShortsword, ProjectileID.TinShortswordStab, 3, 0.1f),
            new ShortstrikeInfo(ItemID.IronShortsword, ProjectileID.IronShortswordStab, 3, 0.12f),
            new ShortstrikeInfo(ItemID.LeadShortsword, ProjectileID.LeadShortswordStab, 3, 0.12f),
            new ShortstrikeInfo(ItemID.SilverShortsword, ProjectileID.SilverShortswordStab, 3, 0.14f),
            new ShortstrikeInfo(ItemID.TungstenShortsword, ProjectileID.TungstenShortswordStab, 3, 0.14f),
            new ShortstrikeInfo(ItemID.GoldShortsword, ProjectileID.GoldShortswordStab, 4, 0.2f),
            new ShortstrikeInfo(ItemID.PlatinumShortsword, ProjectileID.PlatinumShortswordStab, 4, 0.2f),
            new ShortstrikeInfo(ItemID.Gladius, ProjectileID.GladiusStab, 4, 0.1f),

            //new ShortstrikeInfo(ItemID.CopperShortsword, ProjectileID.CopperShortswordStab, 3, 0.1f),
            //new ShortstrikeInfo(ItemID.CopperShortsword, ProjectileID.CopperShortswordStab, 3, 0.1f),
            //new ShortstrikeInfo(ItemID.CopperShortsword, ProjectileID.CopperShortswordStab, 3, 0.1f),
        };

        /*{
            { ItemID.CopperShortsword, 0.1f },
            { ItemID.TinShortsword, 0.1f },
            { ItemID.IronShortsword, 0.12f },
            { ItemID.LeadShortsword, 0.12f },
            { ItemID.SilverShortsword, 0.14f },
            { ItemID.TungstenShortsword, 0.14f },
            { ItemID.GoldShortsword, 0.2f },
            { ItemID.PlatinumShortsword, 0.2f },
            { ItemID.Gladius, 0.1f },

            { ModContent.ItemType<WulfrumLeechDagger>(), 0 },
            { ModContent.ItemType<SporeKnife>(), 0.05f },
            { ModContent.ItemType<Tomutus>(), 0.3f },
            { ModContent.ItemType<ColdheartIcicle>(), 0 },
            { ModContent.ItemType<Caliburn>(), 0.3f },
            { ModContent.ItemType<TrueCaliburn>(), 0.4f },
            { ModContent.ItemType<TerraShiv>(), 0.5f },
            { ModContent.ItemType<Lucrecia>(), 0.2f },
            { ModContent.ItemType<Disease>(), 0.5f },
            { ModContent.ItemType<ElementalShiv>(), 0.5f },
            { ModContent.ItemType<GalileoGladius>(), 0.5f },
            { ModContent.ItemType<CosmicShiv>(), 0.5f },
            { ModContent.ItemType<ExoGladius>(), 0.3f },
        };*/
        public int shortstrikeCharge;
        public int hitCount;
        public override void PostUpdateEquips()
        {
            if (shortswords.ContainItem(Player.HeldItem.type))
            {
                for (int i = 1; i < shortstrikeCharge; i++)
                {
                    if (i % 5 == 0)
                        Player.GetDamage<MeleeDamageClass>() += 0.01f;
                    if (i % 3 == 0)
                        Player.statDefense++;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            shortstrikeCharge = (int)MathHelper.Clamp(shortstrikeCharge, 0, 100);
            CooldownInstance cooldownInstance;
            if (Player.Calamity().cooldowns.TryGetValue(ShortstrikeCharge.ID, out cooldownInstance))
                cooldownInstance.timeLeft = shortstrikeCharge;

            if (shortstrikeCharge > 0)
            {
                Player.AddCooldown(ShortstrikeCharge.ID, 100).timeLeft = this.shortstrikeCharge;
            }
        }
        public override void PreUpdate()
        {
            if (Player.whoAmI == Main.myPlayer)
            {
                if (autoRevertSelectedItem)
                {
                    if (Player.itemTime == 0 && Player.itemAnimation == 0)
                    {
                        Player.selectedItem = originalSelectedItem;
                        autoRevertSelectedItem = false;
                    }
                }
            }
            if (hitCount > 0)
            {
                if (hitCount == shortswords.GetHitCount(Player.HeldItem.type))
                {
                    Player.HeldItem.useTime /= 2;
                    Player.HeldItem.useAnimation /= 2;
                }
                if (hitCount == 1)
                {
                    Player.HeldItem.useTime *= 2;
                    Player.HeldItem.useAnimation *= 2;
                }
                hitCount--;
                QuickUseItemInSlot(Player.selectedItem);
            }
        }
        internal int originalSelectedItem;
        internal bool autoRevertSelectedItem = false;
        public void QuickUseItemInSlot(int index)
        {
            if (index > -1 && index < Main.InventorySlotsTotal && Player.inventory[index].type != ItemID.None)
            {
                if (Player.CheckMana(Player.inventory[index], -1, false, false))
                {
                    originalSelectedItem = Player.selectedItem;
                    autoRevertSelectedItem = true;
                    Player.selectedItem = index;
                    Player.controlUseItem = true;
                    Player.ItemCheck();
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Drip with { Variants = stackalloc int[] { 0, 1, 2 } }, Player.Center);
                }
            }
        }
    }
    public class ShortstrikeGlobalItem : GlobalItem
    {
        private ShortstrikePlayer SSPlayer(Player player) => player.GetModPlayer<ShortstrikePlayer>();
        public bool CanShortfurry(Player player)
        {
            return SSPlayer(player).shortstrikeCharge == 100 && SSPlayer(player).hitCount == 0;
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (ShortstrikePlayer.shortswords.ContainItem(item.type))
                return CanShortfurry(player);
            return base.AltFunctionUse(item, player);
        }
        public override bool? UseItem(Item item, Player player)
        {
            if (ShortstrikePlayer.shortswords.ContainItem(item.type) && player.altFunctionUse == 2)
            {
                if (CanShortfurry(player))
                {
                    player.GetModPlayer<ShortstrikePlayer>().hitCount = ShortstrikePlayer.shortswords.GetHitCount(item.type);
                    player.GetModPlayer<ShortstrikePlayer>().shortstrikeCharge = 0;
                }
                return false;
            }
            return base.UseItem(item, player);
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //float percent = SSPlayer(player).shortstrikeCharge / 100;

            if (player.GetModPlayer<ShortstrikePlayer>().hitCount > 0)
            {
                damage = (int)(damage * (1f + ShortstrikePlayer.shortswords.GetDamageMult(item.type)));
            }
        }
    }
    public class ShortstrikeGlobalProjectile : GlobalProjectile
    {
        /*public static List<int> shortswords = new List<int>()
        {
            ProjectileID.CopperShortswordStab, ProjectileID.TinShortswordStab,
            ProjectileID.IronShortswordStab, ProjectileID.LeadShortswordStab,
            ProjectileID.SilverShortswordStab, ProjectileID.TungstenShortswordStab,
            ProjectileID.GoldShortswordStab, ProjectileID.PlatinumShortswordStab,
            ProjectileID.GladiusStab
        };*/
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (ShortstrikePlayer.shortswords.ContainProjectile(projectile.type))
            {
                if (player.GetModPlayer<ShortstrikePlayer>().hitCount == 0)
                    player.GetModPlayer<ShortstrikePlayer>().shortstrikeCharge++;
            }
        }
    }
}
