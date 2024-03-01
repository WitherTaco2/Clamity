using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.WoB.FrozenHell.Items
{
    public class FrozenHellstone : ModItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Placeables";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMaterials[this.Type] = 99;
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.White;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;

            Item.consumable = true;
            Item.createTile = ModContent.TileType<FrozenHellstoneTile>();
        }
    }
    public class FrozenHellstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileOreFinderPriority[Type] = (short)950;
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 975;
            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithHell(Type);
            //CalamityUtils.SetMerge(Type, ModContent.TileType<FrozenAshTile>());
            TileID.Sets.Ore[Type] = true;
            TileID.Sets.OreMergesWithMud[Type] = true;
            this.AddMapEntry(new Color(208, (int)byte.MaxValue, (int)byte.MaxValue), this.CreateMapEntryName());
            this.MineResist = 3f;
            this.MinPick = 280;
            this.HitSound = new SoundStyle?(SoundID.Tink);
            Main.tileSpelunker[Type] = true;
        }

        public override bool CanExplode(int i, int j) => false;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}
