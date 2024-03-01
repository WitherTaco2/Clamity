using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Clamity.Content.Biomes.FrozenHell.Biome;
using Clamity.Content.Boss.Pyrogen.Drop;
using Clamity.Content.Cooldowns;
using Clamity.Content.Items.Tools.Bags.Fish;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity
{
    public class ClamityPlayer : ModPlayer
    {
        public bool realityRelocator;
        public bool wulfrumShortstrike;
        public bool aflameAcc;
        public List<int> aflameAccList;

        //Equip
        public bool pyroSpear;
        //public int pyroSpearCD;
        public bool vampireEX;
        public bool pyroStone;
        public bool pyroStoneVanity;
        public bool hellFlare;
        public bool icicleRing;

        //Armor
        public bool inflicingMeleeFrostburn;
        public bool frozenParrying;

        //Minion
        public bool hellsBell;

        //Buffs-Debuffs
        //public bool wCleave;
        public bool taintedInferno;
        public bool taintedMagicPower;
        public bool taintedManaRegen;

        //Pets

        public bool ZoneFrozenHell => Player.InModBiome((ModBiome)ModContent.GetInstance<FrozenHell>());
        public override void ResetEffects()
        {
            realityRelocator = false;
            wulfrumShortstrike = false;
            aflameAcc = false;
            aflameAccList = new List<int>();

            pyroSpear = false;
            vampireEX = false;
            pyroStone = false;
            pyroStoneVanity = false;
            hellFlare = false;
            icicleRing = false;

            inflicingMeleeFrostburn = false;
            frozenParrying = false;

            hellsBell = false;

            //wCleave = false;
            taintedInferno = false;
            taintedMagicPower = false;
            taintedManaRegen = false;
        }
        //public Item[] accesories;
        public override void UpdateEquips()
        {
            foreach (Item i in Player.armor)
            {
                if (!i.IsAir)
                    if (i.Calamity().AppliedEnchantment != null)
                    {
                        if (i.Calamity().AppliedEnchantment.Value.ID == 10000)
                            aflameAccList.Add(i.type);
                    }
            }
            if (aflameAccList.Count > 0)
            {
                Player.AddBuff(ModContent.BuffType<WeakBrimstoneFlames>(), 1);
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (base.Player.dead)
            {
                return;
            }
            if (CalamityKeybinds.NormalityRelocatorHotKey.JustPressed && realityRelocator && Main.myPlayer == Player.whoAmI && !Player.CCed)
            {
                Vector2 vector = default(Vector2);
                vector.X = (float)Main.mouseX + Main.screenPosition.X;
                if (base.Player.gravDir == 1f)
                {
                    vector.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)base.Player.height;
                }
                else
                {
                    vector.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                }

                vector.X -= base.Player.width / 2;
                if (vector.X > 50f && vector.X < (float)(Main.maxTilesX * 16 - 50) && vector.Y > 50f && vector.Y < (float)(Main.maxTilesY * 16 - 50) && !Collision.SolidCollision(vector, base.Player.width, base.Player.height))
                {
                    base.Player.Teleport(vector, 4);
                    NetMessage.SendData(65, -1, -1, null, 0, base.Player.whoAmI, vector.X, vector.Y, 1);
                }
            }

        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (pyroSpear && !Player.HasCooldown(PyrospearCooldown.ID))
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 vec1 = Vector2.UnitY.RotatedByRandom(1f);
                    Projectile.NewProjectile(item.GetSource_OnHit(target), target.Center + vec1 * 500f, -vec1.RotatedByRandom(0.1f) * 20f, ModContent.ProjectileType<SoulOfPyrogenSpear>(), item.damage / 2, 1f, Player.whoAmI, target.whoAmI);
                }
                //pyroSpearCD = 100;
                Player.AddCooldown(PyrospearCooldown.ID, 100);
            }
            if (hellFlare)
                CalamityUtils.Inflict246DebuffsNPC(target, BuffID.OnFire3);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>()) //true melee effect
            {
                PyroSpearEffect(proj, target);
            }
            if (proj.Calamity().stealthStrike) //
            {
                PyroSpearEffect(proj, target);
            }
        }
        private void PyroSpearEffect(Projectile proj, NPC target)
        {
            if (pyroSpear && !Player.HasCooldown(PyrospearCooldown.ID))
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 vec1 = Vector2.UnitY.RotatedByRandom(1f);
                    Projectile.NewProjectile(proj.GetSource_OnHit(target), target.Center + vec1 * 500f, -vec1.RotatedByRandom(0.1f) * 20f, ModContent.ProjectileType<SoulOfPyrogenSpear>(), proj.damage / 2, 1f, Player.whoAmI, target.whoAmI);
                }
                //pyroSpearCD = 100;
                Player.AddCooldown(PyrospearCooldown.ID, 100);
            }
        }
        /*public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (wCleave)
                Player.Calamity().contactDamageReduction *= 0.75f;
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (wCleave)
                Player.Calamity().contactDamageReduction *= 0.75f;
        }*/
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool flag = !attempt.inHoney && !attempt.inLava;
            if (flag)
            {
                if (Player.ZoneDesert && Main.hardMode && attempt.uncommon && Main.rand.NextBool(7))
                    itemDrop = ModContent.ItemType<FishOfFlame>();
                /*if (Player.Calamity().ZoneSulphur && DownedBossSystem.downedPolterghast && attempt.uncommon && Main.rand.NextBool(10))
                    itemDrop = ModContent.ItemType<FrontGar>();
                if (Player.ZoneJungle && DownedBossSystem.downedProvidence && attempt.uncommon && Main.rand.NextBool(10))
                    itemDrop = ModContent.ItemType<RearGar>();
                if (Player.ZoneSkyHeight && NPC.downedMoonlord && attempt.uncommon && Main.rand.NextBool(10))
                    itemDrop = ModContent.ItemType<SideGar>();*/
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (icicleRing && Player.statLife > Player.statLifeMax2 / 3)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                Player.lifeRegen -= 30;
            }
        }
        public override void PostUpdateEquips()
        {
            if (taintedInferno)
            {
                Player.buffImmune[BuffID.OnFire3] = false;
                Player.AddBuff(BuffID.OnFire3, 10);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc == null) continue;
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                        npc.AddBuff(BuffID.OnFire3, 10);
                }
            }
            if (taintedManaRegen)
                Player.manaCost *= 2;
        }
        public override void PostUpdateMiscEffects()
        {
            StatModifier statModifier;
            if (pyroStone || pyroStoneVanity)
            {
                //Main.NewText("ClamityPlayer messenge: " + pyroStone.ToString() + " " + pyroStoneVanity.ToString());
                IEntitySource sourceAccessory = Player.GetSource_Accessory(FindAccessory(ModContent.ItemType<PyroStone>()));
                statModifier = Player.GetBestClassDamage();
                int damage = Player.ApplyArmorAccDamageBonusesTo(statModifier.ApplyTo(70f));
                if (Player.whoAmI == Main.myPlayer && Player.ownedProjectileCounts[ModContent.ProjectileType<PyroShieldAccessory>()] == 0)
                    Projectile.NewProjectile(sourceAccessory, Player.Center, Vector2.Zero, ModContent.ProjectileType<PyroShieldAccessory>(), damage, 0.0f, Player.whoAmI);
            }
            if (hellFlare)
            {
                if (this.Player.statLife > (int)(Player.statLifeMax2 * 0.75))
                {
                    Player.GetCritChance<GenericDamageClass>() += 10;
                }
                if (this.Player.statLife < (int)(Player.statLifeMax2 * 0.25))
                {
                    Player.endurance += 0.1f;
                }
            }
        }
        public Item FindAccessory(int itemID)
        {
            for (int index = 0; index < 10; ++index)
            {
                if (this.Player.armor[index].type == itemID)
                    return this.Player.armor[index];
            }
            return new Item();
        }
    }
}
