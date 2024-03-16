using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Clamity.Content.Cooldowns
{
    public class PyrospearCooldown : CooldownHandler
    {
        public new static string ID => nameof(PyrospearCooldown);
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Mods.Clamity.UI.Cooldowns." + PyrospearCooldown.ID);
        public override string Texture => "Clamity/Content/Cooldowns/PyrospearCooldown";
        public override Color OutlineColor => new Color(235, 121, 121);
        public override Color CooldownStartColor => new Color(105, 53, 81);
        public override Color CooldownEndColor => new Color(125, 63, 96);
    }
}
