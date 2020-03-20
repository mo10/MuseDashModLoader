using System;

public interface IMod
{
    /// <summary>
    /// 插件名
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 插件描述
    /// </summary>
    string Description { get; }
    /// <summary>
    /// 插件作者
    /// </summary>
    string Author { get; }
    /// <summary>
    /// 项目主页链接
    /// </summary>
    string HomePage { get; }
    /// <summary>
    /// 在特定Assembly加载后执行DoPatching
    /// </summary>
    string RequireAssembly { get; }
    /// <summary>
    /// 执行插件
    /// </summary>
    void DoPatching();
}