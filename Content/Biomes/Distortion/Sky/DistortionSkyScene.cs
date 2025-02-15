using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.Distortion.Sky
{
    public class DistortionSkyScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => player.Clamity().ZoneDistortion;
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Clamity:Distortion", isActive);

        }
    }
}
