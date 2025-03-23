using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.EntropicSpace.Biomes
{
    public class ShatteredIslandsBiome : ModBiome
    {
        private string Texture => (GetType().Namespace + "." + nameof(ShatteredIslandsBiome)).Replace('.', '/');
        public override string BackgroundPath => Texture + "_Map";
        public override string MapBackground => BackgroundPath;
        public override string BestiaryIcon => Texture + "_Icon";
        public override Color? BackgroundColor => base.BackgroundColor;
        public override bool IsBiomeActive(Player player) => player.Clamity().ZoneShatteredIslands;
        public override int Music => Clamity.mod.GetMusicFromMusicMod("EntropicSpace/ShatteredIslands") ?? MusicID.OtherworldlySpace;
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;
    }
}
