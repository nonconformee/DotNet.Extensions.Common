using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nonconformee.DotNet.Extensions.Comparison;

public static class ComparerExtensions
{
    public static IComparer<T> ThenBy<T, TKey>(this IComparer<T> first, Func<T, TKey> keySelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        return Comparer<T>.Create((x, y) =>
        {
            int result = first.Compare(x, y);
            return result != 0 ? result : Comparer<TKey>.Default.Compare(keySelector(x), keySelector(y));
        });
    }

    public static IComparer<T> ThenByDescending<T, TKey>(this IComparer<T> first, Func<T, TKey> keySelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        return Comparer<T>.Create((x, y) =>
        {
            int result = first.Compare(x, y);
            return result != 0 ? result : Comparer<TKey>.Default.Compare(keySelector(y), keySelector(x));
        });
    }

    public static IComparer<T> ThenBy<T, TKey>(this IComparer<T> first, IComparer<TKey> second, Func<T, TKey> keySelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        return Comparer<T>.Create((x, y) =>
        {
            int result = first.Compare(x, y);
            return result != 0 ? result : second.Compare(keySelector(x), keySelector(y));
        });
    }

    public static IComparer<T> ThenByDescending<T, TKey>(this IComparer<T> first, IComparer<TKey> second, Func<T, TKey> keySelector)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
        return Comparer<T>.Create((x, y) =>
        {
            int result = first.Compare(x, y);
            return result != 0 ? result : second.Compare(keySelector(y), keySelector(x));
        });
    }

    /// <summary>
    /// Returns an <see cref="IComparer{T}"/> that reverses the sort order of the specified comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="comparer">The comparer whose sort order is to be reversed.</param>
    /// <returns>An <see cref="IComparer{T}"/> that sorts in the opposite order of the specified <paramref name="comparer"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
    {
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        return Comparer<T>.Create((x, y) => -comparer.Compare(x, y));
    }
}
