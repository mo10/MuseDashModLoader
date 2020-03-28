using HarmonyLib;
using ModHelper;
using System.Diagnostics;

namespace RedirectUnityLog
{
    public static class RedirectUnityLog
    {
        // Harmony反射 + BaseLog + 1
        private static int FrameIdx = 3;
        public static void DoPatching()
        {
            var harmony = new Harmony("com.github.mo10.redirectunitylog");

            var NoFormat = new System.Type[] { typeof(object) };
            var HasFormat = new System.Type[] { typeof(string), typeof(object[]) };

            var DebugLog = AccessTools.Method(typeof(UnityEngine.Debug), "Log", NoFormat);
            var DebugLogPrefix = AccessTools.Method(typeof(RedirectUnityLog), "Log");
            harmony.Patch(DebugLog, new HarmonyMethod(DebugLogPrefix));


            var DebugLogError = AccessTools.Method(typeof(UnityEngine.Debug), "LogError", NoFormat);
            var DebugLogErrorPrefix = AccessTools.Method(typeof(RedirectUnityLog), "LogError");
            harmony.Patch(DebugLogError, new HarmonyMethod(DebugLogErrorPrefix));

            var DebugLogWarning = AccessTools.Method(typeof(UnityEngine.Debug), "LogWarning", NoFormat);
            var DebugLogWarningPrefix = AccessTools.Method(typeof(RedirectUnityLog), "LogWarning");
            harmony.Patch(DebugLogWarning, new HarmonyMethod(DebugLogWarningPrefix));

            var DebugLogFormat = AccessTools.Method(typeof(UnityEngine.Debug), "LogFormat", HasFormat);
            var DebugLogFormatPrefix = AccessTools.Method(typeof(RedirectUnityLog), "LogFormat");
            harmony.Patch(DebugLogFormat, new HarmonyMethod(DebugLogFormatPrefix));

            var DebugLogErrorFormat = AccessTools.Method(typeof(UnityEngine.Debug), "LogErrorFormat", HasFormat);
            var DebugLogErrorFormatPrefix = AccessTools.Method(typeof(RedirectUnityLog), "LogErrorFormat");
            harmony.Patch(DebugLogErrorFormat, new HarmonyMethod(DebugLogErrorFormatPrefix));

            var DebugLogWarningFormat = AccessTools.Method(typeof(UnityEngine.Debug), "LogWarningFormat", HasFormat);
            var DebugLogWarningFormatPrefix = AccessTools.Method(typeof(RedirectUnityLog), "LogWarningFormat");
            harmony.Patch(DebugLogWarningFormat, new HarmonyMethod(DebugLogWarningFormatPrefix));

        }
        public static void BaseLog(UnityEngine.LogType logType, object message)
        {
            var frame = new StackTrace().GetFrame(FrameIdx);
            var className = frame.GetMethod().ReflectedType.Name;
            var methodName = frame.GetMethod().Name;

            ModLogger.AddLog(className, methodName, message);
            UnityEngine.Debug.unityLogger.Log(logType, message);
        }
        public static bool Log(object message)
        {
            BaseLog(UnityEngine.LogType.Log, message);
            return false;
        }
        public static bool LogError(object message)
        {
            BaseLog(UnityEngine.LogType.Error, message);
            return false;
        }
        public static bool LogWarning(object message)
        {
            BaseLog(UnityEngine.LogType.Warning, message);
            return false;
        }
        public static bool LogFormat(string format, params object[] args)
        {
            BaseLog(UnityEngine.LogType.Log, string.Format(format, args));
            return false;
        }
        public static bool LogErrorFormat(string format, params object[] args)
        {
            BaseLog(UnityEngine.LogType.Error, string.Format(format, args));
            return false;
        }
        public static bool LogWarningFormat(string format, params object[] args)
        {
            BaseLog(UnityEngine.LogType.Warning, string.Format(format, args));
            return false;
        }
    }
}
