using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace ModLoader
{

    public static class ModLogger
    {
        private static StreamWriter fs = File.CreateText("ModLogger.log");
        private static Object  locker = new Object();
        public static void Debug(object obj)
        {
            var frame = new StackTrace().GetFrame(1);
            var className = frame.GetMethod().ReflectedType.Name;
            var methodName = frame.GetMethod().Name;
            AddLog(className, methodName, obj);
        }
        private static void AddLog(string className, string methodName,object obj)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            var text = $"[{time}][{className}:{methodName}]: {obj}";

            lock (locker)
            {
                fs.WriteLine(text);
                fs.Flush();
            }
        }
    }
}
