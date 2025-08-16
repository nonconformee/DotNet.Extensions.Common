
namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="LinkedList{T}"/>.
/// </summary>
public static class LinkedListExtensions
{
    /// <summary>
    /// Iterates through each node in the linked list and performs the specified action on it.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the linked list.</typeparam>
    /// <param name="list">The linked list to iterate through. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each node. Cannot be <see langword="null"/>.</param>
    /// <returns>The total number of nodes in the linked list.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/> or if <paramref name="action"/> is <see langword="null"/>.</exception>
    public static int ForEachNode<T>(this LinkedList<T> list, Action<LinkedListNode<T>> action)
        => ForEachNode(list, (node, _) => action(node));

    /// <summary>
    /// Iterates through each node in the linked list and performs the specified action on it.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the linked list.</typeparam>
    /// <param name="list">The linked list to iterate through. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each node. The action receives the nodes and their zero-based index. Cannot be <see langword="null"/>.</param>
    /// <returns>The total number of nodes in the linked list.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/> or if <paramref name="action"/> is <see langword="null"/>.</exception>
    public static int ForEachNode<T>(this LinkedList<T> list, Action<LinkedListNode<T>, int> action)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (action is null) throw new ArgumentNullException(nameof(action));
        
        var node = list.First;
        var index = 0;

        while(node != null)
        {
            action(node, index);
            node = node.Next;
            index++;
        }

        return index;
    }

    /// <summary>
    /// Adds a range of elements after the specified node in the linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list. Cannot be <see langword="null"/>.</param>
    /// <param name="node">The node after which the elements will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The elements to add. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/>, <paramref name="node"/>, or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddAfterRange<T>(this LinkedList<T> list, LinkedListNode<T> node, IEnumerable<T> items)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (node is null) throw new ArgumentNullException(nameof(node));
        if (items is null) throw new ArgumentNullException(nameof(items));

        LinkedListNode<T> current = node;

        foreach (var item in items)
        {
            current = list.AddAfter(current, item);
        }
    }

    /// <summary>
    /// Adds a range of elements after the specified node in the linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list. Cannot be <see langword="null"/>.</param>
    /// <param name="node">The node after which the elements will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The elements to add. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/>, <paramref name="node"/>, or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddAfterRange<T>(this LinkedList<T> list, LinkedListNode<T> node, params T[] items)
        => AddAfterRange(list, node, (IEnumerable<T>)items);

    /// <summary>
    /// Adds a range of elements before the specified node in the linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list. Cannot be <see langword="null"/>.</param>
    /// <param name="node">The node before which the elements will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The elements to add. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/>, <paramref name="node"/>, or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddBeforeRange<T>(this LinkedList<T> list, LinkedListNode<T> node, IEnumerable<T> items)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (node is null) throw new ArgumentNullException(nameof(node));
        if (items is null) throw new ArgumentNullException(nameof(items));

        var stack = new Stack<T>();

        foreach (var item in items)
        {
            stack.Push(item);
        }

        while (stack.Count > 0)
        {
            list.AddBefore(node, stack.Pop());
        }
    }

    /// <summary>
    /// Adds a range of elements before the specified node in the linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list. Cannot be <see langword="null"/>.</param>
    /// <param name="node">The node before which the elements will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The elements to add. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/>, <paramref name="node"/>, or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddBeforeRange<T>(this LinkedList<T> list, LinkedListNode<T> node, params T[] items)
        => AddBeforeRange(list, node, (IEnumerable<T>)items);

    /// <summary>
    /// Adds a range of elements at the beginning of the linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The elements to add at the beginning. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddFirstRange<T>(this LinkedList<T> list, IEnumerable<T> items)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (items is null) throw new ArgumentNullException(nameof(items));

        var stack = new Stack<T>();

        foreach (var item in items)
        {
            stack.Push(item);
        }

        while (stack.Count > 0)
        {
            list.AddFirst(stack.Pop());
        }
    }

    /// <summary>
    /// Adds a range of elements at the beginning of the linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The elements to add at the beginning. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddFirstRange<T>(this LinkedList<T> list, params T[] items)
        => AddFirstRange(list, (IEnumerable<T>)items);

    /// <summary>
    /// Adds a range of elements at the end of the linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The elements to add at the end. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddLastRange<T>(this LinkedList<T> list, IEnumerable<T> items)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (items is null) throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
        {
            list.AddLast(item);
        }
    }

    /// <summary>
    /// Adds a range of elements at the end of the linked list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The elements to add at the end. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddLastRange<T>(this LinkedList<T> list, params T[] items)
        => AddLastRange(list, (IEnumerable<T>)items);

    /// <summary>
    /// Enumerates the elements of the linked list in forward order, starting from the first node.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list to enumerate. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that iterates through the elements of the linked list in forward order.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> ForwardItems<T>(this LinkedList<T> list)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        var current = list.First;

        while (current is not null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    /// <summary>
    /// Enumerates the elements of the linked list in backward order, starting from the last node.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list to enumerate. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that iterates through the elements of the linked list in backward order.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> BackwardItems<T>(this LinkedList<T> list)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        var current = list.Last;

        while(current is not null)
        {
            yield return current.Value;
            current = current.Previous;
        }
    }

    /// <summary>
    /// Enumerates the nodes of the linked list in forward order, starting from the first node.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list to enumerate. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that iterates through the nodes of the linked list in forward order.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static IEnumerable<LinkedListNode<T>> ForwardNodes<T>(this LinkedList<T> list)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        var current = list.First;

        while (current is not null)
        {
            yield return current;
            current = current.Next;
        }
    }

    /// <summary>
    /// Enumerates the nodes of the linked list in backward order, starting from the last node.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list to enumerate. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that iterates through the nodes of the linked list in backward order.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static IEnumerable<LinkedListNode<T>> BackwardNodes<T>(this LinkedList<T> list)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        var current = list.Last;

        while (current is not null)
        {
            yield return current;
            current = current.Previous;
        }
    }

    /// <summary>
    /// Finds all nodes in the linked list that match the specified value using the provided equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list to search. Cannot be <see langword="null"/>.</param>
    /// <param name="value">The value to match against.</param>
    /// <param name="equalityComparer">An optional equality comparer to use for the comparison.</param>
    /// <returns>A list of all matching nodes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
    public static List<LinkedListNode<T>> FindAll<T>(this LinkedList<T> list, T value, IEqualityComparer<T>? equalityComparer = null)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        equalityComparer ??= EqualityComparer<T>.Default;

        var result = new List<LinkedListNode<T>>();
        var current = list.First;

        while (current is not null)
        {
            if (equalityComparer.Equals(current.Value, value))
            {
                result.Add(current);
            }

            current = current.Next;
        }

        return result;
    }

    /// <summary>
    /// Finds all nodes in the linked list that match the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    /// <param name="list">The linked list to search. Cannot be <see langword="null"/>.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A list of all matching nodes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static List<LinkedListNode<T>> FindAll<T>(this LinkedList<T> list, Func<T, bool> predicate)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var result = new List<LinkedListNode<T>>();
        var current = list.First;

        while (current is not null)
        {
            if (predicate(current.Value))
            {
                result.Add(current);
            }

            current = current.Next;
        }

        return result;
    }
}
