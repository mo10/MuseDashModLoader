using ModHelper;

namespace Patch
{
    class DiscordFixEntry : IMod
    {
        public string Name => "DiscordFix";

        public string Description => "Discord RunCallbacks Fix";

        public string Author => "Mo10";

        public string HomePage => "https://github.com/mo10";

        public void DoPatching()
        {
            DiscordFix.DoPatching();
        }
    }
    class GraphicSettingsFixEntry : IMod
    {
        public string Name => "GraphicSettingsFix";

        public string Description => "Borderless Window Fix";

        public string Author => "Mo10";

        public string HomePage => "https://github.com/mo10";

        public void DoPatching()
        {
            GraphicSettingsFix.DoPatching();
        }
    }
}
