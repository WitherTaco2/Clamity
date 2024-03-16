using CalamityMod;
using CalamityMod.Cooldowns;
using Clamity.Content.Buffs.Shortstrike;
using Clamity.Content.Cooldowns;
using Clamity.Content.Items.Weapons.Melee.Shortswords;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Commons
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
            proj = -1;
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

            new ShortstrikeInfo(ModContent.ItemType<WulfrumLeechDagger>(), ModContent.ProjectileType<WulfrumLeechDaggerProjectile>(), 2, 0f),
            new ShortstrikeInfo(ModContent.ItemType<SporeKnife>(), ModContent.ProjectileType<SporeKnifeProjectile>(), 4, 0.2f),
            new ShortstrikeInfo(ModContent.ItemType<Tomutus>(), ModContent.ProjectileType<TomutusProjectile>(), 3, 0.3f),

            new ShortstrikeInfo(ModContent.ItemType<ColdheartIcicle>(), ModContent.ProjectileType<ColdheartIcicleProjectile>(), 2, 0f),
            new ShortstrikeInfo(ModContent.ItemType<Caliburn>(), ModContent.ProjectileType<CaliburnProjectile>(), 4, 0.15f),
            new ShortstrikeInfo(ModContent.ItemType<TrueCaliburn>(), ModContent.ProjectileType<TrueCaliburnProjectile>(), 4, 0.2f),
            new ShortstrikeInfo(ModContent.ItemType<TerraShiv>(), ModContent.ProjectileType<TerraShivProjectile>(), 3, 0.2f),
            new ShortstrikeInfo(ModContent.ItemType<Disease>(), ModContent.ProjectileType<DiseaseProjectile>(), 3, 0.2f),
            new ShortstrikeInfo(ModContent.ItemType<Everest>(), ModContent.ProjectileType<EverestProjectile>(), 5, 0.1f),
            new ShortstrikeInfo(ModContent.ItemType<ExoGladius>(), ModContent.ProjectileType<ExoGladiusProjectile>(), 3, 0.3f),

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
        public bool CanShortfurry()
        {
            return shortstrikeCharge == 100 && hitCount == 0;
        }
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
                Player.AddCooldown(ShortstrikeCharge.ID, 100).timeLeft = shortstrikeCharge;
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
                    Player.HeldItem.shootSpeed /= 2;
                }
                if (hitCount == 1)
                {
                    Player.HeldItem.useTime *= 2;
                    Player.HeldItem.useAnimation *= 2;
                    Player.HeldItem.shootSpeed *= 2;
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
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (ShortstrikePlayer.shortswords.ContainItem(item.type))
                return player.SSPlayer().CanShortfurry();
            return base.AltFunctionUse(item, player);
        }
        public override bool? UseItem(Item item, Player player)
        {
            if (ShortstrikePlayer.shortswords.ContainItem(item.type) && player.altFunctionUse == 2)
            {
                if (player.SSPlayer().CanShortfurry())
                {
                    Shortstrike(player, item, ModContent.BuffType<CopperShortstrike>(), 5, ItemID.CopperShortsword);
                    Shortstrike(player, item, ModContent.BuffType<TinShortstrike>(), 5, ItemID.TinShortsword);
                    Shortstrike(player, item, ModContent.BuffType<IronShortstrike>(), 5, ItemID.IronShortsword);
                    Shortstrike(player, item, ModContent.BuffType<LeadShortstrike>(), 5, ItemID.LeadShortsword);
                    Shortstrike(player, item, ModContent.BuffType<SilverShortstrike>(), 5, ItemID.SilverShortsword);
                    Shortstrike(player, item, ModContent.BuffType<TungstenShortstrike>(), 5, ItemID.TungstenShortsword);
                    Shortstrike(player, item, ModContent.BuffType<GoldShortstrike>(), 10, ItemID.GoldShortsword);
                    Shortstrike(player, item, ModContent.BuffType<PlatinumShortstrike>(), 10, ItemID.PlatinumShortsword);
                    Shortstrike(player, item, ModContent.BuffType<GladiusShortstrike>(), 5, ItemID.Gladius, 1.25f);

                    player.GetModPlayer<ShortstrikePlayer>().hitCount = ShortstrikePlayer.shortswords.GetHitCount(item.type);
                    player.GetModPlayer<ShortstrikePlayer>().shortstrikeCharge = 0;

                }
                return false;
            }
            return base.UseItem(item, player);
        }
        private void Shortstrike(Player player, Item item, int buffID, float timeInSeconds, int itemID, float percent = 2)
        {
            if (item.type == itemID)
            {
                player.AddBuff(buffID, CalamityUtils.SecondsToFrames(timeInSeconds));
                for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.PiOver4 / 3)
                {
                    //Dust dust = Dust.NewDustPerfect(proj.Center + proj.velocity, DustID.Electric, Vector2.UnitX.RotatedBy(i) * 3f + proj.velocity);
                    Vector2 velocity = player.SafeDirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero) * item.shootSpeed;
                    Dust dust = Dust.NewDustPerfect(player.Center + velocity, DustID.Electric, Vector2.UnitX.RotatedBy(i) * 3f + velocity);
                    dust.noGravity = true;
                }
            }
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            //float percent = SSPlayer(player).shortstrikeCharge / 100;

            if (player.GetModPlayer<ShortstrikePlayer>().hitCount > 0)
            {
                damage = (int)(damage * (1f + ShortstrikePlayer.shortswords.GetDamageMult(item.type)));
                velocity *= 1.5f;
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
        public override bool InstancePerEntity => true;
        public bool shortstrike = false;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (ShortstrikePlayer.shortswords.ContainProjectile(projectile.type))
            {
                if (player.GetModPlayer<ShortstrikePlayer>().hitCount == 0)
                    player.GetModPlayer<ShortstrikePlayer>().shortstrikeCharge++;
            }
        }
        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (player.SSPlayer().hitCount > 0)
                shortstrike = true;
        }
    }
}
