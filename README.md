# MuseDashModLoader
Muse Dash 插件加载器

## 与修改游戏文件或第三方启动器有何不同？

安装MuseDashModLoader仅需要复制几个文件到游戏目录下即可，该过程中不会覆盖任何文件。

MuseDashModLoader将自动跟随游戏一同运行，无需您修改任何启动参数，与您平常启动游戏一样。

因为MuseDashModLoader不会修改您的游戏文件，所以就算游戏更新也不会影响到MuseDashModLoader的运行。

## 安装MuseDashModLoader

首先，请转到Muse Dash游戏目录下:

### Steam操作

打开Steam，在左侧游戏列表中找到Muse Dash，按下您金贵的右键，点击"属性"，在属性窗口中的"本地文件"选项卡中点击"浏览本地文件"，就会弹出Muse Dash所在目录


转到Muse Dash游戏目录后，将`winhttp.dll`,`ModLoader.dll`,`load_config.ini`,`IMod.dll`这四个文件复制过去即可。

插件放在`Mods`文件夹中，如果您没有这个文件夹，新建一个即可。

## 听起来有点香，如何开发新插件呢？

MuseDashModLoader的插件开发非常简单：您只需要按照步骤操作即可快速上手：

需要的开发工具：Visual Studio (推荐使用2017或者2019)

创建插件工程：

1. 打开Visual Studio
2. 克隆本仓库或使用现有`IMod.dll`文件均可
3. 新建工程 -> 类库(.NET Framework) 框架版本选择.NET Framework 3.5
4. 在创建好的工程中，找到解决方案资源管理器，然后在您的解决方案中添加本仓库的`IMod项目`到您的解决方案中，然后在您的插件项目中引用IMod项目即可。
如果您使用IMod.dll进行开发，只需在引用中选择该DLL即可。
5. 待续...
