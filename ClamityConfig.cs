using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Clamity
{
    public class ClamityConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static ClamityConfig Instance;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool UsesEnchantedMetalInShadowspecBarRecipe;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool ChangesCalamityRecipes;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool UsesClamityWeaponsInCalamityRecipes;
    }
}
