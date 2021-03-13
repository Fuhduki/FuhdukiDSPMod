using System.Collections.Generic;
using System.Linq;

namespace DestructorModeReset
{
    public readonly struct PluginConfig
    {
        public string ModVersion { get; }

        /// <summary>
        /// 削除のモードのリセット有効フラグ
        /// </summary>
        public bool EnableDecstructCursorReset { get; }

        /// <summary>
        /// 削除のモードのリセット値
        /// </summary>
        public int DestructCursor { get; }

        /// <summary>
        /// 削除の連鎖モードの設定値(文字列)
        /// </summary>
        public string DestructChainString { get; }

        /// <summary>
        /// 削除の連鎖モードのリセット値
        /// </summary>
        public bool? DestructChain { get; }

        public PluginConfig(
            string modVersion,
            bool enableDecstructCursorReset,
            int destructCursor,
            string destructChainString)
        {
            ModVersion = modVersion;
            EnableDecstructCursorReset = enableDecstructCursorReset;
            DestructCursor = destructCursor;
            DestructChainString = destructChainString;
            DestructChain = PluginConfigExtention.ParceDestructChainConfig(destructChainString);
        }
    }

    public static class PluginConfigExtention
    {
        // 削除のモードに設定できる値
        private static readonly IEnumerable<int> SettableDestructCursor = new List<int>() { 0, 1 };

        // 削除の連鎖モードの設定保持用の値
        private static readonly string DestructChainKeepString = "keep";

        /// <summary>
        /// 設定のチェックと修正
        /// </summary>
        /// <param name="config"></param>
        /// <param name="fixedPluginConfig"></param>
        /// <returns></returns>
        public static PluginConfig CheckAndFixConfig(this PluginConfig config, out bool fixedPluginConfig)
        {
            fixedPluginConfig = false;
            var destructCursor = config.DestructCursor;
            if (!SettableDestructCursor.Contains(destructCursor))
            {
                fixedPluginConfig = true;
                destructCursor = 0;
            }

            var destructChainString = config.DestructChainString;
            if (destructChainString.ToLower() != DestructChainKeepString && 
                !bool.TryParse(destructChainString, out var _))
            {
                fixedPluginConfig = true;
                destructChainString = "false";
            }

            return new PluginConfig(config.ModVersion, config.EnableDecstructCursorReset, destructCursor, destructChainString);
        }

        /// <summary>
        /// 削除の連鎖モードの文字列をNull許容Bool値に変換
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool? ParceDestructChainConfig(string val)
        {
            if (val.ToLower() == DestructChainKeepString)
                return null;

            if (bool.TryParse(val, out var result))
                return result;
            else
                return false;
        }
    }
}