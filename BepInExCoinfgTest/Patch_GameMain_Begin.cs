using HarmonyLib;

namespace BepInExCoinfgTest
{
    [HarmonyPatch(typeof(GameMain), "Begin")]
    class Patch_GameMain_Begin
    {
        [HarmonyPrefix]
        public static void GameMain_Begin_Prefix()
        {
            TestPlugin.Config.Clear();
            TestPlugin.Config.Reload();
        }
    }
}
