# MuseDashModLoader
A Muse Dash Mod Loader

[![Build status](https://ci.appveyor.com/api/projects/status/x6u9uqyk0cvrnoa5?svg=true)](https://ci.appveyor.com/project/mo10/musedashmodloader)

[中文Readme](README.zh.md)

## Now available mods

- Patch : Fix game borderless implementation. Fix Discord API error.
- RedirectUnityLog : Redirect and trace the output of methods like Debug.Log () to Modlogger.Helpful for development.

## How is it different from modifying game files or third-party launchers?

To install MuseDashModLoader, you only need to copy a few files to the game root directory.

No files are overwritten.

No need to install again after game update.

No third-party launcher required.

## Install

You can download the latest stable version in [Github releases](releases)

Or download the latest development version in [Appveyor artifacts](https://ci.appveyor.com/project/mo10/musedashmodloader/build/artifacts)

  - `Dist Release`:For users, it does not contain program symbols.

    If you are not a developer.Download it is enough.

  - `Debug Release`:For developers with program symbols.

Extract files into the root directory of Muse Dash.

Congratulations, MuseDashModLoader has been installed!

MuseDashModLoader will launch automatically when you start the game.

## Install the mods

Put the mod file (.dll extension) into the `Mods` folder.

`Mods` folder will be created automatically after you run the game once.

## Uninstall mods

Just delete it from the `Mods` folder.

## ModLoader.ini Configuration

Some initial behavior of MuseDashModLoader can be set in `ModConfig.ini`

```ini
[UnityDoorstop]
# Specify whether to enable Modloader
enabled=true
# Specify the dll to be injected
targetAssembly=ModLoader.dll
# Specifies whether Unity's output log should be redirected to <current folder>\output_log.txt
redirectOutputLog=true
# Specifies whether to display the debug window. Using ModLogger.Debug() method
showConsoleWindow=true
```

## Develop new mods

Development tools: Visual Studio 2019

1. Create a new project.
2. Select `Class Library (.NET Framework)` tamplate.
3. Select Framework version: `.NET Framework 3.5`.
4. Complete project creation.
5. Add [ModHelper](ModHelper) project to your `Solution`.
6. Add `ModHelper` reference to your project.
7. In `ModHelper Reference Properties`, set `Copy Local` to `False`.  
If you use `Harmony` Library in your project, you also need to set `Copy Local` to `False`.  
ModLoader already contains the library.
8. Create a new class,here is an example.  
```csharp
using HarmonyLib;
using ModHelper;

namespace MyAwesomeProject
{
    class MyAwesomeMod : IMod
    {
        public string Name => "A awesome mod";

        public string Description => "An example of mod";

        public string Author => "Mo10";

        public string HomePage => "https://github.com/mo10/MuseDashModLoader";

        public void DoPatching()
        {
            var harmony = new Harmony("com.github.mo10.myawesomemod");
            // To do somthing...

            // When you want to print the log:
            // ModHelper.ModLogger.Debug("My log output");
        }
    }
}
```
9. When the MuseDashModLoader runs, the DoPatching method will be executed.

## Catch exception

If your mod throws an exception in DoPatching(), you can find the exception message in `Modlogger.log`.

## Thanks

Muse Dash Mod Loader uses the following projects.

Proxy:https://github.com/NeighTools/UnityDoorstop  
Harmony:https://github.com/pardeike/Harmony
