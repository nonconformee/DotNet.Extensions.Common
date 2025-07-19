
using nonconformee.DotNet.Extensions.Exceptions;

namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="IList{T}"/>.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Inserts multiple items to the list at the specified index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to which the items will be inserted. Cannot be <see langword="null"/>.</param>
    /// <param name="index">Zero-based index at which the items will be inserted.</param>
    /// <param name="items">The items to insert into the list. Cannot be <see langword="null"/>.</param>
    /// <returns>The original list for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index for <paramref name="list"/>.</exception>
    public static IList<T> InsertMulti<T>(this IList<T> list, int index, IEnumerable<T> items)
    {
        list.ThrowIfIndexOutOfRange(index + 1);

        foreach (var item in items)
        {
            list.Insert(index++, item);
        }

        return list;
    }

    /// <summary>
    /// Inserts multiple items to the list at the specified index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to which the items will be inserted. Cannot be <see langword="null"/>.</param>
    /// <param name="index">Zero-based index at which the items will be inserted.</param>
    /// <param name="items">The items to insert into the list. Cannot be <see langword="null"/>.</param>
    /// <returns>The original list for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index for <paramref name="list"/>.</exception>
    public static IList<T> InsertMulti<T>(this IList<T> list, int index, params T[] items)
    {
        list.ThrowIfIndexOutOfRange(index + 1);

        foreach (var item in items)
        {
            list.Insert(index++, item);
        }

        return list;
    }

    /// <summary>
    /// Removes multiple items from the list at the specified index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="index">Zero-based index at which the items will be removed.</param>
    /// <param name="count">The number of items to remove starting from the specified index.</param>
    /// <returns>The original list for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static IList<T> RemoveAtMulti<T>(this IList<T> list, int index, int count)
    {
        list.ThrowIfIndexOutOfRange(index, count);

        for (int i = 0; i < count; i++)
        {
            list.RemoveAt(index);
        }

        return list;
    }
}
