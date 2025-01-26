using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Clamity.Commons
{
    public class CalRemixCompatibilitySystem : ModSystem
    {
        private static readonly Queue<FannyDialog> deferredDialogToRegister = new();

        public class FannyDialog
        {
            private readonly object instance;

            public FannyDialog(string dialogKey, string portrait)
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return;

                // Add the mod name in front of the identifier.
                string identifier = $"Clamity_{dialogKey}";

                string dialog = Language.GetTextValue($"Mods.Clamity.FannyDialog.{dialogKey}");
                instance = Clamity.calRemix.Call("CreateFannyDialog", identifier, dialog, portrait);
            }

            public FannyDialog WithoutPersistenceBetweenWorlds()
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("MakeFannyDialogNotPersist", instance);
                return this;
            }

            public FannyDialog WithoutClickability()
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("MakeFannyDialogNonClickable", instance);
                return this;
            }

            public FannyDialog WithCooldown(float cooldownInSeconds)
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("SetFannyDialogCooldown", instance, cooldownInSeconds);
                return this;
            }

            public FannyDialog WithCondition(Func<IEnumerable<NPC>, bool> condition)
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("AddFannyDialogCondition", instance, condition);
                return this;
            }

            public FannyDialog WithDrawSizes(int maxWidth = 380, float fontSizeFactor = 1f)
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("SetFannyDialogDrawSize", instance, maxWidth, fontSizeFactor);
                return this;
            }

            public FannyDialog WithDuration(float durationInSeconds)
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("SetFannyDialogDuration", instance, durationInSeconds);
                return this;
            }

            public FannyDialog WithRepeatability()
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("MakeFannyDialogRepeatable", instance);
                return this;
            }

            public FannyDialog WithEvilness()
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("MakeFannyDialogSpokenByEvilFanny", instance);
                return this;
            }

            public FannyDialog WithHoverItem(int itemID, float drawScale = 1f, Vector2 drawOffset = default)
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("AddFannyItemDisplay", instance, itemID, drawScale, drawOffset);
                return this;
            }

            public FannyDialog WithParentDialog(FannyDialog parent, float appearDelayInSeconds, bool parentNeedsToBeClickedOff = false)
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("ChainFannyDialog", parent.instance, instance, appearDelayInSeconds);
                if (!parentNeedsToBeClickedOff)
                    return WithoutClickability().WithCondition(_ => true);

                return WithCondition(_ => true);
            }

            public FannyDialog WithHoverText(string hoverText)
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return this;

                Clamity.calRemix.Call("SetFannyHoverText", instance, hoverText);
                return this;
            }

            public static bool JustReadLoreItem(int loreItemID)
            {
                (bool readLoreItem, int hoverItemID) = (Tuple<bool, int>)Clamity.calRemix.Call("GetFannyItemHoverInfo");

                return readLoreItem && hoverItemID == loreItemID;
            }

            public void Register()
            {
                if (Clamity.calRemix is null || Main.netMode == NetmodeID.Server)
                    return;

                if (Main.gameMenu)
                {
                    deferredDialogToRegister.Enqueue(this);
                    return;
                }

                Clamity.calRemix.Call("RegisterFannyDialog", instance);
            }
        }

        public static void MakeCountAsLoreItem(int loreItemID)
        {
            Clamity.calRemix?.Call("MakeItemCountAsLoreItem", loreItemID);
        }

        public override void PreUpdateEntities()
        {
            while (deferredDialogToRegister.TryDequeue(out FannyDialog dialog))
                dialog.Register();
        }
    }
}
