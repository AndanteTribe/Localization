using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Localization;

/// <summary>
/// ローカライズ用のフォーマット形式キャッシュ.
/// </summary>
public record LocalizeFormat
{
    internal readonly string[] _literal;

    internal readonly (int index, string format)[] _embed;

    internal readonly int _literalLength;

    /// <summary>
    /// リテラル部分の配列.
    /// </summary>
    public ReadOnlySpan<string> Literal => _literal.AsSpan();

    /// <summary>
    /// 埋め込み部分の配列.
    /// </summary>
    public ReadOnlySpan<(int index, string format)> Embed => _embed.AsSpan();

    /// <summary>
    /// Initialize a new instance of <see cref="LocalizeFormat"/>.
    /// </summary>
    /// <param name="literal"></param>
    /// <param name="embed"></param>
    /// <param name="literalLength"></param>
    internal LocalizeFormat(string[] literal, (int index, string format)[] embed, int literalLength)
    {
        _literal = literal;
        _embed = embed;
        _literalLength = literalLength;
    }

    /// <summary>
    /// フォーマット文字列を解析して<see cref="LocalizeFormat"/>を生成します.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocalizeFormat Parse([StringSyntax(StringSyntaxAttribute.CompositeFormat)] in string format) => Parse(format.AsSpan());

    /// <summary>
    /// フォーマット文字列を解析して<see cref="LocalizeFormat"/>を生成します.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static LocalizeFormat Parse([StringSyntax(StringSyntaxAttribute.CompositeFormat)] in ReadOnlySpan<char> format)
    {
        var (literal, embed) = FormatHelper.AnalyzeFormat(format);
        var literalLength = 0;
        foreach (var l in literal.AsSpan())
        {
            literalLength += l.Length;
        }
        return new LocalizeFormat(literal, embed, literalLength);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (_embed.Length == 0)
        {
            return _literal[0];
        }

        var sb = FormatHelper.TryGetDefaultLength(_literalLength, _embed.Length, out var l)
            ? new DefaultInterpolatedStringHandler(_literalLength, _embed.Length, null, stackalloc char[l])
            : new DefaultInterpolatedStringHandler(_literalLength, _embed.Length);

        for (var i = 0; i < _embed.Length; i++)
        {
            sb.AppendLiteral(_literal[i]);
            sb.AppendLiteral("{");
            sb.AppendFormatted(_embed[i].index);
            if (!string.IsNullOrEmpty(_embed[i].format))
            {
                sb.AppendLiteral(":");
                sb.AppendLiteral(_embed[i].format);
            }
            sb.AppendLiteral("}");
        }

        if (_literal.Length > _embed.Length)
        {
            sb.AppendLiteral(_literal[^1]);
        }
        return sb.ToStringAndClear();
    }
}