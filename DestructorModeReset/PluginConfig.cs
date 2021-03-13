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

        public PluginConfig(
            string modVersion,
            bool enableDecstructCursorReset,
            int destructCursor)
        {
            ModVersion = modVersion;
            EnableDecstructCursorReset = enableDecstructCursorReset;
            DestructCursor = destructCursor;
        }
    }

    public static class PluginConfigExtention
    {
        // 削除のモードに設定できる値
        private static readonly IEnumerable<int> SettableDestructCursor = new List<int>() { 0, 1 };

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

            return new PluginConfig(config.ModVersion, config.EnableDecstructCursorReset, destructCursor);
        }
    }
}