using ModHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace ModLoader
{
    public class ModLoader
    {
        private static List<IMod> mods = new List<IMod>();
        private static Dictionary<string, Assembly> depends = new Dictionary<string, Assembly>();
        private static void OnAssemblyLoadEventHandler(object sender, AssemblyLoadEventArgs args)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (args.LoadedAssembly.GetName().Name  == "Assembly-CSharp")
            {
                ModLogger.Debug("Start Load Mod");
                // Try to load mods
                try
                {
                    if (!Directory.Exists($"{assemblyFolder}/Mods"))
                        Directory.CreateDirectory($"{assemblyFolder}/Mods");

                    mods = LoadMods<IMod>($"{assemblyFolder}/Mods");

                    ModLogger.Debug("Loaded mods:");
                    foreach (IMod mod in mods)
                    {
                        ModLogger.Debug($"Name:{mod.Name} Desc:{mod.Description}");
                        mod.DoPatching();
                    }
                    ModLogger.Debug($"==========end==========");
                }
                catch (Exception ex)
                {
                    ModLogger.Debug($"Caught exception from {ex.Source}:\n{ex}");
                }
            }
        }
        /// <summary>
        /// mono inject entry
        /// </summary>
        /// <param name="args"></param>
        public static void Injector(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string managedFolder = Environment.GetEnvironmentVariable("DOORSTOP_MANAGED_FOLDER_DIR");
            //bool isWaitingForDebugger = Environment.GetEnvironmentVariable("WAITING_FOR_DEBUGGER") == "FALSE" ? false : true;

            LoadDependency(Assembly.GetCallingAssembly());

            var loadedAssemblies = new Dictionary<string, Assembly>();

            currentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoadEventHandler);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, arg) =>
            {

                if (depends.TryGetValue(arg.Name, out Assembly loadedAssembly))
                {
                    return loadedAssembly;
                }
                return null;

            };
            ModLogger.Debug("Waiting for Assembly-CSharp loaded.");
        }



        /// <summary>
        /// Load Mods Dll
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<T> LoadMods<T>(string path)
        {
            string[] dllFiles;
            List<T> mods = new List<T>();

            if (!Directory.Exists(path))
            {
                return null;
            }
            dllFiles = Directory.GetFiles(path, "*.dll");

            foreach (var dllFile in dllFiles)
            {
                try
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(an);
                    
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetInterface(typeof(T).ToString()) != null)
                        {
                            mods.Add((T)Activator.CreateInstance(type));
                        }
                    }
                    LoadDependency(assembly);
                }
                catch (Exception)
                {
                    // Load failed
                    ModLogger.Debug($"Cannot load mod:{dllFile}");
                }
            }
            return mods;
        }

        private static void LoadDependency(Assembly assembly)
        {
            foreach (string dependStr in assembly.GetManifestResourceNames())
            {
                string filter = $"{assembly.GetName().Name}.Depends.";
                if (dependStr.StartsWith(filter) && dependStr.EndsWith(".dll"))
                {
                    string dependName = dependStr.Remove(dependStr.LastIndexOf(".dll")).Remove(0, filter.Length);
                    if (depends.ContainsKey(dependName))
                    {
                        ModLogger.Debug($"Dependency conflict: {dependName} First at: {depends[dependName].GetName().Name}");
                        continue;
                    }

                    Assembly dependAssembly;
                    using (var stream = assembly.GetManifestResourceStream(dependStr))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        dependAssembly = Assembly.Load(buffer);
                    }
                    ModLogger.Debug($"Dependency added: {dependName}");

                    depends.Add(dependName, dependAssembly);
                }
            }
        }
    }
}
