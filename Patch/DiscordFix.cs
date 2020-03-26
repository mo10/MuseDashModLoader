using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using HarmonyLib;
using System;
using UnityEngine.Events;

namespace Patch
{
    public static class DiscordFix
    {
        public static void DoPatching()
        {
            var harmony = new Harmony("com.github.mo10.discordfix");

            var SetNoBorder = AccessTools.Method(typeof(DiscordManager), "InitDiscord");
            var SetNoBorderPreFix = AccessTools.Method(typeof(DiscordFix), "InitDiscord");

            harmony.Patch(SetNoBorder, new HarmonyMethod(SetNoBorderPreFix));
        }
        private static DiscordManager instance;
        public static bool InitDiscord(DiscordManager __instance)
        {
            ModHelper.ModLogger.Debug("Triggered");
            instance = __instance;
            instance.discord = new Discord.Discord(599659394082406493L, 1UL);
            SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop(new UnityAction<float>(DiscordRunCallbacks), UnityGameManager.LoopType.Update, float.MaxValue);
            instance.activityManager = instance.discord.GetActivityManager();
            instance.applicationManager = instance.discord.GetApplicationManager();
            instance.SetUpdateActivity(false, string.Empty);
            return false;
        }
        public static void DiscordRunCallbacks(float time)
        {
            try
            {
                instance.discord.RunCallbacks();
            }
            catch (Exception)
            {
                // do nothing
            }
        }
    }
}
