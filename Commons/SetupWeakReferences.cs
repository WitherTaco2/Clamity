using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityMod.Events;
using CalamityMod.Items.Mounts;
using CalamityMod.NPCs.Yharon;
using CalamityMod.UI.CalamitasEnchants;
using Clamity.Content.Biomes.FrozenHell.Biome.Background;
using Clamity.Content.Bosses.Clamitas;
using Clamity.Content.Bosses.Clamitas.Drop;
using Clamity.Content.Bosses.Clamitas.NPCs;
using Clamity.Content.Bosses.Pyrogen;
using Clamity.Content.Bosses.Pyrogen.Drop;
using Clamity.Content.Bosses.Pyrogen.NPCs;
using Clamity.Content.Bosses.WoB;
using Clamity.Content.Bosses.WoB.Drop;
using Clamity.Content.Bosses.WoB.NPCs;
using Clamity.Content.Cooldowns;
using Clamity.Content.Items.Mounts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Clamity.Commons
{
    public static class SetupWeakReferences
    {
        #region Load
        public static void Load()
        {
            LoadEnchantments();
            LoadBossRush();
            if (!Main.dedServ)
            {
                LoadShaders();
            }
        }
        private static bool EnchantableAcc(Item item) => !item.IsAir && item.maxStack == 1 && item.ammo == AmmoID.None && item.accessory;
        /// <summary>
        /// Creates Clamity's Enchantments and Exhume Crafts
        /// </summary>
        public static void LoadEnchantments()
        {
            //Enchantments
            ModLoader.GetMod("CalamityMod").Call(
                "CreateEnchantment",
                ClamityUtils.GetText("UI.Enchantments.AflameAcc.DisplayName"),
                ClamityUtils.GetText("UI.Enchantments.AflameAcc.Description"),
                10000,
                new Predicate<Item>(EnchantableAcc),
                "CalamityMod/UI/CalamitasEnchantments/CurseIcon_Aflame",
                delegate (Player player) { player.Clamity().aflameAcc = true; }

            );

            //Exhume
            EnchantmentManager.ItemUpgradeRelationship.Add(ModContent.ItemType<ExoThrone>(), ModContent.ItemType<FlameCube>());
        }
        /// <summary>
        /// Adds Clamity's Bosses into Boss Rush
        /// </summary>
        public static void LoadBossRush()
        {
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
            BossRushEvent.Bosses.Insert(22,
                new BossRushEvent.Boss(
                    ModContent.NPCType<ClamitasBoss>(),
                    BossRushEvent.TimeChangeContext.None,
                    type =>
                    {
                        Player player = Main.player[BossRushEvent.ClosestPlayerToWorldCenter];

                        NPC.SpawnOnPlayer(BossRushEvent.ClosestPlayerToWorldCenter, type);
                    },
                    -1,
                    false,
                    0.0f
                )
            );

            BossRushEvent.BossDeathEffects.Add(ModContent.NPCType<Yharon>(), (NPC npc) =>
            {
                for (int i = 0; i < 255; i++)
                {
                    Player player = Main.player[i];
                    if (player != null && player.active)
                    {
                        player.Calamity().BossRushReturnPosition = player.Center;
                        Vector2? underworldPosition = CalamityPlayer.GetUnderworldPosition(player);
                        if (!underworldPosition.HasValue)
                        {
                            break;
                        }

                        CalamityPlayer.ModTeleport(player, underworldPosition.Value, playSound: false, 2);
                        SoundStyle style = BossRushEvent.TeleportSound with
                        {
                            Volume = 1.6f
                        };
                        SoundEngine.PlaySound(in style, player.Center);
                    }
                }
            });
            BossRushEvent.BossDeathEffects.Add(ModContent.NPCType<WallOfBronze>(), BossRushEvent.BossDeathEffects[113]);
            BossRushEvent.Bosses.Insert(44,
                new BossRushEvent.Boss(
                    ModContent.NPCType<WallOfBronze>(),
                    BossRushEvent.TimeChangeContext.None,
                    type =>
                    {
                        Player player = Main.player[BossRushEvent.ClosestPlayerToWorldCenter];

                        SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/SCalSounds/SepulcherSpawn"), player.Center);
                        //NPC.SpawnBoss(BossRushEvent.ClosestPlayerToWorldCenter, type);

                        int center = Main.maxTilesX * 16 / 2;
                        NPC.NewNPC(player.GetSource_ItemUse(player.HeldItem), (int)player.Center.X - 1000 * (player.Center.X > center ? -1 : 1), (int)player.Center.Y, type);
                    },
                    -1,
                    true,
                    0.0f,
                    ModContent.NPCType<WallOfBronzeClaw>(),
                    ModContent.NPCType<WallOfBronzeLaser>(),
                    ModContent.NPCType<WallOfBronzeTorret>()
                )
            );
        }
        /// <summary>
        /// Creates screen shaders
        /// </summary>
        public static void LoadShaders()
        {
            Filters.Scene["Clamity:FrozenHellSky"] = new Filter(new FrozenHellShaderData("FilterMiniTower").UseColor(0.5f, 1f, 1f).UseOpacity(0.65f), EffectPriority.VeryHigh);
            SkyManager.Instance["Clamity:FrozenHellSky"] = (CustomSky)new FrozenHellSky();
        }
        /// <summary>
        /// Registers Clamity's Cooldowns
        /// </summary>
        public static void LoadCooldowns()
        {
            CooldownRegistry.Register<ShortstrikeCooldown>(ShortstrikeCooldown.ID);
            CooldownRegistry.Register<PyrospearCooldown>(PyrospearCooldown.ID);
            CooldownRegistry.Register<ShortstrikeCharge>(ShortstrikeCharge.ID);
        }
        #endregion

        #region PostSetupContent
        public static void PostSetupContent()
        {
            SetupBossChecklist();
            //SetupInfernumIntroScreen();
        }
        public static void AddBoss(Mod bossChecklist, Mod hostMod, string name, float difficulty, object npcTypes, Func<bool> downed, Dictionary<string, object> extraInfo)
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

        /// <summary>
        /// Setups boss pages in Boss Checklist mod
        /// </summary>
        public static void SetupBossChecklist()
        {
            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
            {
                //Clamitas
                {
                    List<int> itemList = new List<int>()
                    {
                        ModContent.ItemType<ClamitasRelic>(),
                        ModContent.ItemType<LoreWhat>()
                    };
                    if (Clamity.musicMod != null)
                    {
                        itemList.Add(Clamity.musicMod.Find<ModItem>("ClamitasMusicBox").Type);
                    }
                    Action<SpriteBatch, Rectangle, Color> drawing = (Action<SpriteBatch, Rectangle, Color>)((sb, rect, color) =>
                    {
                        Texture2D texture2D = ModContent.Request<Texture2D>(ModContent.GetInstance<ClamitasBoss>().Texture + "_BossChecklist", (AssetRequestMode)2).Value;
                        Vector2 vector2 = new(rect.Center.X - texture2D.Width / 2, rect.Center.Y - texture2D.Height / 2);
                        sb.Draw(texture2D, vector2, color);
                    });
                    AddBoss(bossChecklist, Clamity.mod, "Clamitas", 11.9f, ModContent.NPCType<ClamitasBoss>(), () => ClamitySystem.downedClamitas, new Dictionary<string, object>()
                    {
                        ["spawnItems"] = (object)ModContent.ItemType<ClamitasSummoningItem>(),
                        ["collectibles"] = (object)itemList,
                        ["customPortrait"] = (object)drawing
                    });
                }

                //Pyrogen
                {
                    List<int> itemList = new List<int>()
                    {
                        ModContent.ItemType<PyrogenRelic>(),
                        ModContent.ItemType<PyrogenTrophy>(),
                        ModContent.ItemType<PyrogenMask>(),
                        ModContent.ItemType<LorePyrogen>(),
                        /*ModContent.ItemType<ClamitasMusicbox>()*/
                    };
                    if (Clamity.musicMod != null)
                    {
                        itemList.Add(Clamity.musicMod.Find<ModItem>("PyrogenMusicBox").Type);
                    }
                    Action<SpriteBatch, Rectangle, Color> drawing = (Action<SpriteBatch, Rectangle, Color>)((sb, rect, color) =>
                    {
                        Texture2D texture2D = ModContent.Request<Texture2D>(ModContent.GetInstance<PyrogenBoss>().Texture + "_BossChecklist", (AssetRequestMode)2).Value;
                        Vector2 vector2 = new(rect.Center.X - texture2D.Width / 2, rect.Center.Y - texture2D.Height / 2);
                        sb.Draw(texture2D, vector2, color);
                    });
                    AddBoss(bossChecklist, Clamity.mod, "Pyrogen", 8.5f, ModContent.NPCType<PyrogenBoss>(), () => ClamitySystem.downedPyrogen, new Dictionary<string, object>()
                    {
                        ["spawnItems"] = (object)ModContent.ItemType<PyroKey>(),
                        ["collectibles"] = (object)itemList,
                        ["customPortrait"] = (object)drawing
                    });
                }

                //Wall of Bronze
                {
                    List<int> itemList = new List<int>()
                    {
                        ModContent.ItemType<WoBTrophy>(),
                        ModContent.ItemType<WoBRelic>(),
                        ModContent.ItemType<WoBMask>(),
                        ModContent.ItemType<WoBLore>(),

                    };
                    if (Clamity.musicMod != null)
                    {
                        itemList.Add(Clamity.musicMod.Find<ModItem>("WoBMusicBox").Type);
                    }
                    Action<SpriteBatch, Rectangle, Color> drawing = (Action<SpriteBatch, Rectangle, Color>)((sb, rect, color) =>
                    {
                        Texture2D texture2D = ModContent.Request<Texture2D>(ModContent.GetInstance<WallOfBronze>().Texture + "_BossChecklist", (AssetRequestMode)2).Value;
                        Vector2 vector2 = new(rect.Center.X - texture2D.Width / 2, rect.Center.Y - texture2D.Height / 2);
                        sb.Draw(texture2D, vector2, color);
                    });
                    AddBoss(bossChecklist, Clamity.mod, "WallOfBronze", 22.5f, ModContent.NPCType<WallOfBronze>(), () => ClamitySystem.downedWallOfBronze, new Dictionary<string, object>()
                    {
                        ["spawnItems"] = (object)ModContent.ItemType<AncientConsole>(),
                        ["collectibles"] = (object)itemList,
                        ["customPortrait"] = (object)drawing
                    });
                }
            }
        }
        /// <summary>
        /// He setups Inferum Mode's boss intros
        /// </summary>
        public static void SetupInfernumIntroScreen()
        {
            if (Clamity.infernum != null)
            {
                //bool activeInfernum = Clamity.infernum.Call("GetInfernumActive") as bool? ?? false;
                //Clamitas
                /*{
                    object intro = Clamity.infernum.Call("InitializeIntroScreen",
                        Language.GetOrRegister("Mods.Clamity.InfernumIntro.Clamitas"),
                        150, false,
                        () => { return NPC.AnyNPCs(ModContent.NPCType<ClamitasBoss>()) && (Clamity.infernum.Call("GetInfernumActive") as bool? ?? false); },
                        (float ratio, float completion) => { return Color.DarkRed; }
                        );
                    intro = Clamity.infernum.Call("RegisterIntroScreen", intro);
                }*/

                //Pyrogen
                {
                    object intro = Clamity.infernum.Call("InitializeIntroScreen",
                        Language.GetText("Mods.Clamity.InfernumIntro.Pyrogen"),
                        150, false,
                        () => { return NPC.AnyNPCs(ModContent.NPCType<PyrogenBoss>()) && (Clamity.infernum.Call("GetInfernumActive") as bool? ?? false); },
                        (float ratio, float completion) => { return Color.Red; }
                        );
                    Action action = () => { };
                    intro = Clamity.infernum.Call("SetupCompletionEffects", intro, action);

                    Func<int, float> letterDisplayCompletionRatio = (int animationTimer) => { return 1f; };
                    intro = Clamity.infernum.Call("SetupLetterDisplayCompletionRatio", intro, letterDisplayCompletionRatio);

                    //intro = Clamity.infernum.Call("SetupMainSound", intro, (int animationTimer, int animationTime, float textDelayInterpolant, float letterDisplayCompletionRatio) => { return false; }, () => { return SoundID.MenuTick; });
                    Func<int, int, float, float, bool> canPlaySound = (int animationTimer, int animationTime, float textDelayInterpolant, float letterDisplayCompletionRatio) => { return animationTimer > (int)(animationTime * (textDelayInterpolant + 0.05f)); };
                    Func<SoundStyle> sound = () => { return SoundID.MenuTick; };
                    intro = Clamity.infernum.Call("IntroScreenSetupMainSound", intro, canPlaySound, sound);

                    intro = Clamity.infernum.Call("RegisterIntroScreen", intro);
                }

                //WoB
                /*{
                    object intro = Clamity.infernum.Call("InitializeIntroScreen",
                        Language.GetOrRegister("Mods.Clamity.InfernumIntro.WoB"),
                        150, true,
                        () => { return NPC.AnyNPCs(ModContent.NPCType<WallOfBronze>()) && (Clamity.infernum.Call("GetInfernumActive") as bool? ?? false); },
                        (float ratio, float completion) => { return Color.Lerp(Color.Brown, Color.YellowGreen, completion); }
                        );
                    intro = Clamity.infernum.Call("RegisterIntroScreen", intro);
                }*/
            }
        }
        #endregion
    }
}
