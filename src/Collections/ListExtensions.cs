
using nonconformee.DotNet.Extensions.Comparison;
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
    /// Reverses the order of elements in the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list whose elements will be reversed. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static void Reverse<T>(this IList<T> list)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        int left = 0;
        int right = list.Count - 1;

        while (left < right)
        {
            (list[left], list[right]) = (list[right], list[left]);
            left++;
            right--;
        }
    }

    /// <summary>
    /// Sorts the elements of the specified <see cref="IList{T}"/> in ascending or descending order.
    /// </summary>
    /// <remarks>If the provided list is a <see cref="List{T}"/>, the method uses its built-in sorting
    /// mechanism for better performance. For other implementations of <see cref="IList{T}"/>, the method sorts the
    /// elements by copying them to an array, sorting the array,  and then copying the sorted elements back to the
    /// list.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to sort. Cannot be <see langword="null"/>.</param>
    /// <param name="ascending">A value indicating whether to sort the list in ascending order. If <see langword="false"/>, the list is sorted
    /// in descending order. The default is <see langword="true"/>.</param>
    /// <param name="comparer">An optional comparer to use for comparing elements. If <see langword="null"/>, the default comparer for type
    /// <typeparamref name="T"/> is used.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static void Sort<T>(this IList<T> list, bool ascending = true, IComparer<T>? comparer = null)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        if (list.Count <= 1)
        {
            return;
        }

        comparer ??= Comparer<T>.Default;
        
        if (!ascending)
        {
            comparer = comparer.Reverse();
        }

        if (list is List<T> concreteList)
        {
            concreteList.Sort(comparer);
            return;
        }

        var array = list.ToArray();
        Array.Sort(array, comparer);
        
        for (int i = 0; i < array.Length; i++)
        {
            list[i] = array[i];
        }
    }

    /// <summary>
    /// Shuffles the elements of a list in place using Fisher-Yates algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list in which the items will be shuffled. Cannot be <see langword="null"/>.</param>
    /// <param name="randomizer">The used random number generator. A new <see cref="Random"/> instance will be created and used if <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static void Shuffle<T>(this IList<T> list, Random? randomizer = null)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        randomizer ??= new Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = randomizer.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    /// <summary>
    /// Randomly rearranges the elements of the specified list in place.
    /// </summary>
    /// <remarks>If a custom <see cref="Random"/> instance is provided, it will be used for generating random numbers; 
    /// otherwise, a new instance is created internally.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to be shuffled. Cannot be <see langword="null"/>.</param>
    /// <param name="randomizer">An optional <see cref="Random"/> instance to use for generating random numbers.  If <see langword="null"/>, a
    /// new instance of <see cref="Random"/> is created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static void Mix<T> (this IList<T> list, Random? randomizer = null)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        randomizer ??= new Random();

        int n = list.Count;

        for (int i = 0; i < n; i++)
        {
            int j = randomizer.Next(i, n);
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
        if (list is null) throw new ArgumentNullException(nameof(list));

        if (list.Count == 0)
        {
            return default(T?);
        }

        var index = (randomizer ?? new Random()).Next(0, list.Count);

        return list[index];
    }

    /// <summary>
    /// Picks a random item from the list and removes it.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to pick a random item from. Cannot be <see langword="null"/>.</param>
    /// <param name="randomizer">The used randomizer. Can be <see langword="null"/> in which case a new instance of <see cref="Random"/> is used.</param>
    /// <returns>The randomly picked item or <see langword="null"/> if the list empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static T? PickRandomAndRemove<T>(this IList<T> list, Random? randomizer = null)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        if (list.Count == 0)
        {
            return default(T?);
        }

        var index = (randomizer ?? new Random()).Next(0, list.Count);
        var value = list[index];

        list.RemoveAt(index);

        return list[index];
    }

    /// <summary>
    /// Swaps the elements at the specified indices in the list.
    /// </summary>
    /// <remarks>If <paramref name="index1"/> and <paramref name="index2"/> are the same, the method performs
    /// no operation.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list in which the elements will be swapped. Cannot be <see langword="null"/>.</param>
    /// <param name="index1">The zero-based index of the first element to swap. Must be within the bounds of the list.</param>
    /// <param name="index2">The zero-based index of the second element to swap. Must be within the bounds of the list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index1"/> or <paramref name="index2"/> is out of bounds for the list.</exception>
    public static void Swap<T>(this IList<T> list, int index1, int index2)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        list.ThrowIfIndexArgumentIsOutOfRange(index1);
        list.ThrowIfIndexArgumentIsOutOfRange(index2);

        if (index1 == index2)
        {
            return;
        }

        (list[index1], list[index2]) = (list[index2], list[index1]);
    }

    /// <summary>
    /// Swaps the positions of two specified items in the list.
    /// </summary>
    /// <remarks>This method locates the specified items in the list and swaps their positions. If the items
    /// are not found, an exception is thrown. The method uses the <see cref="IList{T}.IndexOf(T)"/> method to locate
    /// the items and assumes the list supports indexing.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list containing the items to swap. Cannot be <see langword="null"/>.</param>
    /// <param name="item1">The first item to swap. Cannot be <see langword="null"/> and must exist in the list.</param>
    /// <param name="item2">The second item to swap. Cannot be <see langword="null"/> and must exist in the list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/>, <paramref name="item1"/>, or <paramref name="item2"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="item1"/> or <paramref name="item2"/> is not found in the list.</exception>
    public static void Swap<T>(this IList<T> list, T item1, T item2)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (item1 is null) throw new ArgumentNullException(nameof(item1));
        if (item2 is null) throw new ArgumentNullException(nameof(item2));

        int index1 = list.IndexOf(item1);
        int index2 = list.IndexOf(item2);
        
        if (index1 < 0 || index2 < 0)
        {
            throw new ArgumentException("Both items must be present in the list.");
        }
        
        list.Swap(index1, index2);
    }

    /// <summary>
    /// Moves an element within the list from the specified old index to the specified new index.
    /// </summary>
    /// <remarks>If <paramref name="oldIndex"/> and <paramref name="newIndex"/> are the same, the method
    /// performs no operation.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list containing the element to move. Cannot be <see langword="null"/>.</param>
    /// <param name="oldIndex">The zero-based index of the element to move. Must be within the bounds of the list.</param>
    /// <param name="newIndex">The zero-based index to which the element should be moved. Must be within the bounds of the list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/> or <paramref name="newIndex"/> is out of bounds for the list.</exception>
    public static void Move<T> (this IList<T> list, int oldIndex, int newIndex)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        list.ThrowIfIndexArgumentIsOutOfRange(oldIndex);
        list.ThrowIfIndexArgumentIsOutOfRange(newIndex);

        if (oldIndex == newIndex)
        {
            return;
        }

        T item = list[oldIndex];
        list.RemoveAt(oldIndex);
        
        if (newIndex > oldIndex)
        {
            newIndex--; // Adjust for the removal
        }
        
        list.Insert(newIndex, item);
    }
}
