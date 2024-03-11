namespace Clamity.Content.Bosses.Clamitas.Drop
{
    public class ClamitousPearl : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Materials";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 22;
            Item.maxStack = 9999;
            Item.value = 1000;
            Item.rare = ItemRarityID.Pink;
        }
    }
}
