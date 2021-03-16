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

A simple sample is provided here.
ここには簡単なサンプルを記載します。

There is a plugin for testing in [BepInExCoinfgTest](https://github.com/Fuhduki/FuhdukiDSPMod/tree/main/BepInExCoinfgTest), so please refer to it.
[BepInExCoinfgTest](https://github.com/Fuhduki/FuhdukiDSPMod/tree/main/BepInExCoinfgTest) にテスト用のプラグインがあるので参考にしてください。

### Sample Code サンプルコード


Create config class.
設定用クラスを作る。
``` TestConfig.cs
    [BepInExConfig] // 設定用クラスには必須
    public class TestConfig
    {
        // Work - Public Property
        [BepInExConfigMember("Base", "TestProp", 10, order: 99)]
        public int TestProp { get; set; }

        // Work - Public Field
        [BepInExConfigMember("Base", "TestField", "hogehoge", "Descripton", order: 0)]
        public string TestField;

        // Work - Enum
        [BepInExConfigMember("Base", "TestKey", KeyCode.Tab, "Sample Key Code Config", order: 1)]
        public KeyCode TestKey;

        // Not Work - Only public properties or field will work.
        [BepInExConfigMember("Base", "TestProp", 10)]
        private int PrivateProp { get; set; }

        // Not Work  - If you don't add the attribute, it won't work.
        public int NoAttributeProp { get; set; }
    }
```

Load config.
設定をロード
```LoadConfig.cs
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
```SaveConfig.cs
    public static TestConfig PluginConfig { get; set; }
    public static void PluginConfigSave()
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

Saved Config Sample
保存されたコンフィグのサンプル
```config.cfg
## Settings file was created by plugin BepInExCoinfgTest v0.0.1
## Plugin GUID: Fuhduki.DSP.BepInExCoinfgTest

[Base]

## Descripton
# Setting type: String
# Default value: hogehoge
TestField = hugahuga

## Sample Key Code Config
# Setting type: KeyCode
# Default value: Tab
# Acceptable values: None, ...
TestKey = KeypadEnter

# Setting type: Int32
# Default value: 10
TestProp = 999
```

### Simply API Document (簡単な API ドキュメント)

#### Attributes (属性)

- `BepInExConfig`
    - Can only be given to classes. クラスにしか付与できない属性です
    - The configuration class must have a no-argument constructor. (It's OK if you don't write a constructor)

- `BepInExConfigMember(string section, string key, string defaultValue, string description = "", int order = 0)`
    - Can only be given to field or property.
    - Only Work to public field or property.
    - DefaultValue Typse are string, bool, byte, short, ushort, int, uint, long, ulong, float, double, decimal or object(for Enum).
    - The order parameter allows you to determine the order of the configuration items.
        - If nothing is set, the settings will be made in order from the top.
    - If the type of the member is different from the type of the defaultValue, an exception will be thrown when saving or loading.

#### Extended methods (拡張メソッド)

- `T LoadConfig<T>(this ConfigFile config)`
    - Specify the class with the BepInExConfig attribute to T.
    - An instance of the loaded configuration class will be returned.

- `void SaveConfig<T>(this ConfigFile config, T saveTarget)`
    - Pass an instance of the class with the BepInExConfig attribute to the saveTarget parameter.

### 簡単な API ドキュメント

#### 属性

- `BepInExConfig`
    - クラスにしか付与できない属性です
    - 設定クラスには引数なしのコンストラクタが必要です。(コンストラクタを何も書かなければOKです)

- `BepInExConfigMember(string section, string key, string defaultValue, string description = "", int order = 0)`
    - フィールドまたはプロパティにのみ付与することができます。
    - 公開されているフィールドやプロパティにのみ付与することができます。
    - DefaultValueの型は、string、bool、byte、short、ushort、int、uint、long、ulong、float、double、decimal、object(Enumの為)です。
    - orderパラメータは、設定項目の順番を決めることができます。
        - 何も設定しない場合は、上から順に設定されます。
    - メンバーの型が defaultValue の型と異なる場合、保存や読み込みの際に例外が発生します。

#### Extended methods (拡張メソッド)

- `T LoadConfig<T>(this ConfigFile config)`
    - BepInExConfig属性を持つクラスをTに指定します。
    - 読み込まれた設定クラスのインスタンスが返されます。

- `void SaveConfig<T>(this ConfigFile config, T saveTarget)`
    - saveTarget パラメータに BepInExConfig 属性を持つクラスのインスタンスを渡します

## CHANGE LOG 変更履歴

### v0.1.0

 - Publish. 公開