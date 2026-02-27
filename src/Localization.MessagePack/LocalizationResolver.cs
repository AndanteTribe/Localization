using MessagePack;
using MessagePack.Formatters;

namespace Localization.MessagePack;

/// <summary>
/// <see cref="Localization"/>の<see cref="IFormatterResolver"/>.
/// </summary>
public sealed class LocalizationResolver : IFormatterResolver
{
    /// <summary>
    /// SShare instance of <see cref="LocalizationResolver"/>.
    /// </summary>
    public static readonly IFormatterResolver Shared = new LocalizationResolver();

    private static readonly RuntimeTypeHandle s_embedArrayTypeHandle = typeof((int, string)[]).TypeHandle;
    private static readonly RuntimeTypeHandle s_embedTypeHandle = typeof((int, string)).TypeHandle;
    private static readonly RuntimeTypeHandle s_localizeFormatTypeHandle = typeof(LocalizeFormat).TypeHandle;

    private LocalizationResolver()
    {
    }

    /// <inheritdoc />
    public IMessagePackFormatter<T>? GetFormatter<T>() => FormatterCache<T>.Formatter;

    private static class FormatterCache<T>
    {
        public static readonly IMessagePackFormatter<T>? Formatter = GetFormatter(typeof(T)) as IMessagePackFormatter<T>;
    }

    private static object? GetFormatter(Type t)
    {
        var typeHandle = t.TypeHandle;
        if (typeHandle.Equals(s_embedArrayTypeHandle))
        {
            return new ArrayFormatter<(int, string)>();
        }
        if (typeHandle.Equals(s_embedTypeHandle))
        {
            return new ValueTupleFormatter<int, string>();
        }
        if (typeHandle.Equals(s_localizeFormatTypeHandle))
        {
            return new LocalizeFormatFormatter();
        }
        return null;
    }
}