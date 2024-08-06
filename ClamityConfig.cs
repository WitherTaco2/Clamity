using Clamity.Content.Bosses.Losbaf.NPCs;
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

#if DEBUG
        [DefaultValue(LosbafAttack.Slam)]
        //[ReloadRequired]
        public LosbafAttack StartingLosbafAttack;
#endif

        [Header("MusicEvents")]
        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool LosbafIntelude { get; set; }
    }
}
