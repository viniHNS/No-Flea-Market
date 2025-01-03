using System.Reflection;
using SPT.Reflection.Patching;
using EFT.UI.Ragfair;
using HarmonyLib;

namespace noFlea.MyPatches
{
    internal class FleaWarningPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(RagfairAvailabilityWarning), nameof(RagfairAvailabilityWarning.Show));
        }

        [PatchPostfix]
        static void Postfix(RagfairAvailabilityWarning __instance)
        {
            try
            {
                __instance.Close();
                Plugin.LogSource.LogInfo("RagfairAvailabilityWarning closed.");
            }
            catch (System.Exception ex)
            {
                Plugin.LogSource.LogError($"Error: {ex.Message}");
            }
        }
    }
}