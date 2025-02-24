using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion.Biomes
{
    public class DistortionBiome : ModBiome
    {
        private string Texture => (GetType().Namespace + "." + nameof(DistortionBiome)).Replace('.', '/');
        //public override string BackgroundPath => Texture + "_Map";
        public override string BestiaryIcon => Texture + "_Icon";
        //public override string MapBackground => BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => player.Clamity().ZoneDistortion;
        public override int Music => Clamity.mod.GetMusicFromMusicMod("TheDistortion/ShatteredIslands") ?? MusicID.OtherworldlySpace;
    }
}
