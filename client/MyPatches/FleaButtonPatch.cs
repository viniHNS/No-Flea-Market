using System;
using System.Collections.Generic;
using System.Reflection;
using SPT.Reflection.Patching;
using EFT.UI;
using HarmonyLib;

namespace noFlea.MyPatches
{
    internal class FleaButtonPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MenuTaskBar), nameof(MenuTaskBar.Awake));
        }

        [PatchPostfix]
        static void Postfix(MenuTaskBar __instance)
        {
            if (!Plugin.ShouldDisableFleaMarketTab)
                return;

            try
            {
                var toggleButtonsField = AccessTools.Field(typeof(MenuTaskBar), "_toggleButtons");
                var toggleButtons = toggleButtonsField.GetValue(__instance) as Dictionary<EMenuType, AnimatedToggle>;

                if (toggleButtons != null && toggleButtons.ContainsKey(EMenuType.RagFair))
                {
                    var fleaMarketToggle = toggleButtons[EMenuType.RagFair];

                    if (fleaMarketToggle != null && fleaMarketToggle.gameObject != null)
                    {
                        var parentObject = fleaMarketToggle.transform.parent.gameObject;
                        parentObject.SetActive(false);

                        if (Plugin.IsDebugEnabled)
                        {
                            Plugin.LogSource.LogInfo("Flea Market Tab Button disabled by configuration.");
                        }
                    }
                    else
                    {
                        if (Plugin.IsDebugEnabled)
                        {
                            Plugin.LogSource.LogWarning("Flea Market Button not found in _toggleButtons.");
                        }
                    }
                }
                else
                {
                    if (Plugin.IsDebugEnabled)
                    {
                        Plugin.LogSource.LogWarning("Flea Market Button not found in _toggleButtons.");
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"Error in FleaButtonPatch: {ex.Message}");
            }
        }
    }
}