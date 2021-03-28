using HarmonyLib;

namespace ProduceOnlyOneItem
{
    [HarmonyPatch(typeof(UIReplicatorWindow), "_OnUnregEvent")]
    public class PatchUiAssemblerWindowOnUnregEvent
    {
        [HarmonyPostfix]
        public static void Postfix(UIButton ___okButton)
        {
            ___okButton.onRightClick -= OneItemProducePluginLogic.OnOkButtonRightClick;
        }
    }
}