using CalamityMod.Items;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Clamity.Content.Biomes.FrozenHell.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Items.Weapons.Melee.Swords
{
    [LegacyName("FrozenVolcano")]
    public class GreatestPeak : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";

        public override void SetDefaults()
        {
            Item.damage = 500;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.rare = ModContent.RarityType<Violet>();
            Item.width = Item.height = 80;
            Item.scale = 1.5f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
        }

        /*public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(323, 360);
            int damage = player.CalcIntDamage<MeleeDamageClass>(base.Item.damage);
            player.ApplyDamageToNPC(target, damage, 0f, 0);
            float scale = 1.7f;
            float scale2 = 0.8f;
            float scale3 = 2f;
            Vector2 vector = (target.rotation - MathF.PI / 2f).ToRotationVector2() * target.velocity.Length();
            SoundEngine.PlaySound(in SoundID.Item14, target.Center);
            for (int i = 0; i < 40; i++)
            {
                int num = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 174, 0f, 0f, 200, default(Color), scale);
                Dust obj = Main.dust[num];
                obj.position = target.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * target.width / 2f;
                obj.noGravity = true;
                obj.velocity.Y -= 6f;
                obj.velocity *= 3f;
                obj.velocity += vector * Main.rand.NextFloat();
                num = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 174, 0f, 0f, 100, default(Color), scale2);
                obj.position = target.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * target.width / 2f;
                obj.velocity.Y -= 6f;
                obj.velocity *= 2f;
                obj.noGravity = true;
                obj.fadeIn = 1f;
                obj.color = Color.Crimson * 0.5f;
                obj.velocity += vector * Main.rand.NextFloat();
            }

            for (int j = 0; j < 20; j++)
            {
                int num2 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 174, 0f, 0f, 0, default(Color), scale3);
                Dust obj2 = Main.dust[num2];
                obj2.position = target.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(target.velocity.ToRotation()) * target.width / 3f;
                obj2.noGravity = true;
                obj2.velocity.Y -= 6f;
                obj2.velocity *= 0.5f;
                obj2.velocity += vector * (0.6f + 0.6f * Main.rand.NextFloat());
            }
        }*/

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Hellkite>()
                .AddIngredient<UltimusCleaver>()
                .AddIngredient<EndobsidianBar>(8)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
