using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.WoB.Drop
{
    public class HeavyMetalTrashCan : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Pets";
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<HeavyMetalTrashCanPet>(), ModContent.BuffType<HeavyMetalTrashCanBuff>());
            Item.width = 26;
            Item.height = 32;
            Item.rare = 15;
            Item.value = Item.buyPrice(gold: 12);
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.expert = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2); // The item applies the buff, the buff spawns the projectile

            return false;
        }
    }
    public class HeavyMetalTrashCanPet : ModProjectile
    {
        public bool FloatingDestination
        {
            get => (double)Projectile.ai[0] == 1.0;
            set => Projectile.ai[0] = value ? 1f : 0.0f;
        }

        public override void SetStaticDefaults() => Main.projPet[Projectile.type] = true;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(269);
            AIType = -1;
        }

        public override void AI()
        {
            Terraria.Player player = Main.player[Projectile.owner];
            CheckActive(player);
            Movement(player);
        }

        private void CheckActive(Terraria.Player player)
        {
            if (player.dead || !player.HasBuff(ModContent.BuffType<HeavyMetalTrashCanBuff>()))
                return;
            Projectile.timeLeft = 2;
        }

        private void Movement(Terraria.Player player)
        {
            int direction = player.direction;
            Vector2 vector2_1 = new((float)(direction * 40), -40f);
            Vector2 vector2_2 = this.FindDesiredCenter(player) ?? (player.MountedCenter + vector2_1);
            if (!HeavyMetalTrashCanPet.IsStanding(player) && !HeavyMetalTrashCanPet.IsStanding(Projectile))
                vector2_2 = player.MountedCenter + vector2_1;
            Vector2 vector2_3 = vector2_2 - Projectile.Center;
            float num = vector2_3.LengthSquared();
            if (vector2_3 != Vector2.Zero && (num > 6400f || !HeavyMetalTrashCanPet.IsStanding(Projectile)))
                Projectile.velocity = vector2_3 * 0.1f * 2f;
            if (num > 4000000f)
            {
                Projectile.Bottom = vector2_2;
                Projectile.velocity = Vector2.Zero;
            }
            else if (num > 102400f)
            {
                Projectile projectile = Projectile;
                projectile.velocity *= 5f;
            }
            Projectile.rotation = 0.0f;
        }

        private Vector2? FindDesiredCenter(Terraria.Player player)
        {
            Vector2? desiredCenter = new Vector2?();
            for (int index1 = (int)player.MountedCenter.X - 128; index1 < (int)player.MountedCenter.X + 128; ++index1)
            {
                for (int index2 = (int)player.MountedCenter.Y - 128; index2 < (int)player.MountedCenter.Y + 128; ++index2)
                {
                    if (Main.maxTilesX >= index1 / 16 && Main.maxTilesY >= index2 / 16 && Main.tile[index1 / 16, index2 / 16] != null)
                    {
                        Tile tile = Main.tile[index1 / 16, index2 / 16];
                        if (tile.HasTile)
                        {
                            bool[] tileSolid1 = Main.tileSolid;
                            tile = Main.tile[index1 / 16, index2 / 16];
                            int index3 = (int)tile.TileType;
                            if (!tileSolid1[index3])
                            {
                                bool[] tileSolidTop = Main.tileSolidTop;
                                tile = Main.tile[index1 / 16, index2 / 16];
                                int index4 = (int)tile.TileType;
                                if (!tileSolidTop[index4])
                                    continue;
                            }
                            tile = Main.tile[index1 / 16, index2 / 16 - 1];
                            if (tile.HasTile)
                            {
                                bool[] tileSolid2 = Main.tileSolid;
                                tile = Main.tile[index1 / 16, index2 / 16 - 1];
                                int index5 = (int)tile.TileType;
                                if (!tileSolid2[index5])
                                {
                                    bool[] tileSolidTop = Main.tileSolidTop;
                                    tile = Main.tile[index1 / 16, index2 / 16 - 1];
                                    int index6 = (int)tile.TileType;
                                    if (!tileSolidTop[index6])
                                        continue;
                                }
                            }
                            Vector2 vector2_1 = new((float)index1, (float)index2);
                            Vector2? nullable1 = desiredCenter;
                            Vector2 center = Projectile.Center;
                            Vector2? nullable2 = nullable1.HasValue ? nullable1.GetValueOrDefault() - center : new Vector2?();
                            Vector2 vector2_2 = vector2_1 - Projectile.Center;
                            if (desiredCenter.HasValue)
                            {
                                center = nullable2.Value;
                                if (center.LengthSquared() <= vector2_2.LengthSquared())
                                    continue;
                            }
                            desiredCenter = new Vector2?(vector2_1);
                        }
                    }
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
                this.FloatingDestination = !desiredCenter.HasValue;
            return desiredCenter;
        }

        public static bool IsStanding(Terraria.Player player)
        {
            int x = (int)player.Center.X;
            int y = (int)player.Bottom.Y;
            Tile tile = Main.tile[x / 16, y / 16];
            if (!(tile != (ArgumentException)null) || !tile.HasTile)
                return false;
            return Main.tileSolid[(int)tile.TileType] || Main.tileSolidTop[(int)tile.TileType];
        }

        public static bool IsStanding(Projectile projectile)
        {
            int x = (int)projectile.Center.X;
            int y = (int)projectile.Bottom.Y;
            Tile tile = Main.tile[x / 16, y / 16];
            if (!(tile != (ArgumentException)null) || !tile.HasTile)
                return false;
            return Main.tileSolid[(int)tile.TileType] || Main.tileSolidTop[(int)tile.TileType];
        }

        private void Animate(bool movesFast)
        {
            int num = 7;
            if (movesFast)
                num = 4;
            ++Projectile.frameCounter;
            if (Projectile.frameCounter <= num)
                return;
            Projectile.frameCounter = 0;
            ++Projectile.frame;
            if (Projectile.frame < Main.projFrames[Projectile.type])
                return;
            Projectile.frame = 0;
        }
    }
    public class HeavyMetalTrashCanBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        { // This method gets called every frame your buff is active on your player.
            player.buffTime[buffIndex] = 18000;

            int projType = ModContent.ProjectileType<HeavyMetalTrashCanPet>();

            // If the player is local, and there hasn't been a pet projectile spawned yet - spawn it.
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0)
            {
                var entitySource = player.GetSource_Buff(buffIndex);

                Projectile.NewProjectile(entitySource, player.Center, Vector2.Zero, projType, 0, 0f, player.whoAmI);
            }
        }
    }
}
