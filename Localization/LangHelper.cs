namespace Clamity
{
    public static class LangHelper
    {
        /// <summary>
        /// prefixes the modname for the key
        /// </summary>
        /// <returns>Text associated with this key</returns>
        public static string GetTextByMod(Mod mod, string key, params object[] args)
        {
            return Language.GetTextValue($"Mods.{mod.Name}.{key}", args);
        }

        /// <summary>
        /// Defaults to Mods.ClickerClass. as the prefix for the key
        /// </summary>
        /// <returns>Text associated with this key</returns>
        internal static string GetText(string key, params object[] args)
        {
            return GetTextByMod(Clamity.mod, key, args);
        }
        /*internal static string GetTooltipUnsteadBug(ModItem item, params object[] args)
		{
			return GetTextByMod(EmperiumMod.mod, "TooltipUnsteadLangBug." + item.Name, args);
        }*/
        internal static string GetTooltipUnsteadBug(ModItem item, params object[] args)
        {
            return GetTextByMod(Clamity.mod, $"Items.{item.Name}.TooltipNew", args);
        }
    }
}
