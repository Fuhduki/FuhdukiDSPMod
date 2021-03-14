﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DestructorModeReset
{
    [BepInPlugin(ModGuid, ModName, ModVersion)]
    public class DestructorModeResetPlugin : BaseUnityPlugin
    {
        public const string ModGuid = "Fuhduki.DSP.DestructorModeReset";
        public const string ModName = "DestructorModeReset";
        public const string ModVersion = "1.1.0";
        public const string InitializeModVersion = "0.0.0";

        new internal static ManualLogSource Logger;
        new internal static ConfigFile Config;

        public static PluginConfig PluginConfig;

        public void Awake()
        {
            Logger = base.Logger;
            Config = base.Config;
            Config.SaveOnConfigSet = false;
            PluginConfigReload(true);
            Config.ConfigReloaded += ConfigReloadedEvent;
            var harmony = new Harmony($"{ModGuid}.patch");
            harmony.PatchAll(typeof(Patch_GameMain_Begin));
            harmony.PatchAll(typeof(Patch_PlayerAction_Build_PrepareBuild));
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

        /// <summary>
        /// 設定のリロード
        /// </summary>
        /// <param name="isForceSaveConfig"></param>
        private static void PluginConfigReload(bool isForceSaveConfig = false)
        {
            PluginConfig = GetPluginConfig(Config, out var isSaveConfig);

            if (isForceSaveConfig || isSaveConfig)
                SavePluginConfig();
            Logger.LogInfo("Config reloaded.");
        }

        /// <summary>
        /// 設定の保存
        /// </summary>
        private static void SavePluginConfig()
        {
            var orphanedEntries = typeof(ConfigFile)
                .GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(Config, null) as Dictionary<ConfigDefinition, string>;
            orphanedEntries?.Clear();
            Config.Save();
            Logger.LogInfo("Saved config.");
        }

        /// <summary>
        /// プラグイン設定の取得
        /// </summary>
        /// <param name="config"></param>
        /// <param name="isSaveConfig"></param>
        /// <returns></returns>
        private static PluginConfig GetPluginConfig(ConfigFile config, out bool isSaveConfig)
        {
            isSaveConfig = false;
            var modVersionConfig = config.Bind<string>("Base", "ModVersion", InitializeModVersion, "Don't change.");
            if (modVersionConfig.Value == InitializeModVersion)
            {
                Logger.LogInfo("cleared config.");
                isSaveConfig = true;
                modVersionConfig.SetSerializedValue(ModVersion);
            } else if (modVersionConfig.Value != ModVersion)
            {
                Logger.LogInfo($"add new config : Version ${modVersionConfig.Value} -> {ModVersion}.");
                isSaveConfig = true;
                modVersionConfig.SetSerializedValue(ModVersion);
            }

            var pluginConfig = new PluginConfig(
                config.Bind<string>("Base", "ModVersion", ModVersion, "Don't change.").Value,
                config.Bind<bool>("Base", "EnableDecstructCursorReset", true).Value,
                config.Bind<int>("Base", "DestructCursor", 0, new ConfigDescription("DestructCursor Settable Value : 0, 1")).Value,
                config.Bind<string>("Base", "DestructChain", "false", "DestructChain Settable Value : true, false, keep").Value,
                config.Bind<bool>("Base", "EnableDestructFilterReset", false).Value,
                config.Bind<bool>("Base", "DestructFilterFactory", true).Value,
                config.Bind<bool>("Base", "DestructFilterBelt", true).Value,
                config.Bind<bool>("Base", "DestructFilterInserter", true).Value
                )
                .CheckAndFixConfig(out var fixConfig);
            isSaveConfig |= fixConfig;

#if DEBUG
            Logger.LogInfo($"isSaveConfig : {isSaveConfig}");
            Logger.LogInfo($"Setting: EnableDecstructCursorReset : {pluginConfig.EnableDecstructCursorReset}");
            Logger.LogInfo($"Setting: DestructCursor : {pluginConfig.DestructCursor}");
            Logger.LogInfo($"Setting: DestructChainString : {pluginConfig.DestructChainString}");
            Logger.LogInfo($"Setting: EnableDestructFilterReset : {pluginConfig.EnableDestructFilterReset}");
            Logger.LogInfo($"Setting: DestructFilterFactory : {pluginConfig.DestructFilterFactory}");
            Logger.LogInfo($"Setting: DestructFilterBelt : {pluginConfig.DestructFilterBelt}");
            Logger.LogInfo($"Setting: DestructFilterInserter : {pluginConfig.DestructFilterInserter}");
#endif
            return pluginConfig;
        }
    }
}
