using CalamityMod.Rarities;
using CalamityMod;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Clamity.Content.Boss.WoB.FrozenHell.Items
{
    public class FrozenAsh : ModItem, ILocalizedModType, IModType
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
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;

            Item.consumable = true;
            Item.createTile = ModContent.TileType<FrozenAshTile>();
        }
    }
    public class FrozenAshTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            CalamityUtils.MergeWithHell((int)this.Type);
            CalamityUtils.MergeWithGeneral(Type);
            this.AddMapEntry(new Color(78, 89, 99), this.CreateMapEntryName());
            this.HitSound = new SoundStyle?(SoundID.Dig);
        }
        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}
