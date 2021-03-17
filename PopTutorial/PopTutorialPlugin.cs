using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using xiaoye97;

namespace PopTutorial
{

    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class PopTutorialPlugin : BaseUnityPlugin
    {
        public const string ModGuid = "Fuhduki.DSP.PopTutorial";
        public const string ModName = "PopTutorial";
        public const string ModVersion = "0.0.1";

        new internal static ManualLogSource Logger;

        private AudioClip AkariModVoice;

        public void Awake()
        {
            Logger = base.Logger;
            var harmony = new Harmony($"{ModGuid}.patch");
            harmony.PatchAll(typeof(Patch_UIGame_OnUpdate));
        }

        public void Start()
        {
            var ab = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("PopTutorial.tip-101-jp"));
            AkariModVoice = ab.LoadAsset<AudioClip>("tip-101-jp");
            
            LDBTool.PostAddDataAction += AddNewAdvice;
        }

        private void AddNewAdvice()
        {
            Logger.LogInfo("AddNewAdvice Start");

            var advisorProto = new AdvisorTipProto()
            {
                ID = 101,
                Name = "新しいtutorial",
                name = "新しいtutorial".Translate(),
                Voice = null,
                Script = "このチュートリアルを見ている君は Mod を導入していますね？",
                MinLength = 10,
                MaxLength = 100,

            };
            advisorProto.Preload();
            var samples = new float[AkariModVoice.samples * AkariModVoice.samples];
            AkariModVoice.GetData(samples, 0);
            Traverse.Create(advisorProto).Property("auclip").SetValue(AkariModVoice);
            Traverse.Create(advisorProto).Property("samples").SetValue(samples);
            LDBTool.PostAddProto(ProtoType.AdvisorTip, advisorProto);
            Logger.LogInfo("AddNewAdvice End");
        }

    }
}
