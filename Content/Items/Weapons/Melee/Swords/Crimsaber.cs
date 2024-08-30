using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Melee.Swords
{
    public class Crimsaber : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 56; Item.height = 70;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;

            Item.useAnimation = Item.useTime = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 60;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 7f;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                int num = Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center + new Vector2(Main.rand.NextFloat(-50, 50), Main.rand.NextFloat(-300, -225)), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(5f, 7f)), ProjectileID.IchorSplash, Item.damage, Item.knockBack, player.whoAmI);
                Main.projectile[num].tileCollide = true;
                Main.projectile[num].penetrate = 1;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bladetongue)
                .AddIngredient(ItemID.CrimtaneBar, 20)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.Ichor, 5)
                .AddIngredient<CoreofHavoc>(3)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
