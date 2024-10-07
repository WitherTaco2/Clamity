using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Sounds;
using CalamityMod.World;
using Clamity.Content.Bosses.Losbaf.Drop;
using Clamity.Content.Bosses.Losbaf.Particles;
using Clamity.Content.Bosses.Losbaf.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Losbaf.NPCs
{
    public enum LosbafAttack : int
    {
        Spawn = 0,
        Slam = 1,
        ExoScytheWithTeleports = 2,
        RotatingAroundPlayer = 3,
        DownfallExoScythe = 4,
        CircleExoScythe = 5,
    }
    public enum LosbafCloneColorType : int
    {
        Cyan = 0,
        Yellow = 1,
        Magenta = 2,
    }
    [AutoloadBossHead]
    public partial class LosbafSuperboss : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = NPC.height = 304;
            NPC.npcSlots = 50f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(1);
            NPC.netAlways = true;
            //NPC.hide = true;
            NPC.canGhostHeal = false;

            NPC.DeathSound = CommonCalamitySounds.ExoDeathSound; //new SoundStyle("CalamityMod/Sounds/Item/ClamImpact")
            if (Main.getGoodWorld)
                NPC.DeathSound = new SoundStyle("Clamity/Sounds/Custom/GFBPipe");

            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.lavaImmune = true;

            NPC.LifeMaxNERB(1980000, 3300000, 3300000);
            NPC.DR_NERD(0.3f);
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.damage = 400;

            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToWater = true;

            if (!Main.dedServ)
            {
                Music = Clamity.mod.GetMusicFromMusicMod("Losbaf") ?? MusicID.Boss2;
            }
        }
        public LosbafAttack CurrectAttack
        {
            get => (LosbafAttack)NPC.ai[0];
            set => NPC.ai[0] = (int)value;
        }
        public int AttackTimer
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = (int)value;
        }

        //public static float TimerBetweenScycleRingAttack = 120;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 3200f)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];
            bool bossRushActive = BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || bossRushActive;
            bool rev = CalamityWorld.revenge || bossRushActive;
            bool death = CalamityWorld.death || bossRushActive;

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(faceTarget: false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = 0f;
                    }

                    NPC.velocity.Y -= 0.5f;
                    if (NPC.velocity.Y < -30f)
                    {
                        NPC.velocity.Y = -30f;
                    }

                    if (NPC.timeLeft > 60)
                    {
                        NPC.timeLeft = 60;
                    }

                    return;
                }
            }
            else if (NPC.timeLeft < 1800)
            {
                NPC.timeLeft = 1800;
            }

            int exoScytheType = ModContent.ProjectileType<ExoScythe>();
            int exoBeamType = ModContent.ProjectileType<LosbafExoBeam>();

            #region Attack
            AttackTimer++;
            switch (CurrectAttack)
            {
                case LosbafAttack.Spawn:
                    Spawn(player);
                    break;
                case LosbafAttack.Slam:
                    Slam(player);
                    break;
                case LosbafAttack.ExoScytheWithTeleports:
                    ExoScytheWithTeleports(player);
                    break;
                case LosbafAttack.DownfallExoScythe:
                    DownfallExoScythe(player);
                    break;
                case LosbafAttack.RotatingAroundPlayer:
                    RotatingAroundPlayer(player);
                    break;
            }
            #endregion
        }
        private void NextState(LosbafAttack nextAttack)
        {
            CurrectAttack = nextAttack;
            AttackTimer = 0;
            NPC.ai[2] = 0;
            NPC.ai[3] = 0;
        }
        public static readonly SoundStyle TeleportOutSound = new("Clamity/Sounds/Custom/TeleportOut");
        public void TeleportTo(Vector2 teleportPosition)
        {
            NPC.Center = teleportPosition;
            NPC.velocity = Vector2.Zero;
            NPC.netUpdate = true;

            // Reset the oldPos array, so that afterimages don't suddenly "jump" due to the positional change.
            for (int i = 0; i < NPC.oldPos.Length; i++)
                NPC.oldPos[i] = NPC.position;

            SoundEngine.PlaySound(TeleportOutSound, NPC.Center);

            // Create teleport particle effects.
            ExpandingGreyscaleCircleParticle circle = new(NPC.Center, Vector2.Zero, new(219, 194, 229), 10, 0.28f);
            VerticalLightStreakParticle bigLightStreak = new(NPC.Center, Vector2.Zero, new(228, 215, 239), 10, new(2.4f, 3f));
            MagicBurstParticle magicBurst = new(NPC.Center, Vector2.Zero, new(150, 109, 219), 12, 0.1f);
            for (int i = 0; i < 30; i++)
            {
                Vector2 smallLightStreakSpawnPosition = NPC.Center + Main.rand.NextVector2Square(-NPC.width, NPC.width) * new Vector2(0.4f, 0.2f);
                Vector2 smallLightStreakVelocity = Vector2.UnitY * Main.rand.NextFloat(-3f, 3f);
                VerticalLightStreakParticle smallLightStreak = new(smallLightStreakSpawnPosition, smallLightStreakVelocity, Color.White, 10, new(0.1f, 0.3f));
                smallLightStreak.Spawn();
            }

            circle.Spawn();
            bigLightStreak.Spawn();
            magicBurst.Spawn();
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 107, 0f, 0f, 100, new Color(0, 255, 255), 1f);

            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 3;
                SoundEngine.PlaySound(CommonCalamitySounds.ExoHitSound, NPC.Center);
            }

            /*if (NPC.life <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 107, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
                }
                for (int j = 0; j < 20; j++)
                {
                    int plasmaDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 107, 0f, 0f, 0, new Color(0, 255, 255), 2.5f);
                    Main.dust[plasmaDust].noGravity = true;
                    Main.dust[plasmaDust].velocity *= 3f;
                    plasmaDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 107, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
                    Main.dust[plasmaDust].velocity *= 2f;
                    Main.dust[plasmaDust].noGravity = true;
                }

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody5").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody6").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AresBody7").Type, 1f);
                }
            }*/
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LosbafBag>()));
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();


            mainRule.Add(ItemDropRule.Common(ModContent.ItemType<ThankYouPainting>(), 100));
            //Trophy
            //npcLoot.Add(ModContent.ItemType<>(), 10);
            //Relic
            //npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<>());
            //Mask
            //mainRule.Add(ItemDropRule.Common(ModContent.ItemType<>(), 7));
            //Lore
            //npcLoot.AddConditionalPerPlayer(() => !ClamitySystem.downedLosbaf, ModContent.ItemType<>(), ui: true, DropHelper.FirstKillText);

            //npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ItemID.CopperPlating, 1, 1, 9999, hideLootReport: true);
        }
        public override void OnKill()
        {
            ClamitySystem.downedLosbaf = true;
            CalamityNetcode.SyncWorld();
        }
        public static Color GetLosbafCloneColor(int type, float alpha = 128)
        {
            switch (type)
            {
                case 0:
                    return new Color(0, 255, 255, alpha); //Cyan
                case 1:
                    return new Color(255, 255, 0, alpha); //Yellow
                case 2:
                    return new Color(255, 0, 255, alpha); //Magenta
                default:
                    return Color.White;
            }
        }
        public static Color GetLosbafCloneColor(LosbafCloneColorType type, float alpha = 128)
        {
            return GetLosbafCloneColor((int)type, alpha);
        }
    }
}
