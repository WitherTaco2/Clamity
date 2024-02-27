using Clamity.Content.Boss.ItemBossRush;
using System;
using System.IO;
using Terraria.ModLoader;

namespace Clamity
{
    public class ClamityNetcode
    {
        public static void HandlePacket(Mod mod, BinaryReader reader, int whoAmI)
        {
            try
            {
                ClamityMessageType msgType = (ClamityMessageType)reader.ReadByte();
                switch (msgType)
                {
                    case ClamityMessageType.BulletTheoryStage:
                        int stage = reader.ReadInt32();
                        TwentyTwoBulletTheory.BulletTheoryStage = stage;
                        break;
                    case ClamityMessageType.BulletTheoryStartTimer:
                        TwentyTwoBulletTheory.StartTimer = reader.ReadInt32();
                        break;
                    case ClamityMessageType.BulletTheoryEndTimer:
                        TwentyTwoBulletTheory.EndTimer = reader.ReadInt32();
                        break;
                    case ClamityMessageType.EndBulletTheory:
                        TwentyTwoBulletTheory.EndEffects();
                        break;
                }
            }
            catch (Exception e)
            {
                if (e is EndOfStreamException eose)
                    Clamity.mod.Logger.Error("Failed to parse Calamity packet: Packet was too short, missing data, or otherwise corrupt.", eose);
                else if (e is ObjectDisposedException ode)
                    Clamity.mod.Logger.Error("Failed to parse Calamity packet: Packet reader disposed or destroyed.", ode);
                else if (e is IOException ioe)
                    Clamity.mod.Logger.Error("Failed to parse Calamity packet: An unknown I/O error occurred.", ioe);
                else
                    throw; // this either will crash the game or be caught by TML's packet policing
            }
        }
    }
}
