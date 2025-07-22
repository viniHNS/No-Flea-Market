using EFT;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.Ragfair;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace noFlea.MyPatches
{
    internal class FleaAddOfferContextMenuPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ItemUiContext), "Show") ??
                   AccessTools.Method(typeof(ItemContextClass), "Show") ??
                   AccessTools.FirstMethod(typeof(ItemUiContext), method => method.Name.Contains("Show"));
        }

        [PatchPostfix]
        static void Postfix()
        {
            if (!Plugin.ShouldDisableContextMenuAddOffer)
                return;

            try
            {
                DisableAddOfferButtonImmediate();

                StaticManager.BeginCoroutine(DisableAddOfferButtonCoroutine());
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"Error in context menu patch: {ex.Message}");
            }
        }

        private static void DisableAddOfferButtonImmediate()
        {
            try
            {
                var contextMenuArea = GameObject.Find("Preloader UI/Preloader UI/UIContext/Context Menu Area/ItemContextMenu/InteractionButtonsContainer");

                if (contextMenuArea != null)
                {
                    foreach (Transform child in contextMenuArea.transform)
                    {
                        if (child.gameObject.activeInHierarchy)
                        {
                            var textComponent = child.GetComponentInChildren<TextMeshProUGUI>();
                            if (textComponent != null && IsAddOfferText(textComponent.text))
                            {
                                child.gameObject.SetActive(false);
                                if (Plugin.IsDebugEnabled)
                                {
                                    Plugin.LogSource.LogInfo($"Disabled ADD OFFER button immediately: {child.name}");
                                }
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Plugin.IsDebugEnabled)
                {
                    Plugin.LogSource.LogError($"Error disabling ADD OFFER button immediately: {ex.Message}");
                }
            }
        }

        private static System.Collections.IEnumerator DisableAddOfferButtonCoroutine()
        {
            for (int attempts = 0; attempts < 5; attempts++)
            {
                yield return null;

                try
                {
                    var contextMenuArea = GameObject.Find("Preloader UI/Preloader UI/UIContext/Context Menu Area/ItemContextMenu/InteractionButtonsContainer");

                    if (contextMenuArea != null && contextMenuArea.activeInHierarchy)
                    {
                        foreach (Transform child in contextMenuArea.transform)
                        {
                            if (child.gameObject.activeInHierarchy)
                            {
                                var textComponent = child.GetComponentInChildren<TextMeshProUGUI>();
                                if (textComponent != null && IsAddOfferText(textComponent.text))
                                {
                                    child.gameObject.SetActive(false);
                                    if (Plugin.IsDebugEnabled)
                                    {
                                        Plugin.LogSource.LogInfo($"Disabled ADD OFFER button: {child.name} (backup attempt {attempts + 1})");
                                    }
                                    yield break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (Plugin.IsDebugEnabled)
                    {
                        Plugin.LogSource.LogError($"Error disabling ADD OFFER button (backup attempt {attempts + 1}): {ex.Message}");
                    }
                }
            }
        }

        private static bool IsAddOfferText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            string upperText = text.ToUpper();
            return upperText.Contains("ADD OFFER") ||
                   upperText.Contains("ДОБАВИТЬ") ||
                   upperText.Contains("СОЗДАТЬ ЛОТ") ||
                   upperText.Contains("AJOUTER") ||
                   upperText.Contains("AGREGAR") ||
                   upperText.Contains("HINZUFÜGEN") ||
                   upperText.Contains("ADICIONAR OFERTA") ||
                   upperText.Contains("新着交易单") ||
                   upperText.Contains("PŘIDAT NABÍDKU") ||
                   upperText.Contains("DODAJ OFERTĘ") ||
                   upperText.Contains("AÑADIR OFERTA") ||
                  (upperText.Contains("ADD") && upperText.Contains("OFFER"));
        }
    }
}