using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Cooldowns;
using CalamityMod.Items;
using CalamityMod.UI.CalamitasEnchants;
using Clamity.Content.Boss.Pyrogen.Drop;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace Clamity
{
    public class ClamityPlayer : ModPlayer
    {
        public bool realityRelocator;
        public bool wulfrumShortstrike;
        public bool aflameAcc;
        public List<int> aflameAccList;
        public bool pyroSpear;
        public int pyroSpearCD;
        //Minion
        public bool hellsBell;
        public override void ResetEffects()
        {
            realityRelocator = false;
            wulfrumShortstrike = false;
            aflameAcc = false;
            aflameAccList = new List<int>();
            pyroSpear = false;

            hellsBell = false;
        }
        //public Item[] accesories;
        public override void UpdateEquips()
        {
            if (pyroSpearCD > 0)
                pyroSpearCD--;
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
            if (pyroSpear && pyroSpearCD == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 vec1 = Vector2.UnitY.RotatedByRandom(1f);
                    Projectile.NewProjectile(item.GetSource_OnHit(target), target.Center + vec1 * 500f, -vec1 * 20f, ModContent.ProjectileType<SoulOfPyrogenSpear>(), item.damage / 2, 1f, Player.whoAmI, target.whoAmI);
                }
                pyroSpearCD = 100;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>())
            {
                if (pyroSpear && pyroSpearCD == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 vec1 = Vector2.UnitY.RotatedByRandom(1f);
                        Projectile.NewProjectile(proj.GetSource_OnHit(target), target.Center + vec1 * 500f, -vec1 * 20f, ModContent.ProjectileType<SoulOfPyrogenSpear>(), proj.damage / 2, 1f, Player.whoAmI, target.whoAmI);
                    }
                    pyroSpearCD = 100;
                }
            }
        }
    }
}
