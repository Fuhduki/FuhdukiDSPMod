using HarmonyLib;
using System.Reflection;

namespace DestructorModeReset
{
    [HarmonyPatch(typeof(PlayerAction_Build), "PrepareBuild")]
    class Patch_PlayerAction_Build_PrepareBuild
    {
        [HarmonyPrefix]
        public static void Prefix(PlayerAction_Build __instance)
        {
            var cmd = __instance.controller.cmd;
            var beforeDestructing = __instance.destructing;
            var nowDestructing = (cmd.mode == -1);

            // 解体モード切替時のみ処理
            if (beforeDestructing || !nowDestructing)
                return;

            if (DestructorModeResetPlugin.PluginConfig.EnableDecstructCursorReset)
            {
                __instance.destructCursor = DestructorModeResetPlugin.PluginConfig.DestructCursor;

                if(DestructorModeResetPlugin.PluginConfig.DestructChain.HasValue)
                {
                    var field = typeof(UIBuildMenu).GetField("forceDestructChain", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    field.SetValue(UIRoot.instance.uiGame.buildMenu, DestructorModeResetPlugin.PluginConfig.DestructChain.Value);
                    __instance.destructChain = DestructorModeResetPlugin.PluginConfig.DestructChain.Value;
                }
            }
        }
    }
}