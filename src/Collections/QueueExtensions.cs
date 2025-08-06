
namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="Queue{T}" />.
/// </summary>
public static class QueueExtensions
{
    /// <summary>
    /// Adds a range of items to the queue.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="queue"> The queue to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items"> The items to add to the queue. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="queue"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        if (queue is null) throw new ArgumentNullException(nameof(queue));
        if (items is null) throw new ArgumentNullException(nameof(items));
        
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }

    /// <summary>
    /// Adds a range of items to the queue.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="queue"> The queue to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items"> The items to add to the queue. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="queue"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void EnqueueRange<T>(this Queue<T> queue, params T[] items)
        => queue.EnqueueRange((IEnumerable<T>)items);

    /// <summary>
    /// Dequeues all items from the queue.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="queue"> The queue to which the items will be dequeued. Cannot be <see langword="null"/>.</param>
    /// <returns>A list containing all items that were in the queue. If the queue was empty, an empty list is returned.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="queue"/> is <see langword="null"/>.</exception>
    public static List<T> DequeueAll<T>(this Queue<T> queue)
    {
        if (queue is null) throw new ArgumentNullException(nameof(queue));

        var items = new List<T>(queue.Count);

        while (queue.Count > 0)
        {
            items.Add(queue.Dequeue());
        }

        return items;
    }

    /// <summary>
    /// Enqueues an item to the queue if it is not null.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="queue">The queue to which the item will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="item">The item to enqueue. If it is <see langword="null"/>, no action is taken.</param>
    /// <returns><see langword="true"/> if <paramref name="item"/> is not null and was added, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="queue"/> is <see langword="null"/>.</exception>
    public static bool EnqueueIfNotNull<T>(this Queue<T> queue, T item)
    {
        if (queue is null) throw new ArgumentNullException(nameof(queue));

        if (item != null)
        {
            queue.Enqueue(item);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Adds the specified items to the queue if the collection is not null and contains elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="queue">The queue to which the item will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The collection of items to enqueue. If <see langword="null"/> or empty, no items are added.</param>
    /// <returns><see langword="true"/> if <paramref name="items"/> is not nullcontains elements and were added to the queue, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="queue"/> is <see langword="null"/>.</exception>
    public static bool EnqueueIfNotEmpty<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        if (queue is null) throw new ArgumentNullException(nameof(queue));

        if (items != null && items.Any())
        {
            queue.EnqueueRange(items);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Dequeues an item from the queue or returns the default value for the type if the queue is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="queue">The queue from which the item will be dequeued. Cannot be <see langword="null"/>.</param>
    /// <param name="defaultValue">The default value to return if the index is out of bounds. The default value is <see langword="default"/>.</param>
    /// <returns>The value of the dequeued item or <paramref name="defaultValue"/> if the queue is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="queue"/> is <see langword="null"/>.</exception>
    public static T? DequeueOrDefault<T>(this Queue<T> queue, T? defaultValue = default)
    {
        if (queue is null) throw new ArgumentNullException(nameof(queue));

        return queue.Count > 0 ? queue.Dequeue() : defaultValue;
    }

    /// <summary>
    /// Peeks an item from the queue or returns the default value for the type if the queue is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    /// <param name="queue">The queue from which the item will be peeked. Cannot be <see langword="null"/>.</param>
    /// <param name="defaultValue">The default value to return if the index is out of bounds. The default value is <see langword="default"/>.</param>
    /// <returns>The value of the peeked item or <paramref name="defaultValue"/> if the queue is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="queue"/> is <see langword="null"/>.</exception>
    public static T? PeekOrDefault<T>(this Queue<T> queue, T? defaultValue = default)
    {
        if (queue is null) throw new ArgumentNullException(nameof(queue));

        return queue.Count > 0 ? queue.Peek() : defaultValue;
    }
}
