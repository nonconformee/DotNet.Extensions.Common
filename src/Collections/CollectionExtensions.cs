
using nonconformee.DotNet.Extensions.Exceptions;
using System.Collections;
using System.Collections.ObjectModel;

namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="ICollection{T}"/>.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="collection"/> is <see langword="null"/> or an empty collection, <see langword="false"/> otherwise.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection. Can be <see langword="null"/>.</typeparam>
    /// <param name="collection">The collection to test.</param>
    /// <returns><see langword="true"/> if <paramref name="collection"/> is <see langword="null"/> or an empty collection, <see langword="false"/> otherwise.</returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T>? collection)
        => (collection is null || collection.Count == 0);

    /// <summary>
    /// Returns <see langword="null"/> if <paramref name="collection"/> is <see langword="null"/> or an empty collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to test.</param>
    /// <returns><see langword="null"/> if <paramref name="collection"/> is <see langword="null"/> or an empty collection, <paramref name="collection"/> otherwise.</returns>
    public static ICollection<T>? ToNullIfNullOrEmpty<T>(this ICollection<T>? collection)
        => (collection is null || collection.Count == 0) ? null : collection;

    /// <summary>
    /// Returns an empty collection if <paramref name="collection"/> is <see langword="null"/> or an empty collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to test.</param>
    /// <returns>An empty collection if <paramref name="collection"/> is <see langword="null"/> or an empty collection, <paramref name="collection"/> otherwise.</returns>
    public static ICollection<T> ToEmptyIfNullOrEmpty<T>(this ICollection<T>? collection)
        => collection ?? new List<T>();

    /// <summary>
    /// Adds multiple items to the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static int AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (items is null) throw new ArgumentNullException(nameof(items));

        int added = 0;

        foreach (var item in items)
        {
            collection.Add(item);
            added++;
        }

        return added;
    }

    /// <summary>
    /// Adds multiple items to the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static int AddRange<T>(this ICollection<T> collection, params T[] items)
        => collection.AddRange((IEnumerable<T>)items);

    /// <summary>
    /// Adds an item to the collection if it does not already exist in the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the item will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="item">The item to add to the collection. Can be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the item was added, <see langword="false"/> if it already exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static bool AddIfNotExists<T>(this ICollection<T> collection, T item)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        if (collection.Contains(item))
        {
            return false;
        }

        collection.Add(item);
        return true;
    }

    /// <summary>
    /// Adds an item to the collection if it does not already exist in the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the item will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="item">The item to add to the collection. Can be <see langword="null"/>.</param>
    /// <param name="comparer">A comparer to use for checking existence. Cannot be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the item was added, <see langword="false"/> if it already exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="comparer"/> is <see langword="null"/>.</exception>
    public static bool AddIfNotExists<T>(this ICollection<T> collection, T item, IEqualityComparer<T> comparer)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        if (collection.Contains(item, comparer))
        {
            return false;
        }
        
        collection.Add(item);
        return true;
    }

    /// <summary>
    /// Adds multiple items to the collection if they do not already exist.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static int AddIfNotExistsRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (items is null) throw new ArgumentNullException(nameof(items));

        var addedCount = 0;

        foreach (var item in items)
        {
            if (collection.AddIfNotExists(item))
            {
                addedCount++;
            }
        }

        return addedCount;
    }

    /// <summary>
    /// Adds multiple items to the collection if they do not already exist.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the collection. Cannot be <see langword="null"/>.</param>
    /// <param name="comparer">A comparer to use for checking existence. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="comparer"/> is <see langword="null"/>.</exception>
    public static int AddIfNotExistsRange<T>(this ICollection<T> collection, IEnumerable<T> items, IEqualityComparer<T> comparer)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (items is null) throw new ArgumentNullException(nameof(items));
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        var addedCount = 0;

        foreach (var item in items)
        {
            if (collection.AddIfNotExists(item, comparer))
            {
                addedCount++;
            }
        }

        return addedCount;
    }

    /// <summary>
    /// Adds multiple items to the collection if they do not already exist.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static int AddIfNotExistsRange<T>(this ICollection<T> collection, params T[] items)
        => collection.AddIfNotExistsRange((IEnumerable<T>)items);

    /// <summary>
    /// Adds multiple items to the collection if they do not already exist.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the collection. Cannot be <see langword="null"/>.</param>
    /// <param name="comparer">A comparer to use for checking existence. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="comparer"/> is <see langword="null"/>.</exception>
    public static int AddIfNotExistsRange<T>(this ICollection<T> collection, IEqualityComparer<T> comparer, params T[] items)
        => collection.AddIfNotExistsRange((IEnumerable<T>)items, comparer);

    /// <summary>
    /// Removes multiple items from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to remove from the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of items removed. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static int RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (items is null) throw new ArgumentNullException(nameof(items));

        int removed = 0;

        foreach (var item in items)
        {
            if(collection.Remove(item))
            {
                removed++;
            }
        }

        return removed;
    }

    /// <summary>
    /// Removes multiple items from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to remove from the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of items removed. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static int RemoveRange<T>(this ICollection<T> collection, params T[] items)
        => collection.RemoveRange((IEnumerable<T>)items);

    /// <summary>
    /// Removes all occurences of an item from the collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection. Cannot be <see langword="null"/></param>
    /// <param name="item">The item to remove. Can be <see langword="null"/></param>
    /// <returns>The number of items removed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is Can be <see langword="null"/>.</exception>
    public static int RemoveAll<T>(this ICollection<T> collection, T item)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        var removed = 0;

        while(collection.Remove(item))
        {
            removed++;
        }

        return removed;
    }

    /// <summary>
    /// Removes all occurences of multiple items from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection. Cannot be <see langword="null"/></param>
    /// <param name="items">The items to remove. Cannot be <see langword="null"/></param>
    /// <returns>The number of items removed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static int RemoveAllRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (items is null) throw new ArgumentNullException(nameof(items));

        int removed = 0;

        foreach(var item in items)
        {
            removed += collection.RemoveAll(item);
        }

        return removed;
    }

    /// <summary>
    /// Removes all occurences of multiple items from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection. Cannot be <see langword="null"/></param>
    /// <param name="items">The items to remove. Cannot be <see langword="null"/></param>
    /// <returns>The number of items removed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static int RemoveAllRange<T>(this ICollection<T> collection, params T[] items)
        => collection.RemoveAllRange((IEnumerable<T>)items);

    /// <summary>
    /// Removes all elements matching the given predicate from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection"> The collection from which elements will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="predicate">The predicate to match elements for removal. Cannot be <see langword="null"/>.</param>
    /// <returns>The list of removed elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static List<T> RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var itemsToRemove = collection.Where(predicate).ToList();

        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
        }

        return itemsToRemove;
    }

    /// <summary>
    /// Removes all elements matching the given element using the given equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection"> The collection from which elements will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="element">The element to match for removal. Can be <see langword="null"/>.</param>
    /// <param name="comparer">The equality comparer to use for matching elements. Can be <see langword="null"/>, in which case the default comparer is used.</param>
    /// <returns>The list of removed elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="element"/> is <see langword="null"/>.</exception>
    public static List<T> RemoveWhere<T>(this ICollection<T> collection, T element, IEqualityComparer<T>? comparer = null)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        comparer ??= EqualityComparer<T>.Default;
        var itemsToRemove = collection.Where(x => comparer.Equals(x, element)).ToList();

        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
        }

        return itemsToRemove;
    }

    /// <summary>
    /// Adds an item to the collection if the item is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to add to. Cannot be <see langword="null"/>.</param>
    /// <param name="item">The item to test and add. Can be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the item was not <see langword="null"/> and therefore added to the collection, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static bool AddIfNotNullOrEmpty<T>(this ICollection<T> collection, T? item)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        if (item is null)
        {
            return false;
        }

        collection.Add(item);

        return true;
    }

    /// <summary>
    /// Adds a sequence of items to the collection if the sequence is not <see langword="null"/> and contains elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to add to. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The sequence to test and add. Can be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <remarks>Items in <paramref name="items"/> which are <see langword="null"/> itself will also not be added.</remarks>
    public static int AddIfNotNullOrEmptyRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        if (items is null)
        {
            return 0;
        }

        var enumeratedItems = items.ToList();

        if(enumeratedItems.Count == 0)
        {
            return 0;
        }

        var added = 0;

        foreach (var item in enumeratedItems)
        {
            if(item is null)
            {
                continue;
            }

            collection.Add(item);
            added++;
        }
        
        return added;
    }

    /// <summary>
    /// Adds a sequence of items to the collection if the sequence is not <see langword="null"/> and contains elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to add to. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The sequence to test and add. Can be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/>vis <see langword="null"/>.</exception>
    /// <remarks>Items in <paramref name="items"/> which are <see langword="null"/> itself will also not be added.</remarks>
    public static int AddIfNotNullOrEmptyRange<T>(this ICollection<T> collection, params T[] items)
        => collection.AddIfNotNullOrEmptyRange<T>((IEnumerable<T>) items);

    /// <summary>
    /// Adds a recursive sequence of elements to the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to be recursively added to the collection. Can be <see langword="null"/>.</param>
    /// <param name="addPredicate">An optional predicate to determine whether an item should be added to the collection. If not provided (or is <see langword="null"/> respectively), all items will be added.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <remarks>
    ///     If <paramref name="items"/> is of type <typeparamref name="T"/>, <paramref name="items"/> is added as a single item.
    ///     If <paramref name="items"/> is of type <see cref="IEnumerable{T}"/>, <paramref name="items"/> is enumerated and its items added."/>
    ///     If <paramref name="items"/> is a sequence of of type <see cref="IEnumerable{T}"/>, <paramref name="items"/> is enumerated and each of its sequence is enumerated and its items added."/>
    /// </remarks>
    public static int AddFlattened<T>(this ICollection<T> collection, object items, Func<ICollection<T>, object?, bool>? addPredicate = null)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        if (items is null)
        {
            return 0;
        }

        addPredicate ??= (_, _) => true;

        if (items is T singleInstance)
        {
            if(!addPredicate(collection, singleInstance))
            {
                return 0;
            }

            collection.Add(singleInstance);
            return 1;
        }
        else if(items is IEnumerable<T> enumerable1)
        {
            var added = 0;

            foreach (var item in enumerable1)
            {
                if(!addPredicate(collection, item))
                {
                    continue;
                }

                if(collection.AddIfNotNullOrEmpty(item))
                {
                    added++;
                }
            }

            return added;
        }
        else if(items is IEnumerable enumerable2)
        {
            var added = 0;

            foreach (var item in enumerable2)
            {
                if (!addPredicate(collection, item))
                {
                    continue;
                }

                added += collection.AddFlattened(item);
            }

            return added;
        }
        else
        {
            throw new ArgumentException($"The provided items of type {items.GetType()} cannot be added to the collection.", nameof(items));
        }
    }

    /// <summary>
    /// Adds a recursive sequence of elements to the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The sequence to be recursively added to the collection. Can be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <remarks>
    ///     If <paramref name="items"/> is of type <typeparamref name="T"/>, <paramref name="items"/> is added as a single item.
    ///     If <paramref name="items"/> is of type <see cref="IEnumerable{T}"/>, <paramref name="items"/> is enumerated and its items added."/>
    ///     If <paramref name="items"/> is a sequence of of type <see cref="IEnumerable{T}"/>, <paramref name="items"/> is enumerated and each of its sequence is enumerated and its items added."/>
    /// </remarks>
    public static int AddFlattened<T>(this ICollection<T> collection, params object[] items)
        => collection.AddFlattened<T>((IEnumerable)items);

    /// <summary>
    /// Picks a random item from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to pick a random item from. Cannot be <see langword="null"/>.</param>
    /// <param name="randomizer">The used randomizer. Can be <see langword="null"/> in which case a new instance of <see cref="Random"/> is used.</param>
    /// <returns>The randomly picked item or <see langword="null"/> if the collection empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static T? PickRandom<T>(this ICollection<T> collection, Random? randomizer = null)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        if(collection.Count == 0)
        {
            return default(T?);
        }

        var index = (randomizer ?? new Random()).Next(0, collection.Count);

        return collection.Skip(index).FirstOrDefault();
    }

    /// <summary>
    /// Picks a random item from the collection and removes it.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to pick a random item from. Cannot be <see langword="null"/>.</param>
    /// <param name="randomizer">The used randomizer. Can be <see langword="null"/> in which case a new instance of <see cref="Random"/> is used.</param>
    /// <returns>The randomly picked item or <see langword="null"/> if the collection empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static T? PickRandomAndRemove<T>(this ICollection<T> collection, Random? randomizer = null)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        if (collection.Count == 0)
        {
            return default(T?);
        }

        var index = (randomizer ?? new Random()).Next(0, collection.Count);
        var value = collection.Skip(index).FirstOrDefault();

        if(value is not null)
        {
            collection.Remove(value);
        }

        return value;
    }

    /// <summary>
    /// Divides the elements of the specified collection into smaller lists of a given size.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be divided. Cannot be <see langword="null"/>.</param>
    /// <param name="sliceSize">The maximum number of elements in each sublist. Must be greater than or equal to 1.</param>
    /// <returns>A collection of lists, where each list contains up to <paramref name="sliceSize"/> elements from the original
    /// collection. The last list may contain fewer elements if the total number of elements in the collection is not
    /// evenly divisible by <paramref name="sliceSize"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sliceSize"/> is 0 or less.</exception>
    public static ICollection<List<T>> Slice<T>(this ICollection<T> collection, int sliceSize)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (sliceSize <= 0) throw new ArgumentOutOfRangeException(nameof(sliceSize), "Slice size must be greater than or equal to 1.");
        if (sliceSize > collection.Count) throw new ArgumentOutOfRangeException(nameof(sliceSize), "Slice size must be less than or equal to the collection size.");

        var result = new List<List<T>>();
        var slice = new List<T>();

        foreach (var item in collection)
        {
            slice.Add(item);

            if (slice.Count >= sliceSize)
            {
                result.Add([.. slice]);
                slice.Clear();
            }
        }

        if (slice.Count > 0)
        {
            result.Add([.. slice]);
        }

        return result;
    }

    /// <summary>
    /// Divides the elements of the specified collection into smaller lists based on a split predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be divided. Cannot be <see langword="null"/>.</param>
    /// <param name="splitPredicate">A function to determine whether an element should be the start of a new slice.</param>
    /// <returns>A collection of lists, where each list contains elements from the original collection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static ICollection<List<T>> Slice<T>(this ICollection<T> collection, Func<T, bool> splitPredicate)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (splitPredicate is null) throw new ArgumentNullException(nameof(splitPredicate));

        var result = new List<List<T>>();
        var slice = new List<T>();

        foreach (var item in collection)
        {
            if (splitPredicate(item))
            {
                if (slice.Count > 0)
                {
                    result.AddRange(slice);
                    slice.Clear();
                }
            }

            slice.Add(item);
        }
        if (slice.Count > 0)
        {
            result.AddRange(slice);
        }

        return result;
    }

    /// <summary>
    /// Identifies and groups duplicate elements in the specified collection.
    /// </summary>
    /// <remarks>The method groups elements based on their equality as determined by the specified or default
    /// equality comparer. Each group in the result contains all occurrences of a particular element from the input
    /// collection.</remarks>
    /// <typeparam name="T">The type of elements in the collection. Must be non-nullable.</typeparam>
    /// <param name="collection">The collection to search for duplicate elements. Cannot be <see langword="null"/>.</param>
    /// <param name="includeUniques">A value indicating whether to include unique elements (elements that appear only once) in the result. If <see
    /// langword="true"/>, unique elements will be included as single-item groups; otherwise, only duplicates are
    /// included.</param>
    /// <param name="equalityComparer">An optional equality comparer to use for comparing elements. If <see langword="null"/>, the default equality
    /// comparer for type <typeparamref name="T"/> is used.</param>
    /// <returns>A collection of lists, where each list contains all occurrences of a specific element from the input collection.
    /// If <paramref name="includeUniques"/> is <see langword="false"/>, only lists with more than one element are
    /// included.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static ICollection<List<T>> FindDuplicates<T>(this ICollection<T> collection, bool includeUniques = false, IEqualityComparer<T>? equalityComparer = null)
        where T : notnull
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        var duplicates = new Dictionary<T, List<T>>(collection.Count, equalityComparer ?? EqualityComparer<T>.Default);

        foreach (var item in collection)
        {
            if (item is null)
            {
                continue;
            }

            duplicates.AddIfNecessary(item);
            duplicates[item].Add(item);
        }

        return duplicates
            .Where(x => includeUniques || x.Value.Count > 1)
            .Select(x => x.Value)
            .ToList();
    }

    /// <summary>
    /// Identifies and groups duplicate elements in the specified collection.
    /// </summary>
    /// <remarks>The method groups elements based on their equality as determined by the specified or default
    /// equality comparer after applying the key selector function. Each group in the result contains all occurrences of a particular element from the input
    /// collection.</remarks>
    /// <typeparam name="T">The type of elements in the collection. Must be non-nullable.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by the key selector function. Must be non-nullable.</typeparam>
    /// <param name="collection">The collection to search for duplicate elements. Cannot be <see langword="null"/>.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="includeUniques">A value indicating whether to include unique elements (elements that appear only once) in the result. If <see
    /// langword="true"/>, unique elements will be included as single-item groups; otherwise, only duplicates are
    /// included.</param>
    /// <param name="equalityComparer">An optional equality comparer to use for comparing elements. If <see langword="null"/>, the default equality
    /// comparer for type <typeparamref name="T"/> is used.</param>
    /// <returns>A collection of lists, where each list contains all occurrences of a specific element from the input collection.
    /// If <paramref name="includeUniques"/> is <see langword="false"/>, only lists with more than one element are
    /// included.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
    public static ICollection<List<T>> FindDuplicates<T, TKey>(this ICollection<T> collection, Func<T, TKey> keySelector, bool includeUniques = false, IEqualityComparer<TKey>? equalityComparer = null)
        where TKey : notnull
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        var duplicates = new Dictionary<TKey, List<T>>(equalityComparer ?? EqualityComparer<TKey>.Default);

        foreach (var item in collection)
        {
            if (item is null)
            {
                continue;
            }

            var key = keySelector(item);
            duplicates.AddIfNecessary(key);
            duplicates[key].Add(item);
        }

        return duplicates
            .Where(x => includeUniques || x.Value.Count > 1)
            .Select(x => x.Value)
            .ToList();
    }
}
