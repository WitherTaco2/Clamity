using CalamityMod.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Clamity.Content.Boss.ItemBossRush
{
    public class TwentyTwoBulletTheory
    {
        internal static IEntitySource Source => new EntitySource_WorldEvent("Clamity_ItemBossRush");

        public static int HostileProjectileKillCounter;
        public static bool BossRushActive = false; // Whether Boss Rush is active or not.
        public static bool DeactivateStupidFuckingBullshit = false; // Force Boss Rush to inactive.
        public static int BossRushStage = 0; // Boss Rush Stage.
        public static int BossRushSpawnCountdown = 180; // Delay before another Boss Rush boss can spawn.
        public static List<BossRushEvent.Boss> Bosses = new List<BossRushEvent.Boss>();
        public static Dictionary<int, int[]> BossIDsAfterDeath = new Dictionary<int, int[]>();
        public static Dictionary<int, Action<NPC>> BossDeathEffects = new Dictionary<int, Action<NPC>>();
        public static int StartTimer;
        public static int EndTimer;
        public static float WhiteDimness;
        public static readonly Color XerocTextColor = new(250, 213, 77); // #FAD54D
        public const int StartEffectTotalTime = 120;
        public const int EndVisualEffectTime = 340;
        public static int ClosestPlayerToWorldCenter => Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
        public static int CurrentlyFoughtBoss => Bosses[BossRushStage].EntityID;
        public static int NextBossToFight => Bosses[BossRushStage + 1].EntityID;

        public static readonly SoundStyle BossSummonSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushSummon", 2);

        public static readonly SoundStyle TeleportSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushTeleport");

        public static readonly SoundStyle TerminusActivationSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushTerminusActivate");

        public static readonly SoundStyle StartBuildupSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushTerminusCharge");

        public static readonly SoundStyle TerminusDeactivationSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushTerminusDeactivate");

        public static readonly SoundStyle Tier2TransitionSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushTier2Transition");

        public static readonly SoundStyle Tier3TransitionSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushTier3Transition");

        public static readonly SoundStyle Tier4TransitionSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushTier4Transition");

        public static readonly SoundStyle Tier5TransitionSound = new("CalamityMod/Sounds/Custom/BossRush/BossRushTier5Transition");

        public static readonly SoundStyle VictorySound = new("CalamityMod/Sounds/Custom/BossRush/BossRushVictory");
    }
}
