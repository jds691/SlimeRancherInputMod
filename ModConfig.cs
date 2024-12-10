using System.Linq;
using SRML.Config;
using SRML.Config.Attributes;

namespace SteamInputFixMod
{
    [ConfigFile("SteamInputFix")]
    public static class ModConfig
    {
        private static ConfigFile _configData;

        public static void Initialize()
        {
            _configData = ConfigFile.GenerateConfig(typeof(ModConfig));
            if (_configData.TryLoadFromFile())
            {
                var styleSection = _configData.Sections.Where(section => section.Name == "Style").First();
                StyleConfig.Init(styleSection);

                var generalSection = _configData.Sections.Where(section => section.Name == "General").First();
                GeneralConfig.Init(generalSection);

                var slimeRancherStyleSection = _configData.Sections
                    .Where(section => section.Name == "Style/Slime Rancher Style").First();
                SlimeRancherStyleConfig.Init(slimeRancherStyleSection);
            }
        }

        [ConfigSection("General")]
        public static class GeneralConfig
        {
            public static bool useAlways;

            public static void Init(ConfigSection sectionData)
            {
                var useAlwaysElement = sectionData.Elements.Where(x => x.ElementType == typeof(bool)).First();
                useAlwaysElement.OnValueChanged += UseAlwaysElementOnOnValueChanged;
                useAlways = useAlwaysElement.GetValue<bool>();
            }

            private static void UseAlwaysElementOnOnValueChanged(object newval)
            {
                useAlways = (bool)newval;
            }
        }

        [ConfigSection("Style")]
        public static class StyleConfig
        {
            public enum ButtonStyle
            {
                SlimeRancher,
                Steam
            }

            public static ButtonStyle preferredStyle = ButtonStyle.SlimeRancher;

            public static void Init(ConfigSection sectionData)
            {
                var preferredStyleElement =
                    sectionData.Elements.Where(x => x.ElementType == typeof(ButtonStyle)).First();

                preferredStyleElement.OnValueChanged += ButtonStyleHandler.OnPreferredStyleChanged;
                ButtonStyleHandler.OnPreferredStyleChanged(preferredStyleElement.GetValue<ButtonStyle>());
            }
        }

        // The Steam client has additional colour styles inside of Steam/controller_base/api
        // As far as I know there is no way to request a specific colour style via Steamworks, at least using the version Slime Rancher does
        // If there is a way to call a newer version of the API without breaking Slime Rancher I will implement it
        /*[ConfigSection("Steam Style")]
        public static class SteamStyleConfig
        {
            public static ColorScheme inGameColorScheme = ColorScheme.Light;
            public static ColorScheme menuColorScheme = ColorScheme.Dark;

            public enum ColorScheme
            {
                Light,
                LightColorful,
                LightOutline,
                Dark,
                DarkColorful,
                DarkOutline
            }
        }*/

        [ConfigSection("Style/Slime Rancher Style")]
        public static class SlimeRancherStyleConfig
        {
            [ConfigComment(
                "For Steam Controllers and the Steam Deck, you can use the Xbox button prompts instead of the old Steam artwork.")]
            public static bool replaceSteamWithXboxButtons = true;

            public static void Init(ConfigSection sectionData)
            {
                var replaceSteamWithXboxButtonsElement = sectionData.Elements
                    .Where(x => x.Options.Name == "replaceSteamWithXboxButtons").First();
                replaceSteamWithXboxButtons = replaceSteamWithXboxButtonsElement.GetValue<bool>();
            }
        }
    }
}