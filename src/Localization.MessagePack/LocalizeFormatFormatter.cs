using MessagePack;
using MessagePack.Formatters;
using IMessagePackFormatter = MessagePack.Formatters.IMessagePackFormatter<Localization.LocalizeFormat?>;

namespace Localization.MessagePack;

/// <summary>
/// <see cref="LocalizeFormat"/>の<see cref="IMessagePackFormatter{T}"/>.
/// </summary>
public class LocalizeFormatFormatter : IMessagePackFormatter
{
    /// <inheritdoc />
    void IMessagePackFormatter.Serialize(ref MessagePackWriter writer, LocalizeFormat? value, MessagePackSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();
            return;
        }

        writer.WriteArrayHeader(3);
        options.Resolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value._literal, options);
        options.Resolver.GetFormatterWithVerify<(int index, string format)[]>().Serialize(ref writer, value._embed, options);
        writer.Write(value._literalLength);
    }

    /// <inheritdoc />
    LocalizeFormat? IMessagePackFormatter.Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.TryReadNil())
        {
            return null;
        }

        options.Security.DepthStep(ref reader);
        var count = reader.ReadArrayHeader();
        if (count != 3)
        {
            throw new MessagePackSerializationException("Invalid array length.");
        }

        var literal = options.Resolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, options);
        var embed = options.Resolver.GetFormatterWithVerify<(int index, string format)[]>().Deserialize(ref reader, options);
        var literalLength = reader.ReadInt32();

        var result = new LocalizeFormat(literal, embed, literalLength);
        reader.Depth--;
        return result;
    }
}