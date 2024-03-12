using CalamityMod.Cooldowns;

namespace Clamity.Content.Cooldowns
{
    public class ShortstrikeCooldown : CooldownHandler
    {
        public new static string ID => nameof(ShortstrikeCooldown);
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Mods.Clamity.UI.Cooldowns." + ShortstrikeCooldown.ID);
        public override string Texture => "Clamity/Content/Cooldowns/ShortstrikeCooldown";
        public override Color OutlineColor => new Color(255, 226, 217);
        public override Color CooldownStartColor => new Color(205, 134, 71);
        public override Color CooldownEndColor => new Color(235, 166, 135);
    }
}
