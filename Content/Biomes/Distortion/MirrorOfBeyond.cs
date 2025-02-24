using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion
{
    public class MirrorOfBeyond : ModItem
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 10;
            Item.rare = ModContent.RarityType<Turquoise>();

            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool? UseItem(Player player)
        {
            if (!SubworldSystem.IsActive<TheDistortion>())
                SubworldSystem.Enter<TheDistortion>();
            else
                SubworldSystem.Exit();
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            //TODO - make animation later
            //if (player.ownedProjectileCounts[Item.shoot] >= 1)
            //    return false;

            // Entering/exiting subworlds appears to reset the mouse item for some reason, meaning that if you use this item
            // that way it'll be functionally distroyed, which we don't want.
            if (!Main.mouseItem.IsAir && Main.mouseItem.type == Type)
                return false;

            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Vector2 center = Item.position - Main.screenPosition;

            //Drawing a rings
            string path = GetType().Namespace.Replace('.', '/');
            Texture2D t = ModContent.Request<Texture2D>(path + "/DistPortal_SmallRing").Value;
            Texture2D t2 = ModContent.Request<Texture2D>(path + "/DistPortal_BigRing").Value;
            spriteBatch.Draw(t, center, null, lightColor, Main.GlobalTimeWrappedHourly, t.Size() / 2, 0.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(t2, center, null, lightColor, -Main.GlobalTimeWrappedHourly * 0.5f, t2.Size() / 2, 0.5f, SpriteEffects.None, 0);

            //Drawing an item itself
            Texture2D t3 = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(t3, center + new Vector2(0, MathF.Sin(Main.GlobalTimeWrappedHourly * 2) * 4), null, lightColor, -MathHelper.PiOver4, t3.Size() / 2, scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
