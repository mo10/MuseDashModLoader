using ModHelper;
namespace RedirectUnityLog
{
    class Entry : IMod
    {
        public string Name => "RedirectUnityLog";

        public string Description => "Redirect Unity Debug Log";

        public string Author => "Mo10";

        public string HomePage => "https://github.com/mo10";

        public void DoPatching()
        {
            RedirectUnityLog.DoPatching();
        }
    }
}
