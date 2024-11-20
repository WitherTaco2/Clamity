using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamity.Assets
{
    public static class ClamityAssets
    {
        public static readonly string GreyscaleTexturesPath = "Clamity/Assets/Greyscale";
        public static readonly string TrailStreakTexturesPath = "Clamity/Assets/TrailStreak";


        public static readonly Texture2D BloomCircle = LoadDeferred($"{GreyscaleTexturesPath}/BloomCircle");

        public static readonly Texture2D BloomFlare = LoadDeferred($"{GreyscaleTexturesPath}/BloomFlare");

        public static readonly Texture2D DendriticNoise = LoadDeferred($"{GreyscaleTexturesPath}/DendriticNoise");

        public static readonly Texture2D ViscousNoise = LoadDeferred($"{GreyscaleTexturesPath}/ViscousNoise");

        public static readonly Texture2D StreakNightmareDeathray = LoadDeferred($"{TrailStreakTexturesPath}/StreakNightmareDeathray");

        public static readonly Texture2D StreakNightmareDeathrayOverlay = LoadDeferred($"{TrailStreakTexturesPath}/StreakNightmareDeathrayOverlay");

        public static readonly Texture2D StreakLightning = LoadDeferred($"{TrailStreakTexturesPath}/StreakLightning");


        private static Texture2D LoadDeferred(string path)
        {
            // Don't attempt to load anything serverside.
            if (Main.netMode == NetmodeID.Server)
                return default;

            return ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;
        }
        private static Texture2D[] LoadDeferred(string path, int textureCount)
        {
            // Don't attempt to load anything serverside.
            if (Main.netMode == NetmodeID.Server)
                return default;

            Texture2D[] textures = new Texture2D[textureCount];
            for (int i = 0; i < textureCount; i++)
                textures[i] = LoadDeferred($"{path}{i + 1}");

            return textures;
        }
    }
}
