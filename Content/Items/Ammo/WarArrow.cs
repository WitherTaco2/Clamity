using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Clamity.Content.Items.Ammo
{
    public class WarArrow : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Ammo.Dart";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CrystalDart);
            Item.damage = 28;
            Item.shoot = ModContent.ProjectileType<ProfanedDartProjectile>();
            Item.rare = ItemRarityID.Purple;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100)
                .AddIngredient(ItemID.CrystalDart, 100)
                .AddIngredient<UnholyEssence>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class WarArrowProjectile : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<WarArrow>().Texture;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
        }
        /*public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<>)
        }*/
    }
}
