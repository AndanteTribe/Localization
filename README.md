# Localization

[![dotnet-test](https://github.com/AndanteTribe/Localization/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/AndanteTribe/Localization/actions/workflows/dotnet-test.yml)
[![nuget](https://img.shields.io/nuget/v/AndanteTribe.Localization.svg)](https://www.nuget.org/packages/AndanteTribe.Localization/)
[![Releases](https://img.shields.io/github/release/AndanteTribe/Localization.svg)](https://github.com/AndanteTribe/Localization/releases)
[![GitHub license](https://img.shields.io/github/license/AndanteTribe/Localization.svg)](./LICENSE)

English | [日本語](README_JA.md)

## Overview

**Localization** is a .NET library that provides format-aware localization utilities with high-performance string formatting.

It provides the following:

1. `LocalizeFormat` — A parsed representation of a composite format string (e.g., `"Hello {0}, you have {1:N0} messages"`). Pre-parsing the format string separates the literal parts from the placeholders, avoiding repeated parsing at runtime.
2. `Localize` — A static class that applies typed arguments to a `LocalizeFormat`, producing the final localized string. The implementation uses `DefaultInterpolatedStringHandler` with stack allocation for high performance.
3. `LocalizedMemberAttribute` — An attribute to annotate fields or properties that hold localized values.
4. `Localization.MessagePack` — An optional package that provides MessagePack serialization support for `LocalizeFormat`, enabling format strings to be stored and loaded via MessagePack.

## Installation

### NuGet Packages

This library requires .NET Standard 2.1 or higher. The packages can be obtained from NuGet.

### .NET CLI

#### Core package

```ps1
dotnet add package AndanteTribe.Localization
```

#### MessagePack support (optional)

```ps1
dotnet add package AndanteTribe.Localization.MessagePack
```

### Package Manager

#### Core package

```ps1
Install-Package AndanteTribe.Localization
```

#### MessagePack support (optional)

```ps1
Install-Package AndanteTribe.Localization.MessagePack
```

## Quick Start

### Parsing and formatting

```csharp
using Localization;

// Parse a localized format string once (e.g., at application startup or when loading a locale file)
LocalizeFormat greeting = LocalizeFormat.Parse("Hello {0}, you have {1:N0} messages.");

// Apply arguments at runtime
string result = Localize.Format(greeting, "Alice", 4321);
// result == "Hello Alice, you have 4,321 messages."
```

### LocalizedMemberAttribute

Use `[LocalizedMember]` to annotate fields or properties that hold localized format strings.

```csharp
using Localization;

public class MyLocale
{
    [LocalizedMember]
    public string Greeting { get; set; } = "Hello {0}!";

    [LocalizedMember]
    public string ItemCount { get; set; } = "You have {0:N0} items.";
}
```

## LocalizeFormat

`LocalizeFormat` parses a composite format string and stores it as:

- **Literal** — The literal text parts between the placeholders.
- **Embed** — The placeholder parts, each with an argument index and an optional format specifier.

```csharp
// Parse from a string
LocalizeFormat format = LocalizeFormat.Parse("Name: {0}, Score: {1:N0}");

// Reconstruct the original format string
string original = format.ToString(); // "Name: {0}, Score: {1:N0}"
```

## Localize

`Localize` provides overloads that support up to five typed arguments:

```csharp
Localize.Format(format, arg0);
Localize.Format(format, arg0, arg1);
Localize.Format(format, arg0, arg1, arg2);
Localize.Format(format, arg0, arg1, arg2, arg3);
Localize.Format(format, arg0, arg1, arg2, arg3, arg4);
```

Argument indices in the format string do not have to be in order:

```csharp
LocalizeFormat format = LocalizeFormat.Parse("{1} is {0}");
string result = Localize.Format(format, "awesome", "C#");
// result == "C# is awesome"
```

## MessagePack Support

The `AndanteTribe.Localization.MessagePack` package provides:

- `LocalizationResolver` — An `IFormatterResolver` that resolves formatters for `LocalizeFormat`.
- `LocalizeFormatFormatter` — An `IMessagePackFormatter<LocalizeFormat?>` for serializing and deserializing `LocalizeFormat` instances.

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

## License

This library is released under the MIT license.
