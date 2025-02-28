using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Biomes.EntropicSpace.Sky
{
    public class EntropicSpaceScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => player.Clamity().ZoneEntropicSpace;
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Clamity:EntropicSpaceSky", isActive);

        }
    }
}
