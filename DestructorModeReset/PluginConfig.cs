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

        /// <summary>
        /// 削除のフィルターリセット有効フラグ
        /// </summary>
        public bool EnableDestructFilterReset { get; }

        /// <summary>
        /// 削除の施設フィルターのリセット値
        /// </summary>
        public bool DestructFilterFactory { get; }

        /// <summary>
        /// 削除のコンベアベルトフィルターのリセット値
        /// </summary>
        public bool DestructFilterBelt { get; }

        /// <summary>
        /// 削除のソーターフィルターのリセット値
        /// </summary>
        public bool DestructFilterInserter { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="modVersion"></param>
        /// <param name="enableDecstructCursorReset"></param>
        /// <param name="destructCursor"></param>
        /// <param name="destructChainString"></param>
        public PluginConfig(
            string modVersion,
            bool enableDecstructCursorReset,
            int destructCursor,
            string destructChainString,
            bool enableDestructFilterReset,
            bool destructFilterFactory,
            bool destructFilterBelt,
            bool destructFilterInserter)
        {
            ModVersion = modVersion;
            EnableDecstructCursorReset = enableDecstructCursorReset;
            DestructCursor = destructCursor;
            DestructChainString = destructChainString;
            DestructChain = PluginConfigExtention.ParceDestructChainConfig(destructChainString);

            EnableDestructFilterReset = enableDestructFilterReset;
            DestructFilterFactory = destructFilterFactory;
            DestructFilterBelt = destructFilterBelt;
            DestructFilterInserter = destructFilterInserter;
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

            var destructFilterFactory = config.DestructFilterFactory;
            var destructFilterInserter = config.DestructFilterInserter;
            if(destructFilterFactory && !destructFilterInserter)
            {
                fixedPluginConfig = true;
                destructFilterFactory = true;
                destructFilterInserter = true;
            }

            return new PluginConfig(
                config.ModVersion,
                config.EnableDecstructCursorReset,
                destructCursor,
                destructChainString,
                config.EnableDestructFilterReset,
                destructFilterFactory,
                config.DestructFilterBelt,
                destructFilterInserter);
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