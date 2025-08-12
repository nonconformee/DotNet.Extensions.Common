using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nonconformee.DotNet.Extensions.Comparison;

/// <summary>
/// Provides extension methods for <see cref="IComparer{T}"/>.
/// </summary>
public static class ComparerExtensions
{
    /// <summary>
    /// Creates a composite comparer that performs a secondary comparison using the specified key selector when theprimary comparer results in equality.
    /// </summary>
    /// <remarks>This method is useful for creating multi-level sorting logic, where the primary comparison is
    /// performed using the <paramref name="first"/> comparer, and ties are resolved using the key extracted by
    /// <paramref name="keySelector"/>.</remarks>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    /// <typeparam name="TKey">The type of the key used for the secondary comparison.</typeparam>
    /// <param name="first">The primary comparer to use for the initial comparison. Cannot be <see langword="null"/>.</param>
    /// <param name="keySelector">A function to extract the key from an object for the secondary comparison. Cannot be <see langword="null"/.</param>
    /// <returns>A comparer that first uses the <paramref name="first"/> comparer and, if the comparison results in equality,
    /// uses the key extracted by <paramref name="keySelector"/> for a secondary comparison.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Creates a composite comparer that performs a secondary descending comparison using the specified key selector.
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    /// <typeparam name="TKey">The type of the key used for the secondary comparison.</typeparam>
    /// <param name="first">The initial comparer used for the primary comparison. Cannot be <see langword="null"/.</param>
    /// <param name="keySelector">A function to extract the key for the secondary comparison. Cannot be <see langword="null"/>.</param>
    /// <returns>A comparer that first uses the <paramref name="first"/> comparer for the primary comparison  and then performs a
    /// descending comparison based on the key provided by <paramref name="keySelector"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="first"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Creates a composite comparer that performs a secondary comparison using the specified key selector when theprimary comparer results in equality.
    /// </summary>
    /// <remarks>This method is useful for creating multi-level sorting logic, where the primary comparison is
    /// performed using the <paramref name="first"/> comparer, and ties are resolved using the key extracted by
    /// <paramref name="keySelector"/>.</remarks>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    /// <typeparam name="TKey">The type of the key used for the secondary comparison.</typeparam>
    /// <param name="first">The primary comparer to use for the initial comparison. Cannot be <see langword="null"/>.</param>>
    /// <param name="second">The secondary comparer to use for the initial comparison. Cannot be <see langword="null"/>.</param>
    /// <param name="keySelector">A function to extract the key from an object for the secondary comparison. Cannot be <see langword="null"/.</param>
    /// <returns>A comparer that first uses the <paramref name="first"/> comparer and, if the comparison results in equality,
    /// uses the key extracted by <paramref name="keySelector"/> for a secondary comparison.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="second"/>, or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Creates a composite comparer that performs a secondary descending comparison using the specified key selector when theprimary comparer results in equality.
    /// </summary>
    /// <remarks>This method is useful for creating multi-level sorting logic, where the primary comparison is
    /// performed using the <paramref name="first"/> comparer, and ties are resolved using the key extracted by
    /// <paramref name="keySelector"/>.</remarks>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    /// <typeparam name="TKey">The type of the key used for the secondary comparison.</typeparam>
    /// <param name="first">The primary comparer to use for the initial comparison. Cannot be <see langword="null"/>.</param>>
    /// <param name="second">The secondary comparer to use for the initial comparison. Cannot be <see langword="null"/>.</param>
    /// <param name="keySelector">A function to extract the key from an object for the secondary comparison. Cannot be <see langword="null"/.</param>
    /// <returns>A comparer that first uses the <paramref name="first"/> comparer and, if the comparison results in equality,
    /// uses the key extracted by <paramref name="keySelector"/> for a secondary comparison.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="second"/>, or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
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
