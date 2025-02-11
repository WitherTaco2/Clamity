using CalamityMod;
using Clamity.Content.Bosses.Yharim.Subworlds;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.Yharim.NPCs
{
    [AutoloadBossHead]
    public class Yharim : ModNPC
    {
        private static NPC myself;
        public static NPC Myself
        {
            get
            {
                if (myself is not null && !myself.active)
                    return null;

                return myself;
            }
            private set => myself = value;
        }
        public override void SetStaticDefaults()
        {

        }
        public Player Target => Main.player[NPC.target];
        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.damage = 400;
            NPC.width = 44;
            NPC.height = 62;
            NPC.defense = 100;
            NPC.LifeMaxNERB(1024000, 2048000, 2048000);
            NPC.Calamity().DR = 0.25f;

            // Fuck arbitrary Expert boosts.
            NPC.lifeMax /= 2;

            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.canGhostHeal = false;
            NPC.boss = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = null;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = Item.buyPrice(6, 25, 0, 0) / 5;
            NPC.netAlways = true;

            NPC.Calamity().ShouldCloseHPBar = true;
        }
        public enum Attack : int
        {
            Idle = 0,
            AirFallAttack,
            CircleProjectileSlash,
            MinosTeleports,
            Illusions,
            ChaseRotation,
            BigSlashWave,
        }
        public Attack CurrentAttack
        {
            set => NPC.ai[0] = (int)value;
            get => (Attack)NPC.ai[0];
        }
        public int AttackTimer
        {
            set => NPC.ai[1] = value;
            get => (int)NPC.ai[1];
        }
        public float SwordRotation
        {
            set => NPC.ai[2] = value;
            get => NPC.ai[2];
        }
        public float SwordRotationVelocity = 0;
        public override void AI()
        {
            // Pick a target if no valid one exists.
            NPC.TargetClosestIfTargetIsInvalid();


            // Only mark this if no other players are alive.
            if (!Main.player.Any(player => !player.dead && player.active))
            {
                DragonAeria.HasYharimAppeared = false;
                CalamityUtils.DisplayLocalizedText("Mods.Clamity.Dialogues.Yharim.PlayerKilled.Text" + Main.rand.Next(1, 4).ToString(), Color.Gold);
                NPC.active = false;
            }

            // Reset things every frame.
            NPC.damage = NPC.defDamage;
            NPC.dontTakeDamage = false;
            NPC.noTileCollide = false;
            NPC.noGravity = false;
            NPC.Calamity().ShouldCloseHPBar = CurrentAttack == Attack.Idle;

            // Do not despawn, you bastard.
            NPC.timeLeft = 7200;

            Myself = NPC;

            SwordRotationVelocity = 0.1f;
            SwordRotation += SwordRotationVelocity;

            AttackTimer++;
            switch (CurrentAttack)
            {
                case Attack.Idle:
                    Do_Idle();
                    break;
                case Attack.AirFallAttack:
                    Do_AirFallAttack();
                    break;
                case Attack.CircleProjectileSlash:
                    Do_CircleProjectileSlash();
                    break;
                case Attack.MinosTeleports:
                    Do_MinosTeleports();
                    break;
                case Attack.Illusions:
                    Do_Illusions();
                    break;
                case Attack.ChaseRotation:
                    Do_ChaseRotation();
                    break;


            }
        }
        public void SelectNextAttack(Yharim.Attack attack)
        {
            CurrentAttack = attack;
            AttackTimer = 0;
            oldSwordRot = SwordRotation;
            for (int i = 0; i < NPC.Calamity().newAI.Length - 1; i++)
            {
                NPC.Calamity().newAI[0] = 0;
            }
        }
        public void Do_Idle()
        {
            int animationFocusTime = 54;
            int animationFocusReturnTime = 14;
            int dialogueDelay = 80;
            int dialogueCount = 8;
            int dialogueTime = dialogueDelay * dialogueCount;
            if (DragonAeria.HasYharimBeenTalkedBefore)
                dialogueTime = 0;
            ref float hasBegunAnimation = ref NPC.Calamity().newAI[0];

            int animationTime = dialogueTime + animationFocusReturnTime;

            // Disable damage entirely.
            NPC.dontTakeDamage = true;
            NPC.damage = 0;

            // Begin the animation once the player is sufficiently close to the vassal.
            if (NPC.WithinRange(Target.Center, 700f) && hasBegunAnimation == 0f)
            {
                hasBegunAnimation = 1f;
                BlockerSystem.Start(true, false, () =>
                {
                    int index = NPC.FindFirstNPC(Type);
                    if (Main.npc.IndexInRange(index))
                    {
                        NPC yharim = Main.npc[index];
                        if ((Attack)yharim.ai[0] == Attack.Idle)
                            return true;
                    }
                    return false;
                });
                NPC.netUpdate = true;


            }
            if (hasBegunAnimation == 1f && !DragonAeria.HasYharimBeenTalkedBefore)
            {
                NPC.Calamity().newAI[1]++;
                if (NPC.Calamity().newAI[1] % dialogueDelay == dialogueDelay - 1 && AttackTimer / dialogueDelay <= dialogueCount)
                {
                    CalamityUtils.DisplayLocalizedText("Mods.Clamity.Dialogues.Yharim.Intro.Text" + ((int)NPC.Calamity().newAI[1] / 100).ToString(), Color.Gold);
                }
            }

            // Get rid of any and all adrenaline.
            Target.Calamity().adrenaline = 0f;


            if (hasBegunAnimation == 0f)
                return;

            CameraPanSystem.PanTowards(NPC.Center, Utils.GetLerpValue(2f, animationFocusTime, AttackTimer, true) * Utils.GetLerpValue(0f, -animationFocusReturnTime, AttackTimer - animationFocusTime - animationTime, true));

            if (AttackTimer >= animationTime + animationFocusReturnTime)
            {
                savedAttackPos = NPC.Center;
                DragonAeria.HasYharimBeenTalkedBefore = true;
                SelectNextAttack(Attack.AirFallAttack);
            }
        }
        public Vector2 savedAttackPos = default(Vector2);
        public Vector2 savedAttackPos2 = default(Vector2);
        public float oldSwordRot = 0;
        public void Do_AirFallAttack()
        {
            int flyTime = 60;
            ref float attackSubstate = ref NPC.Calamity().newAI[0];

            NPC.noTileCollide = true;
            NPC.noGravity = true;

            if (AttackTimer < flyTime)
            {
                Vector2 v = Vector2.Lerp(savedAttackPos, Target.Center - new Vector2(0, 1000), AttackTimer / (float)flyTime);
                v.Y *= MathF.Pow(AttackTimer / (float)flyTime, 0.21f);
                NPC.Center = v;
            }
            if (AttackTimer == flyTime)
            {
                //NPC.velocity = new Vector2(0, 10);
                savedAttackPos = NPC.Center;
                savedAttackPos2 = Target.Center;
            }
            if (AttackTimer > flyTime && attackSubstate == 0)
            {
                //attackSubstate = 1;
                bool hasHitGround = Collision.SolidCollision(NPC.BottomRight - Vector2.UnitY * 4f, NPC.width, 6, true);
                bool ignoreTiles = NPC.Bottom.Y < savedAttackPos2.Y;
                bool pretendFakeCollisionHappened = NPC.Bottom.Y >= Target.Bottom.Y + 900f;

                if (hasHitGround && !ignoreTiles || pretendFakeCollisionHappened)
                {
                    AttackTimer = 0;
                    NPC.Calamity().newAI[1]++;
                }
            }
            if (NPC.Calamity().newAI[1] == 3)
            {

                SelectNextAttack(Attack.CircleProjectileSlash);
            }
        }
        public void Do_CircleProjectileSlash()
        {

        }
        public void Do_MinosTeleports()
        {

        }
        public void Do_Illusions()
        {

        }
        public void Do_ChaseRotation()
        {

        }
        public void Do_()
        {

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D t = ModContent.Request<Texture2D>(Texture).Value;
            int maxFrameY = 1;
            //Rectangle? r = NPC.frame;
            Rectangle? r = null;

            Texture2D sword = ModContent.Request<Texture2D>(Texture + "_Sword").Value;

            if (CurrentAttack != Attack.Idle)
                spriteBatch.Draw(sword, NPC.Center + Vector2.One.RotatedBy(SwordRotation) * (sword.Size().Length() / 3 * 2) - screenPos, null, drawColor, SwordRotation, sword.Size() / 2, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(t, NPC.Center - Main.screenPosition, r, drawColor, NPC.rotation, t.Size() / 2, 1f, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            float collisionPoint = 0f;
            return target.Hitbox.Intersects(NPC.Hitbox) || Collision.CheckAABBvLineCollision(target.TopLeft, target.Size, NPC.Center, NPC.Center + Vector2.One.RotatedBy(SwordRotation) * (new Vector2(140).Length()), 32, ref collisionPoint);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(SwordRotationVelocity);
            writer.WriteVector2(savedAttackPos);
            writer.WriteVector2(savedAttackPos2);
            writer.Write(oldSwordRot);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            SwordRotationVelocity = reader.ReadSingle();
            savedAttackPos = reader.ReadVector2();
            savedAttackPos2 = reader.ReadVector2();
            oldSwordRot = reader.ReadSingle();
        }
    }
}
