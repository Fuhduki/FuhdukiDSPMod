using HarmonyLib;

namespace ProduceOnlyOneItem
{
    [HarmonyPatch(typeof(UIReplicatorWindow), "_OnRegEvent")]
    public class PatchUiAssemblerWindowOnRegEvent
    {
        [HarmonyPostfix]
        public static void Postfix(UIButton ___okButton)
        {
            ___okButton.onRightClick += OneItemProducePluginLogic.OnOkButtonRightClick;
        }
    }
}
