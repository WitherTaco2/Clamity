using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Content.Bosses.WoB.NPCs
{
    internal class MetalMaw : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

        }
        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 22;
            NPC.damage = 140;
            NPC.defense = 35;
            //NPC.lifeMax = 1200;
            NPC.LifeMaxNERB(1200, 3500, 5000);
            NPC.DR_NERD(0.25f);
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
        }
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            NPC.spriteDirection = NPC.direction = (NPC.velocity.X > 0).ToDirectionInt();
            NPC.rotation = NPC.velocity.ToRotation() + (NPC.spriteDirection == 1 ? 0f : MathHelper.Pi);

            Player player = Main.player[NPC.target];

            Vector2 toDestination = player.Center - NPC.Center;
            Vector2 toDestinationNormalized = toDestination.SafeNormalize(Vector2.UnitY);
            float speed = Math.Min(400, toDestination.Length());
            Vector2 chase = toDestinationNormalized * speed / 60;
            NPC.velocity = chase;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Heart, 1));
        }
    }
}
