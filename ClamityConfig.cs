using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Clamity
{
    public class ClamityConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ClamityConfig Instance;

        [DefaultValue(false)]
        public bool PermafrostSoldEnchantedMetal;

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
    /*public class ClamityClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ClamityClientConfig Instance;


    }*/
}
