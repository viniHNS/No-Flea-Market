using System.Reflection;
using SPT.Reflection.Patching;
using EFT.UI.Ragfair;
using HarmonyLib;
using EFT.UI;
using UnityEngine;
using System;

namespace noFlea.MyPatches
{
    internal class FleaAddOfferButtonPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(RagfairScreen), "Awake");
        }

        [PatchPostfix]
        static void Postfix(RagfairScreen __instance)
        {
            try
            {
                if (Plugin.ShouldDisableMyOffers)
                {
                    DisableUIElement(__instance, "FilterSettingsPanel/MyOffersToggleSpawner/MyOffersToggle", "MyOffersToggle");
                }

                if (Plugin.ShouldDisableLockedIcon)
                {
                    DisableUIElement(__instance, "TopRightPanel/LockedIcon", "LockedIcon");
                }

                if (Plugin.ShouldDisableAddOfferButton)
                {
                    DisableUIElement(__instance, "TopRightPanel/AddOfferButton", "AddOfferButton");
                    DisableUIElement(__instance, "TopRightPanel/AddOfferButton/RaycastTarget", "RaycastTarget");
                }
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"Error in FleaAddOfferButtonPatch: {ex.Message}");
            }
        }

        private static void DisableUIElement(RagfairScreen ragfairScreen, string path, string elementName)
        {
            try
            {
                var gameObject = GameObject.Find($"Menu UI/UI/Ragfair Screen/{path}");

                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                    if (Plugin.IsDebugEnabled)
                    {
                        Plugin.LogSource.LogInfo($"{elementName} disabled successfully via GameObject.Find.");
                    }
                    return;
                }

                Transform ragfairTransform = ragfairScreen.transform;
                Transform elementTransform = ragfairTransform.Find(path);

                if (elementTransform != null)
                {
                    elementTransform.gameObject.SetActive(false);
                    if (Plugin.IsDebugEnabled)
                    {
                        Plugin.LogSource.LogInfo($"{elementName} disabled successfully via Transform.Find.");
                    }
                }
                else
                {
                    if (Plugin.IsDebugEnabled)
                    {
                        Plugin.LogSource.LogWarning($"{elementName} not found at path: {path}");
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"Error disabling {elementName}: {ex.Message}");
            }
        }
    }
}