using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Clamity.Content.Items.Pets.MasterRev.DoG
{
    public class CosmicGuardianItem : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Pets";
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DukeFishronPetItem);
            Item.shoot = ModContent.ProjectileType<CosmicGuardianPet>();
            Item.buffType = ModContent.BuffType<CosmicGuardianBuff>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }
    }
    public class CosmicGuardianBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.Clamity().cosmicGuardian = true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CosmicGuardianPet>()] <= 0 && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<CosmicGuardianPet>(), 0, 0f, player.whoAmI);
            }
        }
    }
    public class CosmicGuardianPet : ModProjectile 
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
            //ProjectileID.Sets.LightPet[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 211;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EaterOfWorldsPet);
            AIType = ProjectileID.EaterOfWorldsPet;
        }

        public override bool PreAI()
        {
            Main.player[Projectile.owner].petFlagEaterOfWorldsPet = false;
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            ClamityPlayer modPlayer = player.Clamity();
            if (player.dead)
            {
                modPlayer.cosmicGuardian = false;
            }
            if (modPlayer.cosmicGuardian)
            {
                Projectile.timeLeft = 2;
            }

            Vector2 value2 = Projectile.Center;
            int num4 = Main.projFrames[Projectile.type] - 1;
            for (int i = 1; i < segments; i++)
            {
                Vector2 value4 = Projectile.oldPos[i * 10] + Projectile.Size / 2f;
                float num5 = (value2 - value4).ToRotation();
                value4 = value2 - new Vector2(distance, 0f).RotatedBy(num5, Vector2.Zero);
                //Lighting.AddLight(value4, 0.8f, 0.8f, 0f);
            }

            //Lighting.AddLight(Projectile.Center, 0.8f, 0.8f, 0f);
        }

        const int segments = 20;
        const int distance = 16;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            //Texture2D glow = ModContent.Request<Texture2D>($"{Texture}_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            SpriteEffects effects = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle rectangle = texture.Frame(1, Main.projFrames[Projectile.type]);
            Vector2 origin = rectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Color alpha = Projectile.GetAlpha(Lighting.GetColor(Projectile.Center.ToTileCoordinates()));
            Color color = Color.White * (Main.mouseTextColor / 255f);
            Vector2 value2 = Projectile.Center;
            int num3 = 1;
            int num4 = Main.projFrames[Projectile.type] - 1;
            for (int i = 1; i < segments; i++)
            {
                int frameY = num3;
                frameY += (int)(i % 2);
                if (i == segments - 1)
                    frameY = num4;

                Rectangle value3 = texture.Frame(1, Main.projFrames[Projectile.type], 0, frameY);
                Vector2 value4 = Projectile.oldPos[i * 10] + Projectile.Size / 2f;
                float num5 = (value2 - value4).ToRotation();
                value4 = value2 - new Vector2(distance, 0f).RotatedBy(num5, Vector2.Zero);
                num5 = (value2 - value4).ToRotation() + (float)Math.PI / 2f;
                Vector2 position2 = value4 - Main.screenPosition;
                SpriteEffects effects2 = !(value4.X < value2.X) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                value2 = value4;
                Main.EntitySpriteDraw(texture, position2, value3, Projectile.GetAlpha(Lighting.GetColor(value4.ToTileCoordinates())), num5, origin, Projectile.scale, effects2, 0);
                //Main.EntitySpriteDraw(glow, position2, value3, Projectile.GetAlpha(color), num5, origin, Projectile.scale, effects2, 0);
            }

            Main.EntitySpriteDraw(texture, position, rectangle, alpha, Projectile.rotation, origin, Projectile.scale, effects, 0);
            //Main.EntitySpriteDraw(glow, position, rectangle, color, Projectile.rotation, origin, Projectile.scale, effects, 0);

            return false;
        }
    }
}
