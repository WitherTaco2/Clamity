using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.EntropicSpace.Biomes
{
    public class EntropicSpaceBiome : ModBiome
    {
        private string Texture => (GetType().Namespace + "." + nameof(EntropicSpaceBiome)).Replace('.', '/');
        //public override string BackgroundPath => Texture + "_Map";
        public override string BestiaryIcon => Texture + "_Icon";
        //public override string MapBackground => BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => player.Clamity().ZoneEntropicSpace;
        public override int Music => Clamity.mod.GetMusicFromMusicMod("EntropicSpace/ShatteredIslands") ?? MusicID.OtherworldlySpace;
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
    }
}
