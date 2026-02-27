using System.Runtime.CompilerServices;

namespace Localization.Internal;

internal static class CharExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiDigit(in this char c) => (uint)(c - '0') <= (uint)('9' - '0');
}