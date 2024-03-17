using Terraria;
using Terraria.ModLoader;

namespace Clamity.Content.Buffs.Shortstrike
{
    public class GladiusShortstrike : ModBuff, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Buffs.Shortstrike";
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.2f;
        }
    }
}
