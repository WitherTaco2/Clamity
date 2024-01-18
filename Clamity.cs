using System.Collections.Generic;
using System;
using Clamity.Content.Boss.Clamitas;
using Clamity.Content.Boss.Clamitas.Drop;
using Clamity.Content.Cooldowns;
using CalamityMod.Cooldowns;
using CalamityMod.UI.CalamitasEnchants;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.NPCs.CalClone;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Clamity.Content.Boss.Pyrogen.NPCs;
using Clamity.Content.Boss.Pyrogen;
using Clamity.Content.Boss.Clamitas.NPCs;
using Clamity.Content.Boss.Pyrogen.Drop;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Clamity.Content.Boss.WoB.NPCs;
using Clamity.Content.Boss.WoB;
using Clamity.Content.Boss.WoB.Drop;
using CalamityMod.Skies;
using Terraria.Graphics.Effects;
using Clamity.Content.Boss.WoB.FrozenHell.Biome.Background;
using CalamityMod.Events;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod.CalPlayer;

namespace Clamity
{
    public class Clamity : Mod
	{
        public static Clamity mod; 
        public static Mod musicMod;
        internal bool MusicAvailable => musicMod != null;

        public override void Load()
        {
            mod = this; 
            ModLoader.TryGetMod("ClamityMusic", out musicMod);

            

            ModLoader.GetMod("CalamityMod").Call(
                "CreateEnchantment",
                ClamityUtils.GetText("UI.Enchantments.AflameAcc.DisplayName"),
                ClamityUtils.GetText("UI.Enchantments.AflameAcc.Description"),
                10000,
                new Predicate<Item>(EnchantableAcc),
                "CalamityMod/UI/CalamitasEnchantments/CurseIcon_Aflame",
                delegate (Player player) { player.Clamity().aflameAcc = true; }

            );
            LoadCooldowns();
            LoadBossRush();
            NewNPCStats.Load();
            if (!Main.dedServ)
            {
                LoadShaders();
            }
        }
        private void LoadBossRush()
        {
            /*BossRushEvent.Bosses[12] = new BossRushEvent.Boss(
                113,
                BossRushEvent.TimeChangeContext.None,
                (type =>
                {
                    Terraria.Player player = Main.player[BossRushEvent.ClosestPlayerToWorldCenter];
                    player.Teleport(new Vector2(100, Main.UnderworldLayer + 40) * 16);
                    //bool a = true;
                    //player.Teleport(player.CheckForGoodTeleportationSpot(ref a, 1600, 100, (Main.UnderworldLayer + 40) * 16, 100, new Player.RandomTeleportationAttemptSettings()));

                    NPC.SpawnWOF(Main.player[BossRushEvent.ClosestPlayerToWorldCenter].position);
                }),
                -1,
                false,
                0.0f,
                114,
                117,
                118,
                119,
                115,
                116
            );*/
            BossRushEvent.Bosses.Insert(15, 
                new BossRushEvent.Boss(
                    ModContent.NPCType<PyrogenBoss>(), 
                    BossRushEvent.TimeChangeContext.None, 
                    (BossRushEvent.Boss.OnSpawnContext)null, 
                    -1, 
                    false, 
                    0.0f, 
                    ModContent.NPCType<PyrogenShield>()
                )
            );
            BossRushEvent.Bosses.Insert(25,
                new BossRushEvent.Boss(
                    ModContent.NPCType<ClamitasBoss>(),
                    BossRushEvent.TimeChangeContext.None,
                    (BossRushEvent.Boss.OnSpawnContext)null,
                    -1,
                    false,
                    0.0f
                )
            );
            BossRushEvent.Bosses.Insert(43,
                new BossRushEvent.Boss(
                    ModContent.NPCType<WallOfBronze>(),
                    BossRushEvent.TimeChangeContext.None,
                    (type =>
                    {
                        Terraria.Player player = Main.player[BossRushEvent.ClosestPlayerToWorldCenter];
                        //player.Teleport(new Vector2(100, Main.UnderworldLayer + 40) * 16);
                        //bool a = true;
                        //player.Teleport(player.CheckForGoodTeleportationSpot(ref a, 1600, 100, (Main.UnderworldLayer + 40) * 16, 100, new Player.RandomTeleportationAttemptSettings()));
                        
                        Vector2? underworldPosition = CalamityPlayer.GetUnderworldPosition(player);
                        if (underworldPosition.HasValue)
                        {
                            CalamityPlayer.ModTeleport(player, underworldPosition.Value, false, 2);
                            SoundStyle style = BossRushEvent.TeleportSound with
                            {
                                Volume = 1.6f
                            };
                        }


                        int center = Main.maxTilesX * 16 / 2;
                        NPC.NewNPC(player.GetSource_FromThis(), (int)player.Center.X - 1000 * (player.Center.X > center ? -1 : 1), (int)player.Center.Y, ModContent.NPCType<WallOfBronze>());
                        SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/SCalSounds/SepulcherSpawn"), player.Center);

                    }),
                    -1,
                    true,
                    0.0f,
                    ModContent.NPCType<WallOfBronzeClaw>(),
                    ModContent.NPCType<WallOfBronzeLaser>(),
                    ModContent.NPCType<WallOfBronzeTorret>()
                )
            );
        }
        private void LoadShaders()
        {
                Filters.Scene["Clamity:FrozenHellSky"] = new Filter(new MonolithScreenShaderData("FilterMiniTower").UseColor(1.1f, 0.3f, 0.3f).UseOpacity(0.65f), EffectPriority.VeryHigh);
                SkyManager.Instance["Clamity:FrozenHellSky"] = (CustomSky)new FrozenHellSky();
        }
        private void LoadCooldowns()
        {
            CooldownRegistry.Register<ShortstrikeCooldown>(ShortstrikeCooldown.ID);
            CooldownRegistry.Register<PyrospearCooldown>(PyrospearCooldown.ID);
            CooldownRegistry.Register<ShortstrikeCharge>(ShortstrikeCharge.ID);
        }
        private static bool EnchantableAcc(Item item) => !item.IsAir && item.maxStack == 1 && item.ammo == AmmoID.None && item.accessory;
        public override void Unload()
        {
            mod = null;
            musicMod = null;

            NewNPCStats.UnLoad();
        }
        public override void PostSetupContent()
        {
            ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist);

            List<int> intList14 = new List<int>()
            {
                ModContent.ItemType<ClamitasRelic>(),
                ModContent.ItemType<LoreWhat>()
            };
            if (Clamity.musicMod != null)
            {
                intList14.Add(Clamity.musicMod.Find<ModItem>("ClamitasMusicBox").Type);
            }
            Action<SpriteBatch, Rectangle, Color> action2 = (Action<SpriteBatch, Rectangle, Color>)((sb, rect, color) =>
            {
                Texture2D texture2D = ModContent.Request<Texture2D>(ModContent.GetInstance<ClamitasBoss>().Texture + "_BossChecklist", (AssetRequestMode)2).Value;
                Vector2 vector2 = new(rect.Center.X - texture2D.Width / 2, rect.Center.Y - texture2D.Height / 2);
                sb.Draw(texture2D, vector2, color);
            });
            AddBoss(bossChecklist, mod, "Clamitas", 11.9f, ModContent.NPCType<ClamitasBoss>(), () => ClamitySystem.downedClamitas, new Dictionary<string, object>()
            {
                ["spawnItems"] = (object)ModContent.ItemType<ClamitasSummoningItem>(),
                ["collectibles"] = (object)intList14,
                ["customPortrait"] = (object)action2
            });


            List<int> intList15 = new List<int>()
            {
                ModContent.ItemType<PyrogenRelic>(),
                ModContent.ItemType<LorePyrogen>(),
                /*ModContent.ItemType<ClamitasMusicbox>()*/
            };
            if (Clamity.musicMod != null)
            {
                intList15.Add(Clamity.musicMod.Find<ModItem>("PyrogenMusicBox").Type);
            }
            Action<SpriteBatch, Rectangle, Color> action3 = (Action<SpriteBatch, Rectangle, Color>)((sb, rect, color) =>
            {
                Texture2D texture2D = ModContent.Request<Texture2D>(ModContent.GetInstance<PyrogenBoss>().Texture + "_BossChecklist", (AssetRequestMode)2).Value;
                Vector2 vector2 = new(rect.Center.X - texture2D.Width / 2, rect.Center.Y - texture2D.Height / 2);
                sb.Draw(texture2D, vector2, color);
            });
            AddBoss(bossChecklist, mod, "Pyrogen", 8.5f, ModContent.NPCType<PyrogenBoss>(), () => ClamitySystem.downedPyrogen, new Dictionary<string, object>()
            {
                ["spawnItems"] = (object)ModContent.ItemType<PyroKey>(),
                ["collectibles"] = (object)intList15,
                ["customPortrait"] = (object)action3
            });


            List<int> intList16 = new List<int>()
            {
                ModContent.ItemType<WoBLore>(),

            };
            Action<SpriteBatch, Rectangle, Color> action4 = (Action<SpriteBatch, Rectangle, Color>)((sb, rect, color) =>
            {
                Texture2D texture2D = ModContent.Request<Texture2D>(ModContent.GetInstance<WallOfBronze>().Texture + "_BossChecklist", (AssetRequestMode)2).Value;
                Vector2 vector2 = new(rect.Center.X - texture2D.Width / 2, rect.Center.Y - texture2D.Height / 2);
                sb.Draw(texture2D, vector2, color);
            });
            AddBoss(bossChecklist, mod, "WallOfBronze", 22.5f, ModContent.NPCType<WallOfBronze>(), () => ClamitySystem.downedWallOfBronze, new Dictionary<string, object>()
            {
                ["spawnItems"] = (object)ModContent.ItemType<AncientConsole>(),
                ["collectibles"] = (object)intList16,
                ["customPortrait"] = (object)action4
            });

            /*EnchantmentManager.EnchantmentList.Add(
                new Enchantment(
                    ClamityUtils.GetText("UI.Enchantments.AflameAcc.DisplayName"),
                    ClamityUtils.GetText("UI.Enchantments.AflameAcc.Description"),
                    10000,
                    "CalamityMod/UI/CalamitasEnchantments/CurseIcon_Aflame",
                    (player => player.Clamity().aflameAcc = true),
                    (item => item.IsEnchantable() && item.accessory
                    //&& ClamityUtils.ContainType(item.type, ModContent.ItemType<LuxorsGift>(), ModContent.ItemType<FungalClump>(), ModContent.ItemType<WifeinaBottle>(), ModContent.ItemType<EyeoftheStorm>(), ModContent.ItemType<RoseStone>(), ModContent.ItemType<PearlofEnthrallment>(), ModContent.ItemType<HeartoftheElements>())
                    )
                )
            );*/
        }
        public int? GetMusicFromMusicMod(string songFilename) => !this.MusicAvailable ? new int?() : new int?(MusicLoader.GetMusicSlot(musicMod, "Sounds/Music/" + songFilename));

        private void AddBoss(
          Mod bossChecklist,
          Mod hostMod,
          string name,
          float difficulty,
          object npcTypes,
          Func<bool> downed,
          Dictionary<string, object> extraInfo)
        {
            if (bossChecklist != null)
            {
                bossChecklist.Call(new object[7]
                {
                (object) "LogBoss",
                (object) hostMod,
                (object) name,
                (object) difficulty,
                (object) downed,
                npcTypes,
                (object) extraInfo
                });
            }
        }
    }
}