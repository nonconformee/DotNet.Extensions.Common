﻿
using nonconformee.DotNet.Extensions.Exceptions;
using nonconformee.DotNet.Extensions.Randomizing;
using System.Collections;

namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Ensures that a sequence is never <see langword="null"/> by returning an empty sequence if the input is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to check for <see langword="null"/>. Can be <see langword="null"/>.</param>
    /// <returns>The original sequence if it is not <see langword="null"/>; otherwise, an empty sequence of type <typeparamref name="T"/>.</returns>
    public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T>? sequence)
        => sequence is null ? Enumerable.Empty<T>() : sequence;

    /// <summary>
    /// Performs the specified action on each element of the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each element. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of elements in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    public static int ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        => sequence.ForEach((element, _) => action(element));

    /// <summary>
    /// Performs the specified action on each element of the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each element. The action receives the elements and their zero-based index. Cannot be <see langword="null"/>.</param>
    /// <returns>The number of elements in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    public static int ForEach<T>(this IEnumerable<T> sequence, Action<T, int> action)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (action is null) throw new ArgumentNullException(nameof(action));

        int index = 0;

        foreach (var element in sequence)
        {
            action(element, index);
            index++;
        }

        return index;
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

    /// <summary>
    /// Combines two sequences into a single sequence by interleaving their elements in random order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="sequence">The primary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The secondary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="random">An optional <see cref="Random"/> instance used to determine the interleaving order. If <see langword="null"/>, a new instance of <see cref="Random"/> is created.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that yields elements from both <paramref name="sequence"/> and  <paramref name="items"/> in random order until both sequences are fully enumerated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <remarks>Because <paramref name="sequence"/> and <paramref name="items"/> are sequences, the number of elements is not known for either of them.
    /// Therefore, the resulting sequence returns elements with a chance of 50% from either of those, until the first sequence is finished, where the remaining elements of the other sequence are returned as they are enumerated in the corresponding sequence.</remarks>
    public static IEnumerable<T> Mix<T>(this IEnumerable<T> sequence, IEnumerable<T> items, Random? random = null)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (items is null) throw new ArgumentNullException(nameof(items));

        random ??= new Random();

        using var enum1 = sequence.GetEnumerator();
        using var enum2 = items.GetEnumerator();

        var enum1Done = false;
        var enum2Done = false;

        while (true)
        {
            if(enum1Done && enum2Done)
            {
                yield break;
            }

            var chance = random.NextChance(0.5);
            var useEnum1 = !enum1Done && (enum2Done || chance);
            var useEnum2 = !enum2Done && (enum1Done || !chance);

            if (useEnum1)
            {
                if(enum1.MoveNext())
                {
                    yield return enum1.Current;
                }
                else
                {
                    enum1Done = true;
                }

                continue;
            }

            if (useEnum2)
            {
                if(enum2.MoveNext())
                {
                    yield return enum2.Current;
                }
                else
                {
                    enum2Done = true;
                }

                continue;
            }
        }
    }

    /// <summary>
    /// Combines two sequences into a single sequence by interleaving their elements in random order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="sequence">The primary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The secondary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="random">An optional <see cref="Random"/> instance used to determine the interleaving order. If <see langword="null"/>, a new instance of <see cref="Random"/> is created.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that yields elements from both <paramref name="sequence"/> and  <paramref name="items"/> in random order until both sequences are fully enumerated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <remarks>Because <paramref name="sequence"/> and <paramref name="items"/> are sequences, the number of elements is not known for either of them.
    /// Therefore, the resulting sequence returns elements with a chance of 50% from either of those, until the first sequence is finished, where the remaining elements of the other sequence are returned as they are enumerated in the corresponding sequence.</remarks>
    public static IEnumerable<T> Mix<T>(this IEnumerable<T> sequence, Random? random = null, params T[] items)
        => sequence.Mix((IEnumerable<T>)items, random);

    /// <summary>
    /// Divides a sequence into smaller batches of a specified size.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to be divided into batches. Cannot be <see langword="null"/>.</param>
    /// <param name="batchSize">The maximum number of elements in each batch. Must be greater than 0.</param>
    /// <returns>An enumerable of batches, where each batch is an <see cref="IEnumerable{T}"/> containing up to <paramref name="batchSize"/> elements.
    /// The last batch may contain fewer elements if the total number of elements in the sequence is not evenly divisible by <paramref name="batchSize"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="batchSize"/> is 0 or less.</exception>
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> sequence, int batchSize)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        batchSize.ThrowIfArgumentIsOutOfRange(1, int.MaxValue);

        using var enumerator = sequence.GetEnumerator();
        var batch = new List<T>(batchSize);

        while (enumerator.MoveNext())
        {
            batch.Add(enumerator.Current);

            if(batch.Count >= batchSize)
            {
                yield return new List<T>(batch);
                batch.Clear();
            }
        }

        if(batch.Count > 0)
        {
            yield return batch;
        }
    }

    /// <summary>
    /// Returns an enumerable that allows peeking at the first element of the sequence without consuming it.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to peek into. Cannot be <see langword="null"/>.</param>
    /// <param name="firstElement">When this method returns, contains the first element of the sequence if it exists; otherwise, <see langword="null"/> if the sequence is empty.</param>
    /// <returns>An enumerable that iterates over the original sequence, starting from the first element.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> Peek<T>(this IEnumerable<T> sequence, out T? firstElement)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));

        using var enumerator = sequence.GetEnumerator();
        var hasFirstElement = enumerator.MoveNext();
        firstElement = enumerator.Current;
        
        return new PeekedEnumerable<T>(enumerator, hasFirstElement, firstElement);
    }

    private sealed class PeekedEnumerable<T>(
        IEnumerator<T> _enumerator,
        bool _hasFirstElement,
        T _firstElement)
        : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => new PeekedEnumerator<T>(_enumerator, _hasFirstElement, _firstElement);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class PeekedEnumerator<T>(
        IEnumerator<T> _enumerator,
        bool _hasFirstElement,
        T _firstElement)
        : IEnumerator<T>
    {
        private bool _isFirstMove = true;

        public T Current { get; set; } = default!;

        public bool MoveNext()
        {
            if(_isFirstMove)
            {
                _isFirstMove = false;

                if (_hasFirstElement)
                {
                    Current = _firstElement;
                    _hasFirstElement = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (_enumerator.MoveNext())
                {
                    Current = _enumerator.Current;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Reset()
        {
            _enumerator.Reset();
            _isFirstMove = false;
            _hasFirstElement = false;
            Current = default!;
        }

        public void Dispose() => _enumerator.Dispose();

        object? IEnumerator.Current => Current;
    }
}
