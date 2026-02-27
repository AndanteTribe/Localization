# Localization

[![dotnet-test](https://github.com/AndanteTribe/Localization/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/AndanteTribe/Localization/actions/workflows/dotnet-test.yml)
[![nuget](https://img.shields.io/nuget/v/AndanteTribe.Localization.svg)](https://www.nuget.org/packages/AndanteTribe.Localization/)
[![Releases](https://img.shields.io/github/release/AndanteTribe/Localization.svg)](https://github.com/AndanteTribe/Localization/releases)
[![GitHub license](https://img.shields.io/github/license/AndanteTribe/Localization.svg)](./LICENSE)

[English](README.md) | 日本語

## 概要

**Localization** は、フォーマット対応の多言語化ユーティリティを提供する .NET ライブラリです。高パフォーマンスな文字列フォーマットをサポートします。

主な機能：

1. `LocalizeFormat` — 複合書式文字列（例：`"こんにちは {0}、メッセージが {1:N0} 件あります"`）をパースして保持する型です。書式文字列をあらかじめ解析しておくことで、実行時に繰り返し解析する必要がなくなります。
2. `Localize` — `LocalizeFormat` に型付き引数を適用して最終的なローカライズ済み文字列を生成する静的クラスです。`DefaultInterpolatedStringHandler` とスタックアロケーションを使用した高パフォーマンスな実装です。
3. `LocalizedMemberAttribute` — ローカライズされた値を保持するフィールドやプロパティに付けるアトリビュートです。
4. `Localization.MessagePack` — `LocalizeFormat` の MessagePack シリアライズをサポートするオプションのパッケージです。

## インストール

### NuGet パッケージ

このライブラリには .NET Standard 2.1 以上が必要です。パッケージは NuGet から取得できます。

### .NET CLI

#### コアパッケージ

```ps1
dotnet add package AndanteTribe.Localization
```

#### MessagePack サポート（オプション）

```ps1
dotnet add package AndanteTribe.Localization.MessagePack
```

### パッケージマネージャー

#### コアパッケージ

```ps1
Install-Package AndanteTribe.Localization
```

#### MessagePack サポート（オプション）

```ps1
Install-Package AndanteTribe.Localization.MessagePack
```

## クイックスタート

### パースとフォーマット

```csharp
using Localization;

// ローカライズされた書式文字列を一度だけパースする（アプリ起動時やロケールファイル読み込み時など）
LocalizeFormat greeting = LocalizeFormat.Parse("こんにちは {0}、メッセージが {1:N0} 件あります。");

// 実行時に引数を適用する
string result = Localize.Format(greeting, "アリス", 42);
// result == "こんにちは アリス、メッセージが 42 件あります。"
```

### LocalizedMemberAttribute

`[LocalizedMember]` を使用して、ローカライズされた書式文字列を保持するフィールドやプロパティに注釈を付けます。

```csharp
using Localization;

public class MyLocale
{
    [LocalizedMember]
    public LocalizeFormat Greeting { get; set; } = LocalizeFormat.Parse("こんにちは {0}！");

    [LocalizedMember]
    public LocalizeFormat ItemCount { get; set; } = LocalizeFormat.Parse("アイテムが {0:N0} 個あります。");
}
```

## LocalizeFormat

`LocalizeFormat` は複合書式文字列を解析し、以下のように保持します：

- **Literal** — プレースホルダー間のリテラルテキスト部分。
- **Embed** — プレースホルダー部分。各プレースホルダーは引数インデックスとオプションの書式指定子を持ちます。

```csharp
// 文字列からパース
LocalizeFormat format = LocalizeFormat.Parse("名前: {0}、スコア: {1:N0}");

// 元の書式文字列を復元
string original = format.ToString(); // "名前: {0}、スコア: {1:N0}"
```

## Localize

`Localize` は最大 5 つの型付き引数をサポートするオーバーロードを提供します：

```csharp
Localize.Format(format, arg0);
Localize.Format(format, arg0, arg1);
Localize.Format(format, arg0, arg1, arg2);
Localize.Format(format, arg0, arg1, arg2, arg3);
Localize.Format(format, arg0, arg1, arg2, arg3, arg4);
```

書式文字列内の引数インデックスは順番通りでなくても構いません：

```csharp
LocalizeFormat format = LocalizeFormat.Parse("{1} は {0} です");
string result = Localize.Format(format, "最高", "C#");
// result == "C# は 最高 です"
```

## MessagePack サポート

`AndanteTribe.Localization.MessagePack` パッケージは以下を提供します：

- `LocalizationResolver` — `LocalizeFormat` のフォーマッターを解決する `IFormatterResolver`。
- `LocalizeFormatFormatter` — `LocalizeFormat` のシリアライズ・デシリアライズを行う `IMessagePackFormatter<LocalizeFormat?>`。

```csharp
using MessagePack;
using Localization.MessagePack;

var options = MessagePackSerializerOptions.Standard
    .WithResolver(MessagePack.Resolvers.CompositeResolver.Create(
        LocalizationResolver.Shared,
        MessagePack.Resolvers.StandardResolver.Instance));

byte[] bytes = MessagePackSerializer.Serialize(format, options);
LocalizeFormat? deserialized = MessagePackSerializer.Deserialize<LocalizeFormat>(bytes, options);
```

## ライセンス

このライブラリは MIT ライセンスのもとで公開されています。
