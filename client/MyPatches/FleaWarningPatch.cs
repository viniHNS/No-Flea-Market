using System.Reflection;
using SPT.Reflection.Patching;
using EFT.UI.Ragfair;
using HarmonyLib;
using EFT.UI;

namespace noFlea.MyPatches
{
    internal class FleaWarningPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(RagfairAvailabilityWarning), nameof(RagfairAvailabilityWarning.Show));
        }

        [PatchPrefix]
        static bool Prefix(RagfairAvailabilityWarning __instance)
        {
            if (!Plugin.ShouldDisableFleaWarning)
                return true; 
            try
            {
                if (Plugin.IsDebugEnabled)
                {
                    Plugin.LogSource.LogInfo("RagfairAvailabilityWarning blocked by configuration.");
                }
                return false; 
            }
            catch (System.Exception ex)
            {
                Plugin.LogSource.LogError($"Error in FleaWarningPatch: {ex.Message}");
                return true; 
            }
        }
    }
}