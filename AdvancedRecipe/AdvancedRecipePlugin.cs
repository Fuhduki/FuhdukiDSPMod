using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using xiaoye97;

namespace AdvancedRecipe
{
    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class AdvancedRecipePlugin : BaseUnityPlugin
    {
        public const string ModGuid = "Fuhduki.DSP.AdvancedRecipe";
        public const string ModName = "AdvancedRecipe";
        public const string ModVersion = "0.0.1";
        public const string InitializeModVersion = "0.0.0";

        new internal static ManualLogSource Logger;
        private Sprite AdvanceSmelterIcon;

        public void Awake()
        {
            Logger = base.Logger;
        }

        void Start()
        {
#if DEBUG
            foreach(var token in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                Logger.LogInfo("resouce path=" + token);
            }
#endif
            var ab = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("AdvancedRecipe.smelter-advance"));
            AdvanceSmelterIcon = ab.LoadAsset<Sprite>("smelter-advance");

            LDBTool.PreAddDataAction += AddAdvancedSmelterRecipe;
            LDBTool.PreAddDataAction += AddLanguage;
        }

        void AddAdvancedSmelterRecipe()
        {
            var oldRecipe = LDB.recipes.Select(98);
            var newRecipe = oldRecipe.Copy();

            newRecipe.ID = 699;
            newRecipe.Name = "追加レシピ";
            newRecipe.name = "追加レシピ".Translate();
            newRecipe.Description = "なんかすごい";
            newRecipe.description = newRecipe.Description.Translate();
            newRecipe.Items = new int[] { 1001, 1002, 1012, 1120 };
            newRecipe.Results = new int[] { 1204 };
            newRecipe.ItemCounts = new int[] { 4, 4, 2, 10 };
            newRecipe.ResultCounts = new int[] { 1 };
            newRecipe.Explicit = true;
            newRecipe.preTech = LDB.techs.Select(1702);
            newRecipe.TimeSpend = 300;
            newRecipe.GridIndex = 1112;
            newRecipe.SID = newRecipe.GridIndex.ToString();
            newRecipe.sid = newRecipe.GridIndex.ToString().Translate();
            Traverse.Create(newRecipe).Field("_iconSprite").SetValue(AdvanceSmelterIcon);

            var item = LDB.items.Select(1204);
            item.recipes.Add(newRecipe);

            LDBTool.PostAddProto(ProtoType.Recipe, newRecipe);
        }

        void AddLanguage()
        {
            var name = new StringProto()
            {
                ID = 52091,
                Name = "电磁涡轮（高效）",
                name = "电磁涡轮（高效）",
                ZHCN = "电磁涡轮（高效）",
                ENUS = "Electromagnetic turbine (high efficiency)",
            };

            var desc = new StringProto()
            {
                ID = 52092,
                Name = "用金伯利亚矿石可以缩短你的生产线，这样可以利用闲置的甚至说用处不太大的金伯利亚矿石减轻电磁涡轮的生产负担。再也不必为了生产电磁涡轮而感到苦恼。",
                name = "用金伯利亚矿石可以缩短你的生产线，这样可以利用闲置的甚至说用处不太大的金伯利亚矿石减轻电磁涡轮的生产负担。再也不必为了生产电磁涡轮而感到苦恼。",
                ZHCN = "用金伯利亚矿石可以缩短你的生产线，这样可以利用闲置的甚至说用处不太大的金伯利亚矿石减轻电磁涡轮的生产负担。再也不必为了生产电磁涡轮而感到苦恼。",
                ENUS = "Using kimberlite can shorten your production line, which can reduce the production burden of electromagnetic turbine by using idle or even less useful kimberlite. No longer have to worry about making electromagnetic turbines.",
            };
        }
    }
}
