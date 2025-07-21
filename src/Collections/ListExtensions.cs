
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
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index for <paramref name="list"/>.</exception>
    public static void InsertRange<T>(this IList<T> list, int index, IEnumerable<T> items)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (items is null) throw new ArgumentNullException(nameof(items));
        list.ThrowIfIndexArgumentIsOutOfRange(index + 1);

        foreach (var item in items)
        {
            list.Insert(index++, item);
        }
    }

    /// <summary>
    /// Inserts multiple items to the list at the specified index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to which the items will be inserted. Cannot be <see langword="null"/>.</param>
    /// <param name="index">Zero-based index at which the items will be inserted.</param>
    /// <param name="items">The items to insert into the list. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index for <paramref name="list"/>.</exception>
    public static void InsertRange<T>(this IList<T> list, int index, params T[] items)
        => list.InsertRange(index, (IEnumerable<T>)items);

    /// <summary>
    /// Removes multiple items from the list at the specified index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="index">Zero-based index at which the items will be removed.</param>
    /// <param name="count">The number of items to remove starting from the specified index.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static void RemoveAtRange<T>(this IList<T> list, int index, int count)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        list.ThrowIfIndexArgumentIsOutOfRange(index, count);

        for (int i = 0; i < count; i++)
        {
            list.RemoveAt(index);
        }
    }

    /// <summary>
    /// Shuffles the elements of a list in place using Fisher-Yates algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list in which the items will be shuffled. Cannot be <see langword="null"/>.</param>
    /// <param name="rng">The used random number generator. A new <see cref="Random"/> instance will be created and used if <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static void Shuffle<T>(this IList<T> list, Random? rng = null)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));

        rng ??= new Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    /// <summary>
    /// Picks a random item from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to pick a random item from. Cannot be <see langword="null"/>.</param>
    /// <param name="randomizer">The used randomizer. Can be <see langword="null"/> in which case a new instance of <see cref="Random"/> is used.</param>
    /// <returns>The randomly picked item or <see langword="null"/> if the list empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static T? PickRandom<T>(this IList<T> list, Random? randomizer = null)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));

        if (list.Count == 0)
        {
            return default(T?);
        }

        var index = (randomizer ?? new Random()).Next(0, list.Count);

        return list[index];
    }
}
