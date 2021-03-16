using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using BepInExConfig;

namespace BepInExCoinfgTest
{
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class TestPlugin : BaseUnityPlugin
    {
        public const string ModGuid = "Fuhduki.DSP.BepInExCoinfgTest";
        public const string ModName = "BepInExCoinfgTest";
        public const string ModVersion = "0.0.1";

        new internal static ManualLogSource Logger;
        new internal static ConfigFile Config;

        public void Awake()
        {
            Logger = base.Logger;
            Config = base.Config;
            Config.SaveOnConfigSet = false;
            Config.ConfigReloaded += ConfigReloadedEvent;
            PluginConfigReload();
            var harmony = new Harmony($"{ModGuid}.patch");
            harmony.PatchAll(typeof(Patch_GameMain_Begin));
        }

        /// <summary>
        /// 設定リロードのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ConfigReloadedEvent(object sender, EventArgs e)
        {
            PluginConfigReload();
        }

        public static void PluginConfigReload()
        {
            var hoge = Config.LoadConfig<TestConfig>();
            Logger.LogInfo($"TestField : {hoge.TestField}");
            Logger.LogInfo($"TestKey : {hoge.TestKey}");
            Logger.LogInfo($"TestProp : {hoge.TestProp}");

            hoge.TestField = "hugahuga";
            hoge.TestKey = KeyCode.KeypadEnter;
            hoge.TestProp = 999;

            var orphanedEntries = typeof(ConfigFile)
                .GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(Config, null) as Dictionary<ConfigDefinition, string>;
            orphanedEntries?.Clear();
            Config.SaveConfig(hoge);
            Logger.LogInfo("Saved config.");

        }

        [BepInExConfig]
        public class TestConfig
        {
            [BepInExConfigMember("Base", "TestProp", 10, order: 99)]
            public int TestProp { get; set; }

            [BepInExConfigMember("Base", "TestField", "hogehoge", "Descripton", order: 0)]
            public string TestField;

            [BepInExConfigMember("Base", "TestKey", KeyCode.Tab, "Descripton", order: 1)]
            public KeyCode TestKey;
        }

    }
}
