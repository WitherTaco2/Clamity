namespace Clamity.Content.Buffs.Shortstrike
{
    public class TungstenShortstrike : ModBuff, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Buffs.Shortstrike";
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.04f;
        }
    }
}
