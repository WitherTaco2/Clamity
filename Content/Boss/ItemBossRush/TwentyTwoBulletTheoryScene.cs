﻿using CalamityMod.Skies;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.ItemBossRush
{
    public class TwentyTwoBulletTheoryScene : ModSceneEffect
    {
        internal static readonly FieldInfo EffectsField = typeof(SkyManager).GetField("_effects", BindingFlags.NonPublic | BindingFlags.Instance);

        public override int Music => TwentyTwoBulletTheory.MusicToPlay;
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player) => BossRushSky.DetermineDrawEligibility();

        public override void SpecialVisuals(Player player, bool isActive)
        {
            // Clear all other skies, including the vanilla ones.
            if (isActive)
            {
                Dictionary<string, CustomSky> skies = EffectsField.GetValue(SkyManager.Instance) as Dictionary<string, CustomSky>;
                bool updateRequired = false;
                foreach (string skyName in skies.Keys)
                {
                    if (skies[skyName].IsActive() && skyName != "CalamityMod:BossRush")
                    {
                        skies[skyName].Opacity = 0f;
                        skies[skyName].Deactivate();
                        updateRequired = true;
                    }
                }

                if (updateRequired)
                    SkyManager.Instance.Update(new GameTime());
            }

            player.ManageSpecialBiomeVisuals("CalamityMod:BossRush", isActive);
        }

        public override float GetWeight(Player player) => 1f;
    }
}
