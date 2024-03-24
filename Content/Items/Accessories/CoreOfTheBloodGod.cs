using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Accessories
{
    public class CoreOfTheBloodGod : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories";

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 48;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();

            modPlayer.fleshTotem = true;
            modPlayer.healingPotionMultiplier += 0.25f;

            player.Calamity().bloodyWormTooth = true;
            player.endurance += 0.1f;

            modPlayer.voidOfCalamity = true;
            player.GetDamage<GenericDamageClass>() += 0.2f;
            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_Accessory(Item);
                if (player.immune)
                {
                    if (player.miscCounter % 10 == 0)
                    {
                        int damage = (int)player.GetBestClassDamage().ApplyTo(30);
                        CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<StandingFire>(), damage, 5f, player.whoAmI);
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FleshTotem>()
                .AddIngredient<BloodyWormScarf>()
                .AddIngredient<VoidofCalamity>()
                .AddIngredient<BloodstoneCore>(4)
                .AddIngredient<AscendantSpiritEssence>(5)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
