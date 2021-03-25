using HarmonyLib;

namespace ProduceOnlyOneItem
{
    public static class OneItemProducePluginLogic
    {
        public static void OnOkButtonRightClick(int whatever)
        {
#if DEBUG
            OneItemProducePlugin.Logger.LogInfo(("OnOkButtonRightClick"));
#endif
            var replicatorWindow = UIRoot.instance.uiGame.replicator;
            var selectRecipe = Traverse.Create(replicatorWindow).Field("selectedRecipe").GetValue<RecipeProto>();
            var mechaForge = Traverse.Create(replicatorWindow).Field("mechaForge").GetValue<MechaForge>();

            if (selectRecipe == null)
                return;

            var taskCount = mechaForge.PredictTaskCount(selectRecipe.ID, 99);
            if (taskCount == 0)
            {
                UIRealtimeTip.Popup("材料不足".Translate(), true, 0);
                return;
            }

            if (mechaForge.AddTask(selectRecipe.ID, 1) == null)
                UIRealtimeTip.Popup("材料不足".Translate(), true, 0);
            else
                GameMain.history.RegFeatureKey(1000104);
        }
    }
}