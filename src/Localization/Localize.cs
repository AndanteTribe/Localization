using System.Runtime.CompilerServices;

namespace Localization;

/// <summary>
/// Localization Utility.
/// </summary>
public static class Localize
{
    /// <summary>
    /// Generates a string by applying embedded arguments to the format specified by <see cref="LocalizeFormat"/>.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="arg0"></param>
    /// <typeparam name="T0"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format<T0>(LocalizeFormat format, T0 arg0)
    {
        var sb = FormatHelper.TryGetDefaultLength(format._literalLength, format._embed.Length, out var l)
            ? new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length, null, stackalloc char[l])
            : new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length);

        sb.AppendLiteral(format.Literal[0]);
        var (_, fmt) = format.Embed[0];
        sb.AppendFormatted(arg0, fmt);

        if (format.Literal.Length > format.Embed.Length)
        {
            sb.AppendLiteral(format.Literal[1]);
        }
        return sb.ToStringAndClear();
    }

    /// <summary>
    /// Generates a string by applying embedded arguments to the format specified by <see cref="LocalizeFormat"/>.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format<T0, T1>(LocalizeFormat format, T0 arg0, T1 arg1)
    {
        var sb = FormatHelper.TryGetDefaultLength(format._literalLength, format._embed.Length, out var l)
            ? new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length, null, stackalloc char[l])
            : new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length);

        for (var i = 0; i < format.Embed.Length; i++)
        {
            sb.AppendLiteral(format.Literal[i]);
            var (index, fmt) = format.Embed[i];
            switch (index)
            {
                case 0:
                    sb.AppendFormatted(arg0, fmt);
                    break;
                case 1:
                    sb.AppendFormatted(arg1, fmt);
                    break;
            }
        }

        if (format.Literal.Length > format.Embed.Length)
        {
            sb.AppendLiteral(format.Literal[2]);
        }
        return sb.ToStringAndClear();
    }

    /// <summary>
    /// Generates a string by applying embedded arguments to the format specified by <see cref="LocalizeFormat"/>.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format<T0, T1, T2>(LocalizeFormat format, T0 arg0, T1 arg1, T2 arg2)
    {
        var sb = FormatHelper.TryGetDefaultLength(format._literalLength, format._embed.Length, out var l)
            ? new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length, null, stackalloc char[l])
            : new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length);

        for (var i = 0; i < format.Embed.Length; i++)
        {
            sb.AppendLiteral(format.Literal[i]);
            var (index, fmt) = format.Embed[i];
            switch (index)
            {
                case 0:
                    sb.AppendFormatted(arg0, fmt);
                    break;
                case 1:
                    sb.AppendFormatted(arg1, fmt);
                    break;
                case 2:
                    sb.AppendFormatted(arg2, fmt);
                    break;
            }
        }

        if (format.Literal.Length > format.Embed.Length)
        {
            sb.AppendLiteral(format.Literal[3]);
        }
        return sb.ToStringAndClear();
    }

    /// <summary>
    /// Generates a string by applying embedded arguments to the format specified by <see cref="LocalizeFormat"/>.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <param name="arg3"></param>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format<T0, T1, T2, T3>(LocalizeFormat format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        var sb = FormatHelper.TryGetDefaultLength(format._literalLength, format._embed.Length, out var l)
            ? new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length, null, stackalloc char[l])
            : new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length);

        for (var i = 0; i < format.Embed.Length; i++)
        {
            sb.AppendLiteral(format.Literal[i]);
            var (index, fmt) = format.Embed[i];
            switch (index)
            {
                case 0:
                    sb.AppendFormatted(arg0, fmt);
                    break;
                case 1:
                    sb.AppendFormatted(arg1, fmt);
                    break;
                case 2:
                    sb.AppendFormatted(arg2, fmt);
                    break;
                case 3:
                    sb.AppendFormatted(arg3, fmt);
                    break;
            }
        }

        if (format.Literal.Length > format.Embed.Length)
        {
            sb.AppendLiteral(format.Literal[4]);
        }
        return sb.ToStringAndClear();
    }

    /// <summary>
    /// Generates a string by applying embedded arguments to the format specified by <see cref="LocalizeFormat"/>.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <param name="arg3"></param>
    /// <param name="arg4"></param>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format<T0, T1, T2, T3, T4>(LocalizeFormat format, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        var sb = FormatHelper.TryGetDefaultLength(format._literalLength, format._embed.Length, out var l)
            ? new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length, null, stackalloc char[l])
            : new DefaultInterpolatedStringHandler(format._literalLength, format._embed.Length);

        for (var i = 0; i < format.Embed.Length; i++)
        {
            sb.AppendLiteral(format.Literal[i]);
            var (index, fmt) = format.Embed[i];
            switch (index)
            {
                case 0:
                    sb.AppendFormatted(arg0, fmt);
                    break;
                case 1:
                    sb.AppendFormatted(arg1, fmt);
                    break;
                case 2:
                    sb.AppendFormatted(arg2, fmt);
                    break;
                case 3:
                    sb.AppendFormatted(arg3, fmt);
                    break;
                case 4:
                    sb.AppendFormatted(arg4, fmt);
                    break;
            }
        }

        if (format.Literal.Length > format.Embed.Length)
        {
            sb.AppendLiteral(format.Literal[5]);
        }
        return sb.ToStringAndClear();
    }
}