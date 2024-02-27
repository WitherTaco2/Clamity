using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Boss.ItemBossRush
{
    public class BulletTheoryEffectThing : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Typeless";
        public Player Owner => Main.player[Projectile.owner];
        public ref float Time => ref Projectile.ai[0];
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = TwentyTwoBulletTheory.StartEffectTotalTime;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.Center = Owner.Center;
            if (Time >= 70f)
                MoonlordDeathDrama.RequestLight(Utils.GetLerpValue(70f, 85f, Time, true), Main.LocalPlayer.Center);

            if (Time % 10f == 9f)
                TwentyTwoBulletTheory.SyncStartTimer((int)Time);

            float currentShakePower = MathHelper.Lerp(8f, 12f, Utils.GetLerpValue(TwentyTwoBulletTheory.StartEffectTotalTime * 0.6f, TwentyTwoBulletTheory.StartEffectTotalTime, Time, true));
            currentShakePower *= 1f - Utils.GetLerpValue(1500f, 3700f, Main.LocalPlayer.Distance(Projectile.Center), true);
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = currentShakePower;

            Time++;
        }

        public override void OnKill(int timeLeft)
        {
            TwentyTwoBulletTheory.SyncStartTimer(TwentyTwoBulletTheory.StartEffectTotalTime);
            for (int doom = 0; doom < Main.maxNPCs; doom++)
            {
                NPC n = Main.npc[doom];
                if (!n.active)
                    continue;

                // will also correctly despawn EoW because none of his segments are boss flagged
                bool shouldDespawn = n.boss;
                if (shouldDespawn)
                {
                    n.active = false;
                    n.netUpdate = true;
                }
            }

            TwentyTwoBulletTheory.BulletTheoryStage = 0;
            TwentyTwoBulletTheory.BulletTheoryActive = true;

            // Play startup dialogue
            //BossRushDialogueSystem.StartDialogue(DownedBossSystem.startedBossRushAtLeastOnce ? BossRushDialoguePhase.StartRepeat : BossRushDialoguePhase.Start);

            CalamityNetcode.SyncWorld();
            if (Main.netMode == NetmodeID.Server)
            {
                var netMessage = Mod.GetPacket();
                netMessage.Write((byte)ClamityMessageType.BulletTheoryStage);
                netMessage.Write(TwentyTwoBulletTheory.BulletTheoryStage);
                netMessage.Send();
            }
        }
    }
}
