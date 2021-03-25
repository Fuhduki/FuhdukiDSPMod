using HarmonyLib;

namespace ProduceOnlyOneItem
{
    [HarmonyPatch(typeof(UIReplicatorWindow), "_OnRegEvent")]
    public class PatchUiAssemblerWindowOnRegEvent
    {
        [HarmonyPostfix]
        public static void Postfix(UIReplicatorWindow __instance)
        {
            ref var okButton = ref AccessTools.FieldRefAccess<UIReplicatorWindow, UIButton>(__instance, "okButton");
            okButton.onRightClick += OneItemProducePluginLogic.OnOkButtonRightClick;
            OneItemProducePlugin.Logger.LogInfo(("TestOutput"));
        }
    }
}
