using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;

namespace Clamity.Content.Items.Ammo
{
    public class WarArrow : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Ammo.Arrow";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenArrow);
            Item.damage = 9;
            Item.shoot = ModContent.ProjectileType<WarArrowProjectile>();
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe(250)
                .AddIngredient(ItemID.WoodenArrow, 250)
                .AddIngredient<DepthCells>()
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 240);
        }
        public override void PostAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
        }
    }
}
