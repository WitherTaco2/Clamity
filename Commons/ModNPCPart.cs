using Microsoft.Xna.Framework;
using System.IO;
using Terraria;

namespace Clamity.Commons
{
    public class ModNPCPart : Entity
    {
        public float[] ai = new float[3] { 0, 0, 0 };
        public float rotation = 0;
        public ModNPCPart(int whoIam, Vector2 position, Vector2 velocity)
        {
            this.whoAmI = whoIam;
            this.position = position;
            this.velocity = velocity;
            SetDefaults();
        }

        public virtual void SetDefaults()
        {

        }

        public virtual void AI()
        {

        }

        public virtual void SendExtraAI(BinaryWriter writer)
        {

        }

        public virtual void ReceiveExtraAI(BinaryReader reader)
        {

        }

    }
    public static class ModNPCPartUtils
    {
        public static void CreateAll(this ModNPCPart[] array, Vector2 position, Vector2 velocity)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                array[i] = new ModNPCPart(i, position, velocity);
            }

        }
        public static void CreateAll(this ModNPCPart[] array, Vector2 position) => CreateAll(array, position, Vector2.Zero);
        public static void UpdateAll(this ModNPCPart[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                ModNPCPart npc = array[i];
                npc.position += npc.velocity;
                npc.AI();
            }
        }
        public static void NetSendAll(this ModNPCPart[] array, BinaryWriter writer)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                ModNPCPart npc = array[i];
                writer.WriteVector2(npc.position);
                writer.WriteVector2(npc.velocity);
                writer.Write(npc.rotation);
                foreach (var v in npc.ai)
                    writer.Write(v);
                npc.SendExtraAI(writer);
            }
        }
        public static void NetReceiveAll(this ModNPCPart[] array, BinaryReader reader)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                ModNPCPart npc = array[i];
                npc.position = reader.ReadVector2();
                npc.velocity = reader.ReadVector2();
                npc.rotation = reader.ReadSingle();
                int j = 0;
                foreach (var v in npc.ai)
                {
                    npc.ai[j] = reader.ReadSingle();
                    j++;
                }
                npc.ReceiveExtraAI(reader);
            }
        }
    }
}
