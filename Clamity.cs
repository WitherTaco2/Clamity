using Clamity.Commons;
using Terraria.ModLoader;

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

            NewNPCStats.Load();
            SetupWeakReferences.Load();
        }
        public override void Unload()
        {
            mod = null;
            musicMod = null;

            NewNPCStats.UnLoad();
        }
        public override void PostSetupContent()
        {
            SetupWeakReferences.PostSetupContent();
        }
        public int? GetMusicFromMusicMod(string songFilename) => !this.MusicAvailable ? new int?() : new int?(MusicLoader.GetMusicSlot(musicMod, "Sounds/Music/" + songFilename));
        public override object Call(params object[] args) => ClamityModCalls.Call(args);
    }
}
