
using nonconformee.DotNet.Extensions.Comparison;
using nonconformee.DotNet.Extensions.Exceptions;
using System;

namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="IList{T}"/>.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Inserts multiple items to the list at the specified index.
    /// </summary>
    /// <remarks>This method inserts each item from the specified collection into the list at the specified index.
    /// The items are added to the list in the order they appear in the collection. Therefore, the items at and after <paramref name="index"/> are pushed to the end of the list by the number of items in <paramref name="items"/>.</remarks>
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
    /// <remarks>This method inserts each item from the specified collection into the list at the specified index.
    /// The items are added to the list in the order they appear in the collection. Therefore, the items at and after <paramref name="index"/> are pushed to the end of the list by the number of items in <paramref name="items"/>.</remarks>
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
    /// <remarks>This method reverses the elements of the list in place, meaning that the original list is modified.</remarks>
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
    /// elements by copying them to an array, sorting the array, and then copying the sorted elements back to the list.</remarks>
    /// This method sorts the elements of the list in place, meaning that the original list is modified.
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
    /// <remarks>This method shuffles the elements of the list in place, meaning that the original list is modified.</remarks>
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
    /// Randomly mixes the elements of a sequence into the list.
    /// </summary>
    /// <remarks>This method modifies the original list by inserting elements from the specified sequence at random positions.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to be mixed into. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The sequence of items to mix into the list. Cannot be <see langword="null"/>.</param>
    /// <param name="randomizer">An optional <see cref="Random"/> instance to use for generating random numbers. If <see langword="null"/>, a
    /// new instance of <see cref="Random"/> is created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void MixWith<T> (this IList<T> list, IEnumerable<T> items, Random? randomizer = null)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (items is null) throw new ArgumentNullException(nameof(items));

        randomizer ??= new Random();

        foreach (var item in items)
        {
            int index = randomizer.Next(0, list.Count);
            list.Insert(index, item);
        }
    }

    /// <summary>
    /// Randomly mixes the elements of a sequence into the list.
    /// </summary>
    /// <remarks>This method modifies the original list by inserting elements from the specified sequence at random positions.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to be mixed into. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The sequence of items to mix into the list. Cannot be <see langword="null"/>.</param>
    /// <param name="randomizer">An optional <see cref="Random"/> instance to use for generating random numbers. If <see langword="null"/>, a
    /// new instance of <see cref="Random"/> is created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void MixWith<T>(this IList<T> list, Random? randomizer = null, params T[] items)
        => list.Mix((IEnumerable<T>)items, randomizer);

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
    /// Gets the element at the specified index or returns the default value of <typeparamref name="T"/> if the index is out of bounds.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to retrieve the element from. Cannot be <see langword="null"/>.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <param name="defaultValue">The default value to return if the index is out of bounds. The default value is <see langword="default"/>.</param>
    /// <returns>The element at the specified index or <paramref name="defaultValue"/> if the index is out of bounds.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is negative.</exception>"
    public static T? GetOrDefault<T>(this IList<T?> list, int index, T? defaultValue = default)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");

        return index >= 0 && index < list.Count ? list[index] : default;
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

    /// <summary>
    /// Moves an element within the list to the position of another element.
    /// </summary>
    /// <remarks>If <paramref name="item"/> and <paramref name="itemTo"/> are the same, the method
    /// performs no operation.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list containing the element to move. Cannot be <see langword="null"/>.</param>
    /// <param name="item">The element to move. Must be in the list.</param>
    /// <param name="itemTo">The element to which <paramref name="item"/> should be moved. Must be in the list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="item"/> or <paramref name="itemTo"/> is does not exist in the list.</exception>
    public static void Move<T> (this IList<T> list, T item, T itemTo)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        var itemIndex = list.IndexOf(item);
        var itemToIndex = list.IndexOf(itemTo);

        if(itemIndex == -1)
        {
            throw new ArgumentException("Item to move not found in list.", nameof(item));
        }

        if(itemToIndex == -1)
        {
            throw new ArgumentException("Item to move to not found in list.", nameof(itemTo));
        }

        list.Move(itemIndex, itemToIndex);
    }

    /// <summary>
    /// Applies a transformation function to each element in the specified list, replacing each element with the result of the transformation.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list whose elements will be transformed. Cannot be <see langword="null"/>.</param>
    /// <param name="transform">A function that defines the transformation to apply to each element. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="transform"/> is <see langword="null"/>.</exception>
    /// <remarks>This method modifies the original list by replacing each element with the result of the
    /// <paramref name="transform"/> function. The transformation is applied in-place, and the list retains the same
    /// number of elements.</remarks>
    public static void Transform<T> (this IList<T> list, Func<T,T> transform)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (transform is null) throw new ArgumentNullException(nameof(transform));

        for (int i = 0; i < list.Count; i++)
        {
            list[i] = transform(list[i]);
        }
    }

    /// <summary>
    /// Applies a transformation function to each element in the specified list, replacing each element with the result of the transformation.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list whose elements will be transformed. Cannot be <see langword="null"/>.</param>
    /// <param name="transform">A function that defines the transformation to apply to each element. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="transform"/> is <see langword="null"/>.</exception>
    /// <remarks>This method modifies the original list by replacing each element with the result of the
    /// <paramref name="transform"/> function. The transformation is applied in-place, and the list retains the same
    /// number of elements.</remarks>
    public static void Transform<T>(this IList<T> list, Func<T, int, T> transform)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (transform is null) throw new ArgumentNullException(nameof(transform));

        for (int i = 0; i < list.Count; i++)
        {
            list[i] = transform(list[i], i);
        }
    }
}
