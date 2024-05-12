using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Tools.BreakingTool
{
    public class Waraxe : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Tools";
        public override void SetDefaults()
        {
            Item.width = 32; Item.height = 40;
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.scale = 1.5f;

            Item.useTime = 21;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 26;
            Item.knockBack = 5.25f;
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
