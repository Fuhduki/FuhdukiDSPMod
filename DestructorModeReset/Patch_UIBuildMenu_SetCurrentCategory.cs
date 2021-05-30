using HarmonyLib;

namespace DestructorModeReset
{
    [HarmonyPatch(typeof(UIBuildMenu), "SetCurrentCategory")]
    public class Patch_UIBuildMenu_SetCurrentCategory
    {
        public static void Postfix(int category, UIBuildMenu __instance, ref Player ___player, ref BuildTool_Dismantle ___dismantleTool, ref bool ___forceDismantleChain)
        {
            if (___player == null)
                return;

            if (!__instance.isDismantleMode)
                return;

            if (DestructorModeResetPlugin.PluginConfig.EnableDecstructCursorReset)
            {
                ___dismantleTool.cursorType = DestructorModeResetPlugin.PluginConfig.DestructCursor;
                if (DestructorModeResetPlugin.PluginConfig.DestructChain.HasValue)
                {
                    ___dismantleTool.chainReaction = DestructorModeResetPlugin.PluginConfig.DestructChain.Value;
                    ___forceDismantleChain = DestructorModeResetPlugin.PluginConfig.DestructChain.Value;
                }
            }

            if (DestructorModeResetPlugin.PluginConfig.EnableDestructFilterReset)
            {
                ___dismantleTool.filterFacility = DestructorModeResetPlugin.PluginConfig.DestructFilterFactory;
                ___dismantleTool.filterBelt = DestructorModeResetPlugin.PluginConfig.DestructFilterBelt;
                ___dismantleTool.filterInserter = DestructorModeResetPlugin.PluginConfig.DestructFilterInserter;
            }
        }
    }
}
