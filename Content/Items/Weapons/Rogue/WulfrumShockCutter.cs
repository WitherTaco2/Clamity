using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Rogue
{
    public class WulfrumShockCutter : WulfrumKnife
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.consumable = false;
            Item.maxStack = 1;
            Item.shoot = ModContent.ProjectileType<WulfrumShockCutterProjectile>();
        }
        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WulfrumKnife>(250)
                .AddIngredient<EnergyCore>()
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class WulfrumShockCutterProjectile : WulfrumKnifeProj
    {
        public override string Texture => ModContent.GetInstance<WulfrumShockCutter>().Texture;
    }
}
