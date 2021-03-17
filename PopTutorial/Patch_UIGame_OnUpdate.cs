using HarmonyLib;
using UnityEngine;

namespace PopTutorial
{
    [HarmonyPatch(typeof(UIGame), "_OnUpdate")]
    class Patch_UIGame_OnUpdate
    {
        [HarmonyPostfix]
        public static void Prefix(UIGame __instance)
        {
            if(Input.GetKeyDown(KeyCode.F7))
            {
                PopTutorialPlugin.Logger.LogInfo("Push F7");
                var tip = LDB.advisorTips.Select(1);
                PopTutorialPlugin.Logger.LogInfo(tip.Script.Translate());
                var advisorId = 101;
                GameMain.history.UnregFeatureKey(2000000 + advisorId);
                __instance.RequestAdvisorTip(advisorId);
            }
        }
    }
}
