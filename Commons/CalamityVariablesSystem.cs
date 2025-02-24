using CalamityMod;
using System.Reflection;
using Terraria.ModLoader.IO;

namespace Clamity.Commons
{
    // System from Lucille Karma's Wrath of the Gods mod, for the use of Infernum Mode.
    // https://github.com/DominicKarma/WrathOfTheGodsPublic/blob/main/Core/CrossCompatibility/Inbound/BaseCalamity/CommonCalamityVariables.BossDefeatStates.cs
    // We use reflection to access Calamity's downed boss system. This is done to avoid direct references to Calamity's code.
    // Since the ModCalls in calamity are not changed for compatibility reasons, this is a safe way to access the downed boss system even
    // if the CalamityMod.dll is outdated, making it easy to mantain.
    public static class CalamityVariablesSystem
    {
        #region Methods

        // This represents a shorthand, future-proofed wrapper for accessing Calamity's mod calls where possible.
        // This is done over direct member access for the purpose of minimizing damage in the case of breaking update changes.
        // While the member could be renamed, it's a safe bet that mod calls that access said member will not be a problem.
        public static bool TryGetFromModCall<T>(out T result, params string[] modCallInfo)
        {
            // Use a default value for the output result.
            result = default;

            if (Clamity.calamity is not null)
            {
                // Get the result from the mod call. If incorrect mod call information is passed the call will throw an exception.
                // *Technically* implementing some error-handling for that would be the absolute best for future-proofing, but it's possible that would incur considerable
                // performance costs and I don't take Calamity's developers for such fools that they'd change mod calls without some some legacy handling.
                object callResult = Clamity.calamity.Call(modCallInfo);

                // If the call result is the desired resulting type, return it.
                if (callResult is not null and T r)
                {
                    result = r;
                    //Clamity.mod.Logger.Debug(modCallInfo.ToString() + r.GetType().Name + " " + r.ToString());
                    return true;
                }
            }

            // As a failsafe, return false.
            return false;
        }

        // Be careful with numeric types in this! For most programming purposes it's fine to rely on implicit operators for bytes, shorts, ints, etc. to some extent, but when objects are
        // being boxed and unboxed explicitly again you can't rely on that. 
        public static void TrySetFromModCall(object value, params string[] modCallInfo)
        {
            // Don't bother if Calamity is not enabled.
            if (Clamity.calamity is null)
                return;

            // It is standard that call information, such as string identifiers, go first while value information goes last.
            Clamity.calamity.Call(modCallInfo, value);
        }

        internal static void SetDownedValue(string fieldName, bool value)
        {
            //if (value == false)
            //    Clamity.mod.Logger.Warn(fieldName + " changed to false");
            //ModReferences.BaseCalamity?.Code?.GetType("CalamityMod.DownedBossSystem")?.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
            var r = typeof(DownedBossSystem).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (r == null)
            {
                Clamity.mod.Logger.Warn(r.Name + " returned null");
            }
            r.SetValue(null, value);
            //CalamityNetcode.SyncWorld();
        }

        internal static void SetWorldValue(string fieldName, bool value)
        {
            ModReferences.BaseCalamity?.Code?.GetType("CalamityMod.World.CalamityWorld")?.GetField(fieldName, BindingFlags.Public | BindingFlags.Static).SetValue(null, value);
        }

        #endregion

        #region Defeated Bosses Flags

        public static bool DesertScourgeDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "desertscourge") && defeated;
            set => SetDownedValue("_downedDesertScourge", value);
        }

        public static bool CrabulonDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "crabulon") && defeated;
            set => SetDownedValue("_downedCrabulon", value);
        }

        public static bool HiveMindDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "hivemind") && defeated;
            set => SetDownedValue("_downedHiveMind", value);
        }

        public static bool PerforatorHiveDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "perforator") && defeated;
            set => SetDownedValue("_downedPerforator", value);
        }

        public static bool SlimeGodDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "slimegod") && defeated;
            set => SetDownedValue("_downedSlimeGod", value);
        }

        public static bool CryogenDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "cryogen") && defeated;
            set => SetDownedValue("_downedCryogen", value);
        }

        public static bool AquaticScourgeDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "aquaticscourge") && defeated;
            set => SetDownedValue("_downedAquaticScourge", value);
        }

        public static bool BrimstoneElementalDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "brimstoneelemental") && defeated;
            set => SetDownedValue("_downedBrimstoneElemental", value);
        }

        public static bool CalamitasCloneDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "calamitasclone") && defeated;
            set => SetDownedValue("_downedCalamitasClone", value);
        }

        public static bool LeviathanDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "anahitaleviathan") && defeated;
            set => SetDownedValue("_downedLeviathan", value);
        }

        public static bool AstrumAureusDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "astrumaureus") && defeated;
            set => SetDownedValue("_downedAstrumAureus", value);
        }

        // 2019
        public static bool PeanutButterGoliathDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "plaguebringergoliath") && defeated;
            set => SetDownedValue("_downedPlaguebringer", value);
        }

        public static bool RavagerDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "ravager") && defeated;
            set => SetDownedValue("_downedRavager", value);
        }

        public static bool AstrumDeusDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "astrumdeus") && defeated;
            set => SetDownedValue("_downedAstrumDeus", value);
        }

        public static bool ProfanedGuardiansDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "guardians") && defeated;
            set => SetDownedValue("_downedGuardians", value);
        }

        public static bool DragonfollyDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "dragonfolly") && defeated;
            set => SetDownedValue("_downedDragonfolly", value);
        }

        public static bool ProvidenceDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "providence") && defeated;
            set => SetDownedValue("_downedProvidence", value);
        }

        public static bool CeaselessVoidDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "ceaselessvoid") && defeated;
            set => SetDownedValue("_downedCeaselessVoid", value);
        }

        public static bool StormWeaverDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "stormweaver") && defeated;
            set => SetDownedValue("_downedStormWeaver", value);
        }

        public static bool SignusDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "signus") && defeated;
            set => SetDownedValue("_downedSignus", value);
        }

        public static bool PolterghastDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "polterghast") && defeated;
            set => SetDownedValue("_downedPolterghast", value);
        }

        public static bool OldDukeDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "oldduke") && defeated;
            set => SetDownedValue("_downedBoomerDuke", value);
        }

        public static bool DevourerOfGodsDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "devourerofgods") && defeated;
            set => SetDownedValue("_downedDoG", value);
        }

        public static bool YharonDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "yharon") && defeated;
            set => SetDownedValue("_downedYharon", value);
        }

        public static bool DraedonDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "exomechs") && defeated;
            set => SetDownedValue("_downedExoMechs", value);
        }

        public static bool CalamitasDefeated
        {
            get => TryGetFromModCall(out bool defeated, "GetBossDowned", "calamitas") && defeated;
            set => SetDownedValue("_downedCalamitas", value);
        }

        #endregion

        #region Other Flags
        public static bool RevengeanceModeActive
        {
            get => TryGetFromModCall(out bool active, "GetDifficulty", "Revengeance") && active;
            set => SetWorldValue("revenge", value);
        }

        public static bool DeathModeActive
        {
            get => TryGetFromModCall(out bool active, "GetDifficulty", "Death") && active;
            set => SetWorldValue("death", value);
        }

        public static bool AcidRainIsOngoing => TryGetFromModCall(out bool active, "GetAcidRainActive") && active;


        #endregion

        #region World Flags

        public static void SaveDefeatStates(TagCompound tag)
        {
            /*if (DownedBossSystem.downedDesertScourge)
                tag[nameof(DesertScourgeDefeated)] = true;
            if (DownedBossSystem.downedCrabulon)
                tag[nameof(CrabulonDefeated)] = true;
            if (DownedBossSystem.downedHiveMind)
                tag[nameof(HiveMindDefeated)] = true;
            if (DownedBossSystem.downedPerforator)
                tag[nameof(PerforatorHiveDefeated)] = true;
            if (DownedBossSystem.downedSlimeGod)
                tag[nameof(SlimeGodDefeated)] = true;
            if (DownedBossSystem.downedCryogen)
                tag[nameof(CryogenDefeated)] = true;
            if (DownedBossSystem.downedAquaticScourge)
                tag[nameof(AquaticScourgeDefeated)] = true;
            if (DownedBossSystem.downedBrimstoneElemental)
                tag[nameof(BrimstoneElementalDefeated)] = true;
            if (DownedBossSystem.downedCalamitasClone)
                tag[nameof(CalamitasCloneDefeated)] = true;
            if (DownedBossSystem.downedLeviathan)
                tag[nameof(LeviathanDefeated)] = true;
            if (DownedBossSystem.downedAstrumAureus)
                tag[nameof(AstrumAureusDefeated)] = true;
            if (DownedBossSystem.downedPlaguebringer)
                tag[nameof(PeanutButterGoliathDefeated)] = true;
            if (DownedBossSystem.downedRavager)
                tag[nameof(RavagerDefeated)] = true;
            if (DownedBossSystem.downedAstrumDeus)
                tag[nameof(AstrumDeusDefeated)] = true;
            if (DownedBossSystem.downedGuardians)
                tag[nameof(ProfanedGuardiansDefeated)] = true;
            if (DownedBossSystem.downedDragonfolly)
                tag[nameof(DragonfollyDefeated)] = true;
            if (DownedBossSystem.downedProvidence)
                tag[nameof(ProvidenceDefeated)] = true;
            if (DownedBossSystem.downedCeaselessVoid)
                tag[nameof(CeaselessVoidDefeated)] = true;
            if (DownedBossSystem.downedStormWeaver)
                tag[nameof(StormWeaverDefeated)] = true;
            if (DownedBossSystem.downedSignus)
                tag[nameof(SignusDefeated)] = true;
            if (DownedBossSystem.downedPolterghast)
                tag[nameof(PolterghastDefeated)] = true;
            if (DownedBossSystem.downedBoomerDuke)
                tag[nameof(OldDukeDefeated)] = true;
            if (DownedBossSystem.downedDoG)
                tag[nameof(DevourerOfGodsDefeated)] = true;
            if (DownedBossSystem.downedYharon)
                tag[nameof(YharonDefeated)] = true;
            if (DownedBossSystem.downedExoMechs)
                tag[nameof(DraedonDefeated)] = true;
            if (DownedBossSystem.downedCalamitas)
                tag[nameof(CalamitasDefeated)] = true;*/

            if (DesertScourgeDefeated)
                tag[nameof(DesertScourgeDefeated)] = true;
            if (CrabulonDefeated)
                tag[nameof(CrabulonDefeated)] = true;
            if (HiveMindDefeated)
                tag[nameof(HiveMindDefeated)] = true;
            if (PerforatorHiveDefeated)
                tag[nameof(PerforatorHiveDefeated)] = true;
            if (SlimeGodDefeated)
                tag[nameof(SlimeGodDefeated)] = true;
            if (CryogenDefeated)
                tag[nameof(CryogenDefeated)] = true;
            if (AquaticScourgeDefeated)
                tag[nameof(AquaticScourgeDefeated)] = true;
            if (BrimstoneElementalDefeated)
                tag[nameof(BrimstoneElementalDefeated)] = true;
            if (CalamitasCloneDefeated)
                tag[nameof(CalamitasCloneDefeated)] = true;
            if (LeviathanDefeated)
                tag[nameof(LeviathanDefeated)] = true;
            if (AstrumAureusDefeated)
                tag[nameof(AstrumAureusDefeated)] = true;
            if (PeanutButterGoliathDefeated)
                tag[nameof(PeanutButterGoliathDefeated)] = true;
            if (RavagerDefeated)
                tag[nameof(RavagerDefeated)] = true;
            if (AstrumDeusDefeated)
                tag[nameof(AstrumDeusDefeated)] = true;
            if (ProfanedGuardiansDefeated)
                tag[nameof(ProfanedGuardiansDefeated)] = true;
            if (DragonfollyDefeated)
                tag[nameof(DragonfollyDefeated)] = true;
            if (ProvidenceDefeated)
                tag[nameof(ProvidenceDefeated)] = true;
            if (CeaselessVoidDefeated)
                tag[nameof(CeaselessVoidDefeated)] = true;
            if (StormWeaverDefeated)
                tag[nameof(StormWeaverDefeated)] = true;
            if (SignusDefeated)
                tag[nameof(SignusDefeated)] = true;
            if (PolterghastDefeated)
                tag[nameof(PolterghastDefeated)] = true;
            if (OldDukeDefeated)
                tag[nameof(OldDukeDefeated)] = true;
            if (DevourerOfGodsDefeated)
                tag[nameof(DevourerOfGodsDefeated)] = true;
            if (YharonDefeated)
                tag[nameof(YharonDefeated)] = true;
            if (DraedonDefeated)
                tag[nameof(DraedonDefeated)] = true;
            if (CalamitasDefeated)
                tag[nameof(CalamitasDefeated)] = true;
        }

        public static void LoadDefeatStates(TagCompound tag)
        {
            /*DownedBossSystem.downedDesertScourge = tag.ContainsKey(nameof(DesertScourgeDefeated));
            DownedBossSystem.downedCrabulon = tag.ContainsKey(nameof(CrabulonDefeated));
            DownedBossSystem.downedHiveMind = tag.ContainsKey(nameof(HiveMindDefeated));
            DownedBossSystem.downedPerforator = tag.ContainsKey(nameof(PerforatorHiveDefeated));
            DownedBossSystem.downedSlimeGod = tag.ContainsKey(nameof(SlimeGodDefeated));
            DownedBossSystem.downedCryogen = tag.ContainsKey(nameof(CryogenDefeated));
            DownedBossSystem.downedAquaticScourge = tag.ContainsKey(nameof(AquaticScourgeDefeated));
            DownedBossSystem.downedBrimstoneElemental = tag.ContainsKey(nameof(BrimstoneElementalDefeated));
            DownedBossSystem.downedCalamitasClone = tag.ContainsKey(nameof(CalamitasCloneDefeated));
            DownedBossSystem.downedLeviathan = tag.ContainsKey(nameof(LeviathanDefeated));
            DownedBossSystem.downedAstrumAureus = tag.ContainsKey(nameof(AstrumAureusDefeated));
            DownedBossSystem.downedPlaguebringer = tag.ContainsKey(nameof(PeanutButterGoliathDefeated));
            DownedBossSystem.downedRavager = tag.ContainsKey(nameof(RavagerDefeated));
            DownedBossSystem.downedAstrumDeus = tag.ContainsKey(nameof(AstrumDeusDefeated));
            DownedBossSystem.downedGuardians = tag.ContainsKey(nameof(ProfanedGuardiansDefeated));
            DownedBossSystem.downedDragonfolly = tag.ContainsKey(nameof(DragonfollyDefeated));
            DownedBossSystem.downedProvidence = tag.ContainsKey(nameof(ProvidenceDefeated));
            DownedBossSystem.downedCeaselessVoid = tag.ContainsKey(nameof(CeaselessVoidDefeated));
            DownedBossSystem.downedStormWeaver = tag.ContainsKey(nameof(StormWeaverDefeated));
            DownedBossSystem.downedSignus = tag.ContainsKey(nameof(SignusDefeated));
            DownedBossSystem.downedPolterghast = tag.ContainsKey(nameof(PolterghastDefeated));
            DownedBossSystem.downedBoomerDuke = tag.ContainsKey(nameof(OldDukeDefeated));
            DownedBossSystem.downedDoG = tag.ContainsKey(nameof(DevourerOfGodsDefeated));
            DownedBossSystem.downedYharon = tag.ContainsKey(nameof(YharonDefeated));
            DownedBossSystem.downedExoMechs = tag.ContainsKey(nameof(DraedonDefeated));
            DownedBossSystem.downedCalamitas = tag.ContainsKey(nameof(CalamitasDefeated));*/

            DesertScourgeDefeated = tag.ContainsKey(nameof(DesertScourgeDefeated));
            CrabulonDefeated = tag.ContainsKey(nameof(CrabulonDefeated));
            HiveMindDefeated = tag.ContainsKey(nameof(HiveMindDefeated));
            SlimeGodDefeated = tag.ContainsKey(nameof(SlimeGodDefeated));
            CryogenDefeated = tag.ContainsKey(nameof(CryogenDefeated));
            AquaticScourgeDefeated = tag.ContainsKey(nameof(AquaticScourgeDefeated));
            BrimstoneElementalDefeated = tag.ContainsKey(nameof(BrimstoneElementalDefeated));
            CalamitasCloneDefeated = tag.ContainsKey(nameof(CalamitasCloneDefeated));
            LeviathanDefeated = tag.ContainsKey(nameof(LeviathanDefeated));
            AstrumAureusDefeated = tag.ContainsKey(nameof(AstrumAureusDefeated));
            PeanutButterGoliathDefeated = tag.ContainsKey(nameof(PeanutButterGoliathDefeated));
            RavagerDefeated = tag.ContainsKey(nameof(RavagerDefeated));
            AstrumDeusDefeated = tag.ContainsKey(nameof(AstrumDeusDefeated));
            ProfanedGuardiansDefeated = tag.ContainsKey(nameof(ProfanedGuardiansDefeated));
            DragonfollyDefeated = tag.ContainsKey(nameof(DragonfollyDefeated));
            ProvidenceDefeated = tag.ContainsKey(nameof(ProvidenceDefeated));
            CeaselessVoidDefeated = tag.ContainsKey(nameof(CeaselessVoidDefeated));
            StormWeaverDefeated = tag.ContainsKey(nameof(StormWeaverDefeated));
            SignusDefeated = tag.ContainsKey(nameof(SignusDefeated));
            PolterghastDefeated = tag.ContainsKey(nameof(PolterghastDefeated));
            OldDukeDefeated = tag.ContainsKey(nameof(OldDukeDefeated));
            DevourerOfGodsDefeated = tag.ContainsKey(nameof(DevourerOfGodsDefeated));
            YharonDefeated = tag.ContainsKey(nameof(YharonDefeated));
            DraedonDefeated = tag.ContainsKey(nameof(DraedonDefeated));
            CalamitasDefeated = tag.ContainsKey(nameof(CalamitasDefeated));
        }

        #endregion
    }
}
