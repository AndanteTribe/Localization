using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Localization.Internal;

namespace Localization;

/// <summary>
/// String Formatting Helper.
/// </summary>
public static class FormatHelper
{
    /// <summary>
    /// Parses the format string.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (string[] literal, (int index, string format)[] embed) AnalyzeFormat(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)] in string format) => AnalyzeFormat(format.AsSpan());

    /// <summary>
    /// Parses the format string.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (string[] literal, (int index, string format)[] embed) AnalyzeFormat(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)] in ReadOnlySpan<char> format)
    {
        var literalsBuffer = Array.Empty<string>();
        var indicesBuffer = Array.Empty<(int index, string format)>();
        var (literalCount, indexCount) = (0, 0);

        try
        {
            var pos = 0;
            while (pos < format.Length)
            {
                var remainder = format[pos..];
                var countUntilNextBrace = remainder.IndexOfAny('{', '}');
                if (countUntilNextBrace < 0)
                {
                    ArrayPool<string>.Shared.Grow(ref literalsBuffer, literalCount + 1);
                    literalsBuffer[literalCount++] = remainder.ToString();
                    break;
                }

                if (remainder[countUntilNextBrace] == '}')
                {
                    // 開き中括弧がない.
                    throw new FormatException($"There is no opening curly brace. format={format}");
                }

                ArrayPool<string>.Shared.Grow(ref literalsBuffer, literalCount + 1);
                literalsBuffer[literalCount++] = remainder[..countUntilNextBrace].ToString();

                pos += countUntilNextBrace;
                pos++;

                var endBrace = format[pos..].IndexOf('}');
                if (endBrace < 0)
                {
                    // 閉じ中括弧がない.
                    throw new FormatException($"There is no closing curly brace. format={format}");
                }

                var inside = format.Slice(pos, endBrace);
                var split = inside.IndexOf(':');
                var i = split < 0 ? inside : inside[..split];
                var f = split < 0 ? ReadOnlySpan<char>.Empty : inside[(split + 1)..];

                foreach (var c in i)
                {
                    if (!c.IsAsciiDigit())
                    {
                        // インデックス '{i}' が整数ではない.
                        throw new FormatException($"The index '{i}' is not an integer. format={format}");
                    }
                }

                ArrayPool<(int index, string format)>.Shared.Grow(ref indicesBuffer, indexCount + 1);
                indicesBuffer[indexCount++] = (int.Parse(i), f.ToString());
                pos += endBrace + 1;
            }

            return (literalCount == 0 ? [] : literalsBuffer.AsSpan(0, literalCount).ToArray(), indexCount == 0 ? [] : indicesBuffer.AsSpan(0, indexCount).ToArray());
        }
        finally
        {
            ArrayPool<string>.Shared.Return(literalsBuffer);
            ArrayPool<(int index, string format)>.Shared.Return(indicesBuffer);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryGetDefaultLength(int literalLength, int formattedCount, out int defaultLength)
    {
        // Same as stack-allocation size used today by string.Format.
        const int minimumArrayPoolLength = 256;

        // これもstring.Formatの実装に合わせている.
        const int guessedLengthPerHole = 11;

        defaultLength = literalLength + formattedCount * guessedLengthPerHole;
        return defaultLength <= minimumArrayPoolLength;
    }
}