
namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="Stack{T}"/>.
/// </summary>
public static class StackExtensions
{
    /// <summary>
    /// Adds a range of items to the stack.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack"> The stack to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items"> The items to add to the stack. Cannot be <see langword="null"/>.</param>
    /// <returns>The original stack for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stack"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static Stack<T> PushMulti<T>(this Stack<T> stack, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            stack.Push(item);
        }

        return stack;
    }

    /// <summary>
    /// Adds a range of items to the stack.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack"> The stack to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items"> The items to add to the stack. Cannot be <see langword="null"/>.</param>
    /// <returns>The original stack for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stack"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static Stack<T> PushMulti<T>(this Stack<T> stack, params T[] items)
    {
        foreach (var item in items)
        {
            stack.Push(item);
        }

        return stack;
    }

    /// <summary>
    /// Pops all items from the stack.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack"> The stack to which the items will be popped. Cannot be <see langword="null"/>.</param>
    /// <returns>A list containing all items that were in the stack. If the stack was empty, an empty list is returned.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stack"/> is <see langword="null"/>.</exception>
    public static List<T> PopAll<T>(this Stack<T> stack)
    {
        var items = new List<T>(stack.Count);

        while (stack.Count > 0)
        {
            items.Add(stack.Pop());
        }

        return items;
    }

    /// <summary>
    /// Empties the stack.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack">The stack of elements to empty. Cannot be <see langword="null"/>.</param>
    /// <returns>The original stack for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stack"/> is <see langword="null"/>.</exception>
    /// <remarks><see cref="Empty{T}(Stack{T})"/> exists to clear the stack and allows further processing by returning its instance (e.g. in fluent chaining of queue operations).</remarks>
    public static Stack<T> Empty<T>(this Stack<T> stack)
    {
        stack.Clear();
        return stack;
    }

    /// <summary>
    /// Pushes an item to the stack if it is not null.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack">The stack to which the item will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="item">The item to pushed. If it is <see langword="null"/>, no action is taken.</param>
    /// <returns><see langword="true"/> if <paramref name="item"/> is not null and was added, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stack"/> is <see langword="null"/>.</exception>
    public static bool PushIfNotNull<T>(this Stack<T> stack, T item)
    {
        if (item != null)
        {
            stack.Push(item);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Adds the specified items to the stack if the collection is not null and contains elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack">The stack to which the item will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The collection of items to push. If <see langword="null"/> or empty, no items are added.</param>
    /// <returns><see langword="true"/> if <paramref name="items"/> is not nullcontains elements and were added to the stack, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stack"/> is <see langword="null"/>.</exception>
    public static bool PushIfNotEmpty<T>(this Stack<T> stack, IEnumerable<T> items)
    {
        if (items != null && items.Any())
        {
            stack.PushMulti(items);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Pops an item from the stack or returns the default value for the type if the stack is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack">The stack from which the item will be popped. Cannot be <see langword="null"/>.</param>
    /// <returns>The value of the popped item or <see langword="default"/> of <typeparamref name="T"/> if the stack is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stack"/> is <see langword="null"/>.</exception>
    public static T? PopOrDefault<T>(this Stack<T> stack)
    {
        return stack.Count > 0 ? stack.Pop() : default;
    }

    /// <summary>
    /// Peeks an item from the stack or returns the default value for the type if the stack is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack">The stack from which the item will be peeked. Cannot be <see langword="null"/>.</param>
    /// <returns>The value of the peeked item or <see langword="default"/> of <typeparamref name="T"/> if the stack is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stack"/> is <see langword="null"/>.</exception>
    public static T? PeekOrDefault<T>(this Stack<T> stack)
    {
        return stack.Count > 0 ? stack.Peek() : default;
    }
}
