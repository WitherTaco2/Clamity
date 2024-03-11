using CalamityMod.CalPlayer;

namespace Clamity.Commons
{
    public static class ClamityModCalls
    {
        public static bool GetBossDowned(string boss)
        {

            switch (boss.ToLower())
            {
                default:
                    return false;

                case "clamitas":
                case "sclam":
                case "supremeclamitas":
                case "supreme clamitas":
                case "clamitas supreme clam":
                    return ClamitySystem.downedClamitas;

                case "pyrogen":
                    return ClamitySystem.downedPyrogen;

                case "wob":
                case "wallofbronze":
                case "wall of bronze":
                    return ClamitySystem.downedWallOfBronze;
            }
        }
        public static bool GetInZone(Player p, string zone)
        {

            CalamityPlayer mp = p.Calamity();
            switch (zone.ToLower())
            {
                default:
                    return false;
                case "frozenhell":
                case "frozen hell":
                case "snow hell":
                    return p.Clamity().ZoneFrozenHell;
            }
        }
        public static object Call(params object[] args)
        {
            bool isValidPlayerArg(object o) => o is int || o is Player;
            bool isValidItemArg(object o) => o is int || o is Item;
            bool isValidProjectileArg(object o) => o is int || o is Projectile;
            bool isValidNPCArg(object o) => o is int || o is NPC;

            Player castPlayer(object o)
            {
                if (o is int i)
                    return Main.player[i];
                else if (o is Player p)
                    return p;
                return null;
            }

            Item castItem(object o)
            {
                if (o is int i)
                    return Main.item[i];
                else if (o is Item it)
                    return it;
                return null;
            }

            Projectile castProjectile(object o)
            {
                if (o is int i)
                    return Main.projectile[i];
                else if (o is Projectile p)
                    return p;
                return null;
            }

            NPC castNPC(object o)
            {
                if (o is int i)
                    return Main.npc[i];
                else if (o is NPC n)
                    return n;
                return null;
            }


            if (args is null || args.Length <= 0)
                return new ArgumentNullException("CALL ERROR: No function name specified. First argument must be a function name.");
            if (!(args[0] is string))
                return new ArgumentException("CALL ERROR: First argument must be a string function name.");

            string methodName = args[0].ToString();

            switch (methodName)
            {
                case "Downed":
                case "GetDowned":
                case "BossDowned":
                case "GetBossDowned":
                    if (args.Length < 2)
                        return new ArgumentNullException("CALL ERROR: Must specify a boss or event name as a string.");
                    if (!(args[1] is string))
                        return new ArgumentException("CALL ERROR: The argument to \"Downed\" must be a string.");
                    return GetBossDowned(args[1].ToString());

                case "Zone":
                case "GetZone":
                case "InZone":
                case "GetInZone":
                    if (args.Length < 2)
                        return new ArgumentNullException("CALL ERROR: Must specify both a Player object (or int index of a Player) and a zone name as a string.");
                    if (args.Length < 3)
                        return new ArgumentNullException("CALL ERROR: Must specify a zone name as a string.");
                    if (!(args[2] is string))
                        return new ArgumentException("CALL ERROR: The second argument to \"InZone\" must be a string.");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("CALL ERROR: The first argument to \"InZone\" must be a Player or an int.");
                    return GetInZone(castPlayer(args[1]), args[2].ToString());

                default:
                    return new ArgumentException("CALL ERROR: Invalid method name.");
            }
        }
    }
}
