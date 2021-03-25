using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ProduceOnlyOneItem
{
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class OneItemProducePlugin : BaseUnityPlugin
    {
        public const string ModGuid = "Fuhduki.DSP.ProduceOnlyOneItem";
        public const string ModName = "ProduceOnlyOneItem";
        public const string ModVersion = "1.0.0";

        internal new static ManualLogSource Logger;

        public void Awake()
        {
            Logger = base.Logger;
            Config.SaveOnConfigSet = false;
            var harmony = new Harmony($"{ModGuid}.patch");
            harmony.PatchAll(typeof(PatchUiAssemblerWindowOnRegEvent));
            harmony.PatchAll(typeof(PatchUiAssemblerWindowOnUnregEvent));
        }
    }
}
