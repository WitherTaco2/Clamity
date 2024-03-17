using CalamityMod.Items;
using Clamity.Content.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.KosFragmentBosses.CeasingVoid.Drops
{
    public class SteelBlade : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = Item.height = 72;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;

            Item.useAnimation = Item.useTime = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 7f;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Item.GetSource_FromThis(), target.Center, Vector2.UnitX.RotatedByRandom(0.5f) * 0.1f, ModContent.ProjectileType<SteelBladeSlash>(), (int)(damageDone * 0.5f), Item.knockBack, player.whoAmI);
        }
    }
    public class SteelBladeSlash : BaseSlash
    {
        public override float Scale => 0.5f;
        public override Color FirstColor => Color.White;
        public override Color SecondColor => Color.LightGray;
    }
}
