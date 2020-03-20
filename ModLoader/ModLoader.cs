using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ModLoader
{
    public class ModLoader
    {
        static TextWriter f;
        static List<IMod> mods = new List<IMod>();
        /// <summary>
        /// DLL加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void AssemblyLoadEventHandler(object sender, AssemblyLoadEventArgs args)
        {
            var name = args.LoadedAssembly.GetName().Name;
            f?.WriteLine($"Loaded:{name}");
            foreach(var mod in mods)
            {
                try
                {
                    if (mod.RequireAssembly != name)
                        continue;
                    f?.WriteLine($"-->Do Patching:{mod.Name}");
                    mod.DoPatching();
                }catch(Exception ex)
                {
                    f.WriteLine($"Caught exception from {mod.Name}({ex.Source}):\n{ex}");
                }
            }
            f?.Flush();
        }
        /// <summary>
        /// 注入插件主入口
        /// </summary>
        /// <param name="args"></param>
        public static void Injector(string[] args)
        {
            var loadedAssemblies = new Dictionary<string, Assembly>();
            AppDomain.CurrentDomain.AssemblyResolve += (sender, arg) =>
            {
                String resourceName = "ModLoader.Includes." +
                new AssemblyName(arg.Name).Name + ".dll";

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

            AppDomain currentDomain = AppDomain.CurrentDomain;
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            f = File.CreateText("ModLoader.log");
            try
            {
                mods = LoadMods<IMod>($"{assemblyFolder}/Mods");
                f.WriteLine($"Loaded mods:");
                foreach (IMod mod in mods)
                {
                    f.WriteLine($"Name:{mod.Name} Desc:{mod.Description} Require:{mod.RequireAssembly}");
                }
                f.WriteLine($"==========end==========");
            }
            catch(Exception ex)
            {
                f.WriteLine($"Caught exception from {ex.Source}:\n{ex}");
            }
            f.Flush();
            // 游戏dll加载事件监听
            currentDomain.AssemblyLoad += new AssemblyLoadEventHandler(AssemblyLoadEventHandler);
        }
        /// <summary>
        /// 加载插件文件
        /// </summary>
        /// <param name="path">插件目录</param>
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
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    // 插件加载失败
                }
            }
            return mods;
        }
    }
}
