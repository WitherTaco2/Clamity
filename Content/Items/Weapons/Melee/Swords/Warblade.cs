using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Melee.Swords
{
    public class Warblade : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 34; Item.height = 42;
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.scale = 1.5f;

            Item.useTime = Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 27;
            Item.knockBack = 4.75f;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 450);
        }
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
        }
    }
}
