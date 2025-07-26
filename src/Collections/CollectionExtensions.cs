
using System.Collections;

namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="ICollection{T}"/>.
/// </summary>
public static class CollectionExtensions
{
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
    /// <returns>The number of elements removed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static int RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var itemsToRemove = collection.Where(predicate).ToList();

        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
        }

        return itemsToRemove.Count;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="collection"/> is <see langword="null"/> or an empty collection, <see langword="false"/> otherwise.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection. Can be <see langword="null"/>.</typeparam>
    /// <param name="collection">The collection to test.</param>
    /// <returns><see langword="true"/> if <paramref name="collection"/> is <see langword="null"/> or an empty collection, <see langword="false"/> otherwise.</returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        => (collection is null || collection.Count == 0);

    /// <summary>
    /// Returns <see langword="null"/> if <paramref name="collection"/> is <see langword="null"/> or an empty collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection. Can be <see langword="null"/>.</typeparam>
    /// <param name="collection">The collection to test.</param>
    /// <returns><see langword="null"/> if <paramref name="collection"/> is <see langword="null"/> or an empty collection, <paramref name="collection"/> otherwise.</returns>
    public static ICollection<T>? ToNullIfNullOrEmpty<T>(this ICollection<T> collection)
        => (collection is null || collection.Count == 0) ? null : collection;

    /// <summary>
    /// Adds a sequence of items if it is not <see langword="null"/> and contains elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to add to. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The sequence to test and add. Can be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <remarks>Items which are <see langword="null"/> itself will also not be added.</remarks>
    public static int AddIfNotNullOrEmpty<T>(this ICollection<T> collection, IEnumerable<T> items)
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
    /// Adds a sequence of items if it is not <see langword="null"/> and contains elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to add to. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The sequence to test and add. Can be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/>vis <see langword="null"/>.</exception>
    /// <remarks>Items which are <see langword="null"/> itself will also not be added.</remarks>
    public static int AddIfNotNullOrEmpty<T>(this ICollection<T> collection, params T[] items)
        => collection.AddIfNotNullOrEmpty<T>((IEnumerable<T>) items);

    /// <summary>
    /// Adds a recursive sequence of elements to the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to be recursively added to the collection. Can be <see langword="null"/>.</param>
    /// <returns>The number of items added. Could be zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <remarks>
    ///     If <paramref name="items"/> is of type <typeparamref name="T"/>, <paramref name="items"/> is added as a single item.
    ///     If <paramref name="items"/> is of type <see cref="IEnumerable{T}"/>, <paramref name="items"/> is enumerated and its items added."/>
    ///     If <paramref name="items"/> is a sequence of of type <see cref="IEnumerable{T}"/>, <paramref name="items"/> is enumerated and each of its sequence is enumerated and its items added."/>
    /// </remarks>
    public static int AddFlattened<T>(this ICollection<T> collection, object items)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));

        if (items is null)
        {
            return 0;
        }

        if(items is T singleInstance)
        {
            collection.Add(singleInstance);
            return 1;
        }
        else if(items is IEnumerable<T> enumerable1)
        {
            return collection.AddIfNotNullOrEmpty(enumerable1);
        }
        else if(items is IEnumerable enumerable2)
        {
            var added = 0;

            foreach (var item in enumerable2)
            {
                added += collection.AddFlattened(item);
            }

            return added;
        }

        return 0;
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
}
