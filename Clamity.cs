using System.Collections.Generic;
using System;
using Terraria.ModLoader;
using CalamityMod.NPCs.CalClone;
using Clamity.Content.Boss.Clamitas;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Clamity.Content.Boss.Clamitas.Drop;
using Clamity.Content.Boss.Clamitas.Other;
using CalamityMod.Cooldowns;
using Clamity.Content.Cooldowns;

namespace Clamity
{
	public class Clamity : Mod
	{
        public static Clamity mod;
        public override void Load()
        {
            mod = this;
            CooldownRegistry.Register<ShortstrikeCooldown>(ShortstrikeCooldown.ID);
        }
        public override void Unload()
        {
            mod = null;
        }
        public override void PostSetupContent()
        {
            ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist);
            List<int> intList14 = new List<int>()
            {
                ModContent.ItemType<ClamitasRelic>(),
                ModContent.ItemType<LoreWhat>(),
                ModContent.ItemType<ClamitasMusicbox>()
            };
            AddBoss(bossChecklist, mod, "Clamitas", 11.9f, ModContent.NPCType<ClamitasBoss>(), () => ClamitySystem.downedClamitas, new Dictionary<string, object>()
            {
                ["spawnItems"] = (object)ModContent.ItemType<ClamitasSummoningItem>(),
                ["collectibles"] = (object)intList14
            });


            /*
              string str11 = "CalamitasClone";
              float difficulty11;
              WeakReferenceSupport.ModProgression.TryGetValue(str11, out difficulty11);
              int num6 = ModContent.NPCType<CalamitasClone>();
              List<int> intList14 = new List<int>()
              {
                ModContent.ItemType<CalamitasCloneRelic>(),
                ModContent.ItemType<CalamitasCloneTrophy>(),
                ModContent.ItemType<CataclysmTrophy>(),
                ModContent.ItemType<CatastropheTrophy>(),
                ModContent.ItemType<CalamitasCloneMask>(),
                ModContent.ItemType<HoodOfCalamity>(),
                ModContent.ItemType<RobesOfCalamity>(),
                ModContent.ItemType<LoreCalamitasClone>(),
                ModContent.ItemType<ThankYouPainting>()
              };
              WeakReferenceSupport.AddBoss(bossChecklist, calamity, str11, difficulty11, (object) num6, Downed.DownedCalClone, new Dictionary<string, object>()
              {
                ["spawnItems"] = (object) ModContent.ItemType<EyeofDesolation>(),
                ["collectibles"] = (object) intList14
              });
            */
        }
        private void AddBoss(
          Mod bossChecklist,
          Mod hostMod,
          string name,
          float difficulty,
          object npcTypes,
          Func<bool> downed,
          Dictionary<string, object> extraInfo)
        {
            if (bossChecklist != null)
            {
                bossChecklist.Call(new object[7]
                {
                (object) "LogBoss",
                (object) hostMod,
                (object) name,
                (object) difficulty,
                (object) downed,
                npcTypes,
                (object) extraInfo
                });
            }
        }
    }
}