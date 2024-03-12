namespace Clamity.Content.Buffs.Shortstrike
{
    public class GoldShortstrike : ModBuff, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Buffs.Shortstrike";
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;
        }
    }
}
