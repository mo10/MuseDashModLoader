using ModHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ModLoader
{
    public class ModLoader
    {
        static List<IMod> mods = new List<IMod>();

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
            var loadedAssemblies = new Dictionary<string, Assembly>();

            currentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoadEventHandler);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, arg) =>
            {
                String resourceName = $"ModLoader.Includes.{new AssemblyName(arg.Name).Name}.dll";

                //Must return the EXACT same assembly, do not reload from a new stream
                if (loadedAssemblies.TryGetValue(resourceName, out Assembly loadedAssembly))
                {
                    return loadedAssembly;
                }

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                        return null;
                    Byte[] assemblyData = new Byte[stream.Length];

                    stream.Read(assemblyData, 0, assemblyData.Length);

                    var assembly = Assembly.Load(assemblyData);
                    loadedAssemblies[resourceName] = assembly;
                    return assembly;
                }
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
                }
                catch (Exception)
                {
                    // Load failed
                    ModLogger.Debug($"Cannot load mod:{dllFile}");
                }
            }
            return mods;
        }
    }
}
