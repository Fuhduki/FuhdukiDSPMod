using HarmonyLib;

namespace DeleteFourceReset
{
    [HarmonyPatch(typeof(GameMain), "Begin")]
    class Patch_GameMain_Begin
    {
        [HarmonyPrefix]
        public static void GameMain_Begin_Prefix()
        {
            DestructorModeResetPlugin.Config.Clear();
            DestructorModeResetPlugin.Config.Reload();
        }
    }
}
