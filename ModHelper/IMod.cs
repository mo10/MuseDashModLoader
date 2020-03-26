namespace ModHelper
{
    public interface IMod
    {
        /// <summary>
        /// Mod Name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Mod Description
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Mod Author
        /// </summary>
        string Author { get; }
        /// <summary>
        /// Project Home Page Link
        /// </summary>
        string HomePage { get; }
        /// <summary>
        /// Execute Mod
        /// </summary>
        void DoPatching();
    }
}