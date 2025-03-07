using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Melee.Swords
{
    public class BladeOfEnmity : ModItem, ILocalizedModType, IModType
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
            Item.width = 60; Item.height = 64;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;

            Item.useAnimation = Item.useTime = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 190;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 8f;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Vector2.Zero, ProjectileID.Volcano, hit.Damage, hit.Knockback, player.whoAmI); ;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-10f, 0f);
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FieryGreatsword)
                .AddIngredient<LifeAlloy>(5)
                .AddIngredient<CoreofCalamity>(3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
