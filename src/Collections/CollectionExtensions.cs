
namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="ICollection{T}"/>.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Performs the specified action on each element of the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each element. Cannot be <see langword="null"/>.</param>
    /// <returns>The original collection for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    public static ICollection<T> ForEach<T>(this ICollection<T> collection, Action<T> action)
    {
        foreach (var element in collection)
        {
            action(element);
        }

        return collection;
    }

    /// <summary>
    /// Adds multiple items to the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The original collection for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static ICollection<T> AddMulti<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }

        return collection;
    }

    /// <summary>
    /// Adds multiple items to the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The original collection for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static ICollection<T> AddMulti<T>(this ICollection<T> collection, params T[] items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }

        return collection;
    }

    /// <summary>
    /// Removes multiple items from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to remove from the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The original collection for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static ICollection<T> RemoveMulti<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Remove(item);
        }

        return collection;
    }

    /// <summary>
    /// Removes multiple items from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to remove from the collection. Cannot be <see langword="null"/>.</param>
    /// <returns>The original collection for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static ICollection<T> RemoveMulti<T>(this ICollection<T> collection, params T[] items)
    {
        foreach (var item in items)
        {
            collection.Remove(item);
        }

        return collection;
    }

    /// <summary>
    /// Empties the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection of elements to empty. Cannot be <see langword="null"/>.</param>
    /// <returns>The original collection for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    /// <remarks><see cref="Empty{T}(ICollection{T})"/> exists to clear the collection and allows further processing by returning its instance (e.g. in fluent chaining of collection operations).</remarks>
    public static ICollection<T> Empty<T>(this ICollection<T> collection)
    {
        collection.Clear();
        return collection;
    }
}
