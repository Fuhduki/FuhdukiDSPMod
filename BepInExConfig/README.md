# BepInExConfig

## What's this? これは何？

This isn't Mod. BepInEx Tools.
This tool is BepInEx ConfigFile <-> Class Serializer/Deserializer.
You need to do is to create a class for the configuration with the specified attributes.
Then, when you call the extension methods added by the tool, the settings will be saved and loaded according to the attributes set in the class.

これは Mod ではなく、BepInEx のツールです。
このツールは BepInEx の設定ファイルとクラスをシリアライズ/デシリアライズします。
このツールを使うには、指定された属性を持つ設定用のクラスを作成する必要があります。
あとは、ツールによって追加された拡張メソッドを呼び出すと、クラスに設定された属性に応じて設定の保存・読み込みが行われます。

## INSTALL インストール

### If r2modman r2modmanの場合
1. Install BepInEx
2. Install BepInExConfig
3. Add BeinExConfig.dll to the project reference. (for Visual Stuio)

1. BepInEx をインストールします。
2. BepInExConfig をインストールします。
3. BepInExConfig.dll をプロジェクトの参照に追加します(Visual Stuio の場合)

### If Manual 手動の場合
1. Install BepInEx
2. Then copy BeinExConfig.dll into ~~~/steamapps/common/Dyson Sphere Program/BepInEx/plugins
3. Add BeinExConfig.dll to the project reference. (for Visual Stuio)

1. BepInEx をインストールします。
2. ダウンロードした BepInExConfig.dll を ~~~/steamapps/common/Dyson Sphere Program/BepInEx/plugins にコピーします
3. BepInExConfig.dll をプロジェクトの参照に追加します(Visual Stuio の場合)

## Usage 使い方

Create config class.
設定用クラスを作る。
``` TestConfig.cs
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
```

Load config.
設定をロード
```
    public static TestConfig PluginConfig { get; set; }
    public static void PluginConfigLoad()
    {
        this.PluginConfig = Config.LoadConfig<TestConfig>();
        Logger.LogInfo($"TestField : {hoge.TestField}");
        Logger.LogInfo($"TestKey : {hoge.TestKey}");
        Logger.LogInfo($"TestProp : {hoge.TestProp}");
        Logger.LogInfo("Loaded config.");
    }
```

Save config.
設定を保存
```
    public static TestConfig PluginConfig { get; set; }
    public static void PluginConfigLoad()
    {
        this.PluginConfig = new TestConfig()
        {
            TestField = "hugahuga",
            TestKey = KeyCode.KeypadEnter,
            TestProp = 999,
        }

        var orphanedEntries = typeof(ConfigFile)
            .GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(Config, null) as Dictionary<ConfigDefinition, string>;
        orphanedEntries?.Clear();
        Config.SaveConfig(PluginConfig);
        Logger.LogInfo("Saved config.");
    }
```

## CHANGE LOG 変更履歴

### v0.1.0

 - Publish. 公開


