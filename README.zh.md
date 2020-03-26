# MuseDashModLoader
Muse Dash 插件加载器

[![Build status](https://ci.appveyor.com/api/projects/status/x6u9uqyk0cvrnoa5?svg=true)](https://ci.appveyor.com/project/mo10/musedashmodloader)

## 与修改游戏文件或第三方启动器有何不同？

安装MuseDashModLoader仅需要复制几个文件到游戏目录下即可，不会覆盖任何文件。

MuseDashModLoader将自动跟随游戏一同运行，无需您修改任何启动参数，与您平常启动游戏一样。

MuseDashModLoader不会修改您的游戏文件，游戏更新也不会影响到MuseDashModLoader的运行。

## 安装MuseDashModLoader

首先，[在这里](https://ci.appveyor.com/project/mo10/musedashmodloader/build/artifacts)下载Dist Release.zip

然后，解压里面的文件到Muse Dash游戏目录下

最后，运行游戏，MuseDashModLoader会随着您游戏自动运行

### 安装mod

将您的mod放在Mods文件夹即可。

如果您是第一次安装，没有找到Mods文件夹，可以手动创建或运行一次游戏。如果MuseDashModLoader工作正常，则会自动创建Mods文件夹。

### 卸载mod

将您要卸载的mod从Mods文件夹中移除即可。

### 如何找到我的Muse Dash游戏目录

打开Steam，在左侧游戏列表中找到Muse Dash，按下您的鼠标右键，点击"属性"，在属性窗口中的"本地文件"选项卡中点击"浏览本地文件"，就会打开Muse Dash所在目录。

## ModLoader.ini 设置

MuseDashModLoader 的一些初期行为可以在 `ModConfig.ini` 中进行设置

```ini
[UnityDoorstop]
# 是否启用ModLoader
enabled=true
# 指定注入dll
targetAssembly=ModLoader.dll
# 是否重定向 Unity 的日志到 <当前目录>\output_log.txt
redirectOutputLog=true
# 是否显示调试窗口 使用 ModLogger.Debug 方法进行输出
showConsoleWindow=true
```

## 听起来有点香，如何开发新插件呢？

MuseDashModLoader的插件开发非常简单：您只需要按照步骤操作即可快速上手：

需要的开发工具：Visual Studio (推荐使用2017或者2019)

创建插件工程：

1. 新建项目
2. 选择 `类库(.NET Framework)`
3. 选择框架版本 `.NET Framework 3.5`
4. 完成项目创建
5. 添加 [ModHelper](ModHelper) 项目到你的`解决方案`中
6. 在你的项目中添加对 `ModHelper` 的引用
7. 在 `ModHelper 引用属性` 中，设置 `复制本地` 为 `False`  
本ModLoader已经内嵌了 `Harmony`，如果你使用了 `Harmony` ，也需要设置 `复制本地` 为 `False`
8. 创建一个新的类，下面是例子代码:  
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

            // 如果你需要打印日志:
            // ModHelper.ModLogger.Debug("My log output");
        }
    }
}
```
9. 当 MuseDashModLoader 运行时，将会自动执行 `DoPatching()` 方法

## 异常

如果您的插件在执行 `DoPatching()` 方法时抛出异常,则可以在ModLogger.log找到报错信息。其他情况需要您自己实现异常捕捉。

## 鸣谢

MuseDashModLoader的诞生离不开许多开源项目。

Proxy:https://github.com/NeighTools/UnityDoorstop  
Harmony:https://github.com/pardeike/Harmony
