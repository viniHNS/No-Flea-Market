using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using noFlea.MyPatches;

namespace noFlea
{
    [BepInPlugin("com.vinihns.noFlea", "No Flea Market", "1.3.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        private static class ModConfig
        {
            public static ConfigEntry<bool> DisableFleaWarning;
            public static ConfigEntry<bool> DisableFleaButtons;
            public static ConfigEntry<bool> DisableContextMenuAddOffer;
            public static ConfigEntry<bool> DisableMyOffersToggle;
            public static ConfigEntry<bool> DisableLockedIcon;
            public static ConfigEntry<bool> DisableFleaMarketTab; 
            public static ConfigEntry<bool> EnableDebugLogging;
        }

        private void Awake()
        {
            LogSource = Logger;
            LogSource.LogInfo("Loading No Flea Market configuration...");

            CreateConfigurations();
            ApplyPatches();

            LogSource.LogInfo("No Flea Market loaded successfully!");
        }

        private void CreateConfigurations()
        {
            ModConfig.DisableFleaWarning = Config.Bind(
                "1. Popups & Warnings",
                "Disable Flea Warning Popup",
                true,
                "Disable the flea market availability warning popup that appears when accessing flea market."
            );

            ModConfig.DisableFleaMarketTab = Config.Bind(
                "2. User Interface",
                "Disable Flea Market Tab",
                false,
                "Disable the flea market tab button in the main menu taskbar. REQUIRES GAME RESTART"
            );

            ModConfig.DisableFleaButtons = Config.Bind(
                "2. User Interface",
                "Disable Add Offer Button",
                true,
                "Disable the 'Add Offer' button in the flea market screen. REQUIRES GAME RESTART"
            );

            ModConfig.DisableMyOffersToggle = Config.Bind(
                "2. User Interface",
                "Disable My Offers Toggle",
                true,
                "Disable the 'My Offers' toggle button in the flea market screen. REQUIRES GAME RESTART"
            );

            ModConfig.DisableLockedIcon = Config.Bind(
                "2. User Interface",
                "Disable Locked Icon",
                true,
                "Disable the locked icon that appears near flea market elements. REQUIRES GAME RESTART"
            );

            ModConfig.DisableContextMenuAddOffer = Config.Bind(
                "3. Context Menu",
                "Disable Add Offer in Right-Click Menu",
                true,
                "Disable 'Add Offer' option when right-clicking on items."
            );

            ModConfig.EnableDebugLogging = Config.Bind(
                "4. Debug",
                "Enable Debug Logging",
                false,
                "Enable detailed logging for troubleshooting (may impact performance). REQUIRES GAME RESTART"
            );
        }

        private void ApplyPatches()
        {
            LogSource.LogInfo("Applying patches based on configuration...");

            if (ModConfig.DisableFleaWarning.Value)
            {
                new FleaWarningPatch().Enable();
                LogSource.LogInfo("Flea Warning Patch enabled");
            }

            if (ModConfig.DisableFleaButtons.Value || ModConfig.DisableMyOffersToggle.Value || ModConfig.DisableLockedIcon.Value)
            {
                new FleaAddOfferButtonPatch().Enable();
                LogSource.LogInfo("Flea UI Elements Patch enabled");
            }

            if (ModConfig.DisableContextMenuAddOffer.Value)
            {
                new FleaAddOfferContextMenuPatch().Enable();
                LogSource.LogInfo("Context Menu Patch enabled");
            }

            LogSource.LogInfo($"Configuration summary:");
            LogSource.LogInfo($"  - Flea Warning: {(ModConfig.DisableFleaWarning.Value ? "DISABLED" : "enabled")}");
            LogSource.LogInfo($"  - UI Buttons: {(ModConfig.DisableFleaButtons.Value ? "DISABLED" : "enabled")}");
            LogSource.LogInfo($"  - Context Menu: {(ModConfig.DisableContextMenuAddOffer.Value ? "DISABLED" : "enabled")}");
            LogSource.LogInfo($"  - Debug Logging: {(ModConfig.EnableDebugLogging.Value ? "ENABLED" : "disabled")}");
        }

        public static bool IsDebugEnabled => ModConfig.EnableDebugLogging.Value;
        public static bool ShouldDisableMyOffers => ModConfig.DisableMyOffersToggle.Value;
        public static bool ShouldDisableLockedIcon => ModConfig.DisableLockedIcon.Value;
        public static bool ShouldDisableAddOfferButton => ModConfig.DisableFleaButtons.Value;
        public static bool ShouldDisableFleaWarning => ModConfig.DisableFleaWarning.Value;
        public static bool ShouldDisableFleaMarketTab => ModConfig.DisableFleaMarketTab.Value;
        public static bool ShouldDisableContextMenuAddOffer => ModConfig.DisableContextMenuAddOffer.Value;
    }

}