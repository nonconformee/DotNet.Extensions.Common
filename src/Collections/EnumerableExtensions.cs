
using System;
using System.Collections.Generic;

namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Performs the specified action on each element of the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each element. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (action is null) throw new ArgumentNullException(nameof(action));

        foreach (var element in sequence)
        {
            action(element);
        }
    }

    /// <summary>
    /// Prepends the specified items to the beginning of the given sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to which the items will be prepended. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to prepend to the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>A new sequence that starts with the specified items followed by the elements of the original sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> IncludeAtStart<T>(this IEnumerable<T> sequence, IEnumerable<T> items)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (items is null) throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
        {
            yield return item;
        }

        foreach (var element in sequence)
        {
            yield return element;
        }
    }

    /// <summary>
    /// Prepends the specified items to the beginning of the given sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to which the items will be prepended. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to prepend to the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>A new sequence that starts with the specified items followed by the elements of the original sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> IncludeAtStart<T>(this IEnumerable<T> sequence, params T[] items)
        => sequence.IncludeAtStart((IEnumerable<T>)items);

    /// <summary>
    /// Appends the specified items to the end of the given sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to which the items will be appended. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to append to the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>A new sequence that starts with the elements of the original sequence followed by specified items.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> IncludeAtEnd<T>(this IEnumerable<T> sequence, IEnumerable<T> items)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (items is null) throw new ArgumentNullException(nameof(items));

        foreach (var element in sequence)
        {
            yield return element;
        }

        foreach (var item in items)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Appends the specified items to the end of the given sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to which the items will be appended. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to append to the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>A new sequence that starts with the elements of the original sequence followed by specified items.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> IncludeAtEnd<T>(this IEnumerable<T> sequence, params T[] items)
        => sequence.IncludeAtEnd((IEnumerable<T>)items);

    /// <summary>
    /// Returns a sequence that excludes the specified items from the original sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to filter. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to exclude from the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>An new sequence not containing the excluded elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <remarks><paramref name="items"/> will be fully enumerated before the first element of <paramref name="sequence"/> is yielded.</remarks>
    public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, IEnumerable<T> items)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (items is null) throw new ArgumentNullException(nameof(items));

        var toExclude = items.ToList();

        foreach (var element in sequence)
        {
            if (toExclude.Contains(element))
            {
                continue;
            }

            yield return element;
        }
    }

    /// <summary>
    /// Returns a sequence that excludes the specified items from the original sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to filter. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to exclude from the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>An new sequence not containing the excluded elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <remarks><paramref name="items"/> will be fully enumerated before the first element of <paramref name="sequence"/> is yielded.</remarks>
    public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, params T[] items)
        => sequence.Exclude((IEnumerable<T>)items);
}
