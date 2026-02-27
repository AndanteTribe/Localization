using System.Buffers;
using System.Runtime.CompilerServices;

namespace Localization.Internal;

internal static class ArrayPoolExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Grow<T>(this ArrayPool<T> pool, ref T[] array, int minimumLength)
    {
        if (array.Length < minimumLength)
        {
            var newArray = pool.Rent(minimumLength);
            if (array.Length > 0)
            {
                Array.Copy(array, newArray, array.Length);
                pool.Return(array, true);
            }
            array = newArray;
        }
    }
}