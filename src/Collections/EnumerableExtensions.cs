
using nonconformee.DotNet.Extensions.Exceptions;
using nonconformee.DotNet.Extensions.Randomizing;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Xml.Linq;

namespace nonconformee.DotNet.Extensions.Collections;

// TODO : Ensure all default values are documented.
// TODO : Create copy for AsyncEnumerable

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/> and <see cref="IEnumerable"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Converts a synchronous <see cref="IEnumerable{T}"/> sequence to an asynchronous <see cref="IAsyncEnumerable{T}"/> sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence.</param>
    /// <returns>An asynchronous sequence of the same elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> sequence)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));

        return new AsyncEnumerableWrapper<T>(sequence);
    }

    /// <summary>
    /// Converts a non-generic <see cref="IEnumerable"/> sequence to a generic <see cref="IEnumerable{T}"/> sequence.
    /// </summary>
    /// <remarks>This method is useful for working with non-generic collections in a type-safe manner.</remarks>
    /// <typeparam name="T">The type to which the elements of the sequence should be cast.</typeparam>
    /// <param name="sequence">The non-generic sequence to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A generic <see cref="IEnumerable{T}"/> containing the elements of the input sequence cast to type <typeparamref
    /// name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidCastException">Thrown if an element in the sequence cannot be cast to type <typeparamref name="T"/>.</exception>
    public static IEnumerable<T> ToGeneric<T>(this IEnumerable sequence)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));

        foreach (var item in sequence)
        {
            if (item is T typedItem)
            {
                yield return typedItem;
            }
            else
            {
                throw new InvalidCastException($"Cannot cast item of type {item.GetType()} to {typeof(T)}.");
            }
        }
    }

    /// <summary>
    /// Ensures that a sequence is never <see langword="null"/> by returning an empty sequence if the input is <see langword="null"/>.
    /// </summary>
    /// <remarks>This method is useful for avoiding null reference exceptions when working with sequences.</remarks>
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
    /// <returns>A new sequence not containing the excluded elements.</returns>
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
    /// <returns>A new sequence not containing the excluded elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    /// <remarks><paramref name="items"/> will be fully enumerated before the first element of <paramref name="sequence"/> is yielded.</remarks>
    public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, params T[] items)
        => sequence.Exclude((IEnumerable<T>)items);

    /// <summary>
    /// Combines two sequences into a single sequence by interleaving their elements in random order.
    /// </summary>
    /// <remarks>
    /// Because <paramref name="sequence"/> and <paramref name="items"/> are sequences, the number of elements is not known for either of them.
    /// Therefore, the resulting sequence returns elements with a chance of 50% from either of those, until the first sequence is finished, where the remaining elements of the other sequence are returned as they are enumerated in the corresponding sequence.
    /// This means, that the end of the returned sequence might not have mixed elements, depending on the length difference of <paramref name="sequence"/> and <paramref name="items"/>.
    /// But eventually, all elements of both sequences will be returned.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="sequence">The primary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The secondary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="random">An optional <see cref="Random"/> instance used to determine the interleaving order. If <see langword="null"/>, a new instance of <see cref="Random"/> is created.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that yields elements from both <paramref name="sequence"/> and  <paramref name="items"/> in random order until both sequences are fully enumerated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
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
            if (enum1Done && enum2Done)
            {
                yield break;
            }

            var chance = random.NextChance(0.5);
            var useEnum1 = !enum1Done && (enum2Done || chance);
            var useEnum2 = !enum2Done && (enum1Done || !chance);

            if (useEnum1)
            {
                if (enum1.MoveNext())
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
                if (enum2.MoveNext())
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
    /// <remarks>
    /// Because <paramref name="sequence"/> and <paramref name="items"/> are sequences, the number of elements is not known for either of them.
    /// Therefore, the resulting sequence returns elements with a chance of 50% from either of those, until the first sequence is finished, where the remaining elements of the other sequence are returned as they are enumerated in the corresponding sequence.
    /// This means, that the end of the returned sequence might not have mixed elements, depending on the length difference of <paramref name="sequence"/> and <paramref name="items"/>.
    /// But eventually, all elements of both sequences will be returned.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="sequence">The primary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The secondary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="random">An optional <see cref="Random"/> instance used to determine the interleaving order. If <see langword="null"/>, a new instance of <see cref="Random"/> is created.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that yields elements from both <paramref name="sequence"/> and  <paramref name="items"/> in random order until both sequences are fully enumerated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> Mix<T>(this IEnumerable<T> sequence, Random? random = null, params T[] items)
        => sequence.Mix((IEnumerable<T>)items, random);

    /// <summary>
    /// Divides a sequence into smaller batches of a specified size.
    /// </summary>
    /// <remarks>This method is useful for processing large sequences in smaller chunks.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to be divided into batches. Cannot be <see langword="null"/>.</param>
    /// <param name="batchSize">The maximum number of elements in each batch. Must be greater than 0.</param>
    /// <returns>An enumerable of batches, where each batch is an <see cref="IEnumerable{T}"/> containing up to <paramref name="batchSize"/> elements.
    /// The last batch may contain fewer elements if the total number of elements in the sequence is not evenly divisible by <paramref name="batchSize"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="batchSize"/> is 0 or less.</exception>
    public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> sequence, int batchSize)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (batchSize <= 0) throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than zero.");

        var batch = new List<T>(batchSize);

        foreach (var item in sequence)
        {
            batch.Add(item);

            if (batch.Count >= batchSize)
            {
                yield return batch;
                batch = new List<T>(batchSize);
            }
        }

        if (batch.Count > 0)
        {
            yield return batch;
        }
    }

    /// <summary>
    /// Returns distinct elements from a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="source">The sequence to filter for distinct elements. Cannot be <see langword="null"/>.</param>
    /// <param name="equalityComparer">An optional equality comparer to compare elements. If <see langword="null"/>, the default equality comparer is used.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing only distinct elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> source, IEqualityComparer<T>? equalityComparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var seenElements = new HashSet<T>(equalityComparer ?? EqualityComparer<T>.Default);

        foreach (var element in source)
        {
            if (seenElements.Add(element))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Returns distinct elements from a sequence according to a specified key selector.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">The type of key to distinguish elements.</typeparam>
    /// <param name="source">The sequence to filter for distinct elements. Cannot be <see langword="null"/>.</param>
    /// <param name="keySelector">A function to extract the key for each element. Cannot be <see langword="null"/>.</param>
    /// <param name="equalityComparer">An optional equality comparer to compare keys. If <see langword="null"/>, the default equality comparer is used.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing only distinct elements according to the key selector.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey>? equalityComparer = null)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        var seenKeys = new HashSet<TKey>(equalityComparer ?? EqualityComparer<TKey>.Default);

        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Returns an enumerable that allows peeking at the first element of the sequence without consuming it.
    /// </summary>
    /// <remarks>This method is useful for examining the first element of a sequence and still allowing to fully enumerate the returned sequence.
    /// Note that the returned sequence is a different type and instance than <paramref name="sequence"/>.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to peek into. Cannot be <see langword="null"/>.</param>
    /// <returns>A tuple which contains the sequence to start/continue enumeration (<c>Sequence</c>) and the first element (<c>FirstElement</c>, can be <see langword="null"/>).</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    public static (IEnumerable<T> Sequence, T? FirstElement) Peek<T>(this IEnumerable<T> sequence)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));

        T? firstElement = default;
        bool hasFirstElement = false;

        using var enumerator = sequence.GetEnumerator();
        hasFirstElement = enumerator.MoveNext();

        if (hasFirstElement)
        {
            firstElement = enumerator.Current;
        }

        return (new PeekedEnumerable<T>(enumerator, hasFirstElement, firstElement), firstElement);
    }

    /// <summary>
    /// Returns the minimum element in a sequence according to a specified key selector, or <see langword="null"/> if the sequence is empty.
    /// </summary>
    /// <remarks>
    /// This method is useful for finding the smallest element in a sequence based on a specific criterion.
    /// Elements which are <see langword="null"/> are ignored.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">The type of the selector.</typeparam>
    /// <param name="sequence">The sequence. Cannot be <see langword="null"/>.</param>
    /// <param name="selector">The function which selects the value of an item which is then used for comparison. Cannot be <see langword="null"/>.</param>
    /// <returns>The minimum element or the default value of <typeparamref name="T"/> if the sequence is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static T? MinByOrDefault<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector)
        where TKey : IComparable<TKey>
    {
        if (sequence == null) throw new ArgumentNullException(nameof(sequence));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        using var enumerator = sequence.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return default;
        }

        T minElement = enumerator.Current;
        TKey minValue = selector(minElement);

        while (enumerator.MoveNext())
        {
            var candidate = enumerator.Current;
            var candidateValue = selector(candidate);

            if (candidateValue is null)
            {
                break;
            }

            if (minValue is null)
            {
                minElement = candidate;
                minValue = candidateValue;
                continue;
            }

            if (candidateValue.CompareTo(minValue) < 0)
            {
                minElement = candidate;
                minValue = candidateValue;
            }
        }

        return minElement;
    }

    /// <summary>
    /// Returns the maximum element in a sequence according to a specified key selector, or <see langword="null"/> if the sequence is empty.
    /// </summary>
    /// <remarks>
    /// This method is useful for finding the largest element in a sequence based on a specific criterion.
    /// Elements which are <see langword="null"/> are ignored.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">The type of the selector.</typeparam>
    /// <param name="sequence">The sequence. Cannot be <see langword="null"/>.</param>
    /// <param name="selector">The function which selects the value of an item which is then used for comparison. Cannot be <see langword="null"/>.</param>
    /// <returns>The maximum element or the default value of <typeparamref name="T"/> if the sequence is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static T? MaxByOrDefault<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector)
        where TKey : IComparable<TKey>
    {
        if (sequence == null) throw new ArgumentNullException(nameof(sequence));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        using var enumerator = sequence.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return default;
        }

        T maxElement = enumerator.Current;
        TKey maxValue = selector(maxElement);

        while (enumerator.MoveNext())
        {
            var candidate = enumerator.Current;
            var candidateValue = selector(candidate);

            if (candidateValue is null)
            {
                continue;
            }

            if (maxValue is null)
            {
                maxElement = candidate;
                maxValue = candidateValue;
                continue;
            }

            if (candidateValue.CompareTo(maxValue) > 0)
            {
                maxElement = candidate;
                maxValue = candidateValue;
            }
        }

        return maxElement;
    }

    /// <summary>
    /// Disposes all <see cref="IDisposable"/> objects within the sequence and optionally the sequence implementation itself if it implements <see cref="IDisposable"/>.
    /// </summary>
    /// <remarks>This method is useful for cleaning up resources held by objects in a sequence.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of objects to dispose. Cannot be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if at least one object or the sequence itself was disposed, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    public static bool DisposeAll<T>(this IEnumerable<T> sequence)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));

        var disposed = false;

        foreach (var item in sequence)
        {
            if (item is IDisposable disposable1)
            {
                disposable1.Dispose();
                disposed = true;
            }
        }

        if (sequence is IDisposable disposable2)
        {
            disposable2.Dispose();
            disposed = true;
        }

        return disposed;
    }

    private sealed class PeekedEnumerable<T>(
        IEnumerator<T> _enumerator,
        bool _hasFirstElement,
        T? _firstElement)
        : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => new PeekedEnumerator<T>(_enumerator, _hasFirstElement, _firstElement);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class PeekedEnumerator<T>(
        IEnumerator<T> _enumerator,
        bool _hasFirstElement,
        T? _firstElement)
        : IEnumerator<T>
    {
        private bool _isFirstMove = true;

        public T? Current { get; set; } = default!;

        public bool MoveNext()
        {
            if (_isFirstMove)
            {
                if (_hasFirstElement)
                {
                    Current = _firstElement;
                    _hasFirstElement = false;
                    _isFirstMove = false;
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

    private sealed class AsyncEnumerableWrapper<T>(
        IEnumerable<T> _sequence)
        : IAsyncEnumerable<T>
    {
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumeratorWrapper<T>(_sequence.GetEnumerator(), cancellationToken);
        }
    }

    private sealed class AsyncEnumeratorWrapper<T>(
        IEnumerator<T> _enumerator,
        CancellationToken _cancellationToken)
        : IAsyncEnumerator<T>
    {
        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            _cancellationToken.ThrowIfCancellationRequested();
            return new ValueTask<bool>(_enumerator.MoveNext());
        }
    }
}
