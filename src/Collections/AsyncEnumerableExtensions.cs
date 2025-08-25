
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using nonconformee.DotNet.Extensions.Async;
using nonconformee.DotNet.Extensions.Randomizing;

namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="IAsyncEnumerable{T}"/>.
/// </summary>
public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// Converts an asynchronous <see cref="IAsyncEnumerable{T}"/> sequence to a synchronous <see cref="IEnumerable{T}"/> sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the enumeration.</param>
    /// <returns>The <see cref="IEnumerable{T}"/> which synchronously iterates over <paramref name="sequence"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> ToSyncEnumerable<T>(this IAsyncEnumerable<T> sequence, CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));

        var enumerator = sequence.GetAsyncEnumerator(cancellationToken);

        try
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!enumerator.MoveNextAsync().GetAwaiter().GetResult())
                {
                    yield break;
                }

                yield return enumerator.Current;
            }
        }
        finally
        {
            enumerator.DisposeAsync().GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Ensures that a sequence is never <see langword="null"/> by returning an empty sequence if the input is <see langword="null"/>.
    /// </summary>
    /// <remarks>This method is useful for avoiding null reference exceptions when working with asynchronous sequences.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to check for <see langword="null"/>. Can be <see langword="null"/>.</param>
    /// <returns>The original sequence if it is not <see langword="null"/>; otherwise, an empty sequence of type <typeparamref name="T"/>.</returns>
    public static IAsyncEnumerable<T> ToEmptyIfNull<T>(this IAsyncEnumerable<T>? sequence)
        => sequence ?? EmptyAsync<T>();

    /// <summary>
    /// Performs the specified action on each element of the sequence asynchronously.
    /// </summary>
    /// <remarks>This method is useful for performing operations on each element without creating a new collection.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each element. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The number of elements in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public static ValueTask<int> ForEachAsync<T>(this IAsyncEnumerable<T> sequence, Action<T, CancellationToken> action, CancellationToken cancellationToken = default)
        => sequence.ForEachAsync((element, _) => action(element, cancellationToken), cancellationToken);

    /// <summary>
    /// Performs the specified action on each element of the sequence asynchronously.
    /// </summary>
    /// <remarks>This method is useful for performing operations on each element without creating a new collection.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each element. The action receives the elements and their zero-based index. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The number of elements in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public static async ValueTask<int> ForEachAsync<T>(this IAsyncEnumerable<T> sequence, Action<T, int, CancellationToken> action, CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (action is null) throw new ArgumentNullException(nameof(action));

        int index = 0;

        await foreach (var element in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            action(element, index, cancellationToken);
            index++;
        }

        return index;
    }

    /// <summary>
    /// Performs the specified asynchronous action on each element of the sequence.
    /// </summary>
    /// <remarks>This method is useful for performing asynchronous operations on each element without creating a new collection.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The asynchronous action to perform on each element. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The number of elements in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public static ValueTask<int> ForEachAsync<T>(this IAsyncEnumerable<T> sequence, Func<T, CancellationToken?, ValueTask> action, CancellationToken cancellationToken = default)
        => sequence.ForEachAsync((element, _) => action(element, cancellationToken), cancellationToken);

    /// <summary>
    /// Performs the specified asynchronous action on each element of the sequence.
    /// </summary>
    /// <remarks>This method is useful for performing asynchronous operations on each element without creating a new collection.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The asynchronous action to perform on each element. The action receives the elements and their zero-based index. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The number of elements in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public static async ValueTask<int> ForEachAsync<T>(this IAsyncEnumerable<T> sequence, Func<T, int, CancellationToken, ValueTask> action, CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (action is null) throw new ArgumentNullException(nameof(action));

        int index = 0;

        await foreach (var element in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await action(element, index, cancellationToken).ConfigureAwait(false);
            index++;
        }

        return index;
    }

    /// <summary>
    /// Prepends the specified items to the beginning of the given sequence.
    /// </summary>
    /// <remarks>This method is useful for adding elements to the start of an asynchronous sequence.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to which the items will be prepended. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to prepend to the sequence. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A new sequence that starts with the specified items followed by the elements of the original sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static async IAsyncEnumerable<T> IncludeAtStartAsync<T>(this IAsyncEnumerable<T> sequence, IEnumerable<T> items, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (items is null) throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return item;
        }

        await foreach (var element in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return element;
        }
    }

    /// <summary>
    /// Prepends the specified items to the beginning of the given sequence.
    /// </summary>
    /// <remarks>This method is useful for adding elements to the start of an asynchronous sequence.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to which the items will be prepended. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="items">The items to prepend to the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>A new sequence that starts with the specified items followed by the elements of the original sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IAsyncEnumerable<T> IncludeAtStartAsync<T>(this IAsyncEnumerable<T> sequence, CancellationToken cancellationToken = default, params T[] items)
        => sequence.IncludeAtStartAsync((IEnumerable<T>)items, cancellationToken);

    /// <summary>
    /// Appends the specified items to the end of the given sequence.
    /// </summary>
    /// <remarks>This method is useful for adding elements to the end of an asynchronous sequence.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to which the items will be appended. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to append to the sequence. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A new sequence that starts with the elements of the original sequence followed by specified items.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static async IAsyncEnumerable<T> IncludeAtEndAsync<T>(this IAsyncEnumerable<T> sequence, IEnumerable<T> items, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (items is null) throw new ArgumentNullException(nameof(items));

        await foreach (var element in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return element;
        }

        foreach (var item in items)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return item;
        }
    }

    /// <summary>
    /// Appends the specified items to the end of the given sequence.
    /// </summary>
    /// <remarks>This method is useful for adding elements to the end of an asynchronous sequence.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to which the items will be appended. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to append to the sequence. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A new sequence that starts with the elements of the original sequence followed by specified items.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IAsyncEnumerable<T> IncludeAtEndAsync<T>(this IAsyncEnumerable<T> sequence, CancellationToken cancellationToken = default, params T[] items)
        => sequence.IncludeAtEndAsync((IEnumerable<T>)items, cancellationToken);

    /// <summary>
    /// Returns a sequence that excludes the specified items from the original sequence.
    /// </summary>
    /// <remarks>
    /// <paramref name="items"/> will be fully enumerated before the first element of <paramref name="sequence"/> is yielded.
    /// This method is useful for filtering out specific elements from an asynchronous sequence.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to filter. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to exclude from the sequence. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An new sequence not containing the excluded elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static async IAsyncEnumerable<T> ExcludeAsync<T>(this IAsyncEnumerable<T> sequence, IEnumerable<T> items, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (items is null) throw new ArgumentNullException(nameof(items));

        var toExclude = items.ToList();

        await foreach (var element in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

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
    /// <remarks>
    /// <paramref name="items"/> will be fully enumerated before the first element of <paramref name="sequence"/> is yielded.
    /// This method is useful for filtering out specific elements from an asynchronous sequence.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The original sequence to filter. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="items">The items to exclude from the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>A new sequence not containing the excluded elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IAsyncEnumerable<T> ExcludeAsync<T>(this IAsyncEnumerable<T> sequence, CancellationToken cancellationToken = default, params T[] items)
        => sequence.ExcludeAsync((IEnumerable<T>)items, cancellationToken);

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
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="IAsyncEnumerable{T}"/> that yields elements from both <paramref name="sequence"/> and <paramref name="items"/> in random order until both sequences are fully enumerated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static async IAsyncEnumerable<T> MixAsync<T>(this IAsyncEnumerable<T> sequence, IAsyncEnumerable<T> items, Random? random = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (items is null) throw new ArgumentNullException(nameof(items));

        random ??= new Random();

        var enum1 = sequence.GetAsyncEnumerator(cancellationToken);
        var enum2 = items.GetAsyncEnumerator(cancellationToken);

        try
        {
            var enum1Done = false;
            var enum2Done = false;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (enum1Done && enum2Done)
                {
                    yield break;
                }

                var chance = random.NextChance(0.5);
                var useEnum1 = !enum1Done && (enum2Done || chance);
                var useEnum2 = !enum2Done && (enum1Done || !chance);

                if (useEnum1)
                {
                    if (await enum1.MoveNextAsync().ConfigureAwait(false))
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
                    if (await enum2.MoveNextAsync().ConfigureAwait(false))
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
        finally
        {
            if (enum1 != null) await enum1.DisposeAsync().ConfigureAwait(false);
            if (enum2 != null) await enum2.DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Combines a sequence with items into a single sequence by interleaving their elements in random order.
    /// </summary>
    /// <remarks>
    /// Because <paramref name="sequence"/> and <paramref name="items"/> are sequences, the number of elements is not known for either of them.
    /// Therefore, the resulting sequence returns elements with a chance of 50% from either of those, until the first sequence is finished, where the remaining elements of the other sequence are returned as they are enumerated in the corresponding sequence.
    /// This means, that the end of the returned sequence might not have mixed elements, depending on the length difference of <paramref name="sequence"/> and <paramref name="items"/>.
    /// But eventually, all elements of both sequences will be returned.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="sequence">The primary sequence to be mixed. Cannot be <see langword="null"/>.</param>
    /// <param name="random">An optional <see cref="Random"/> instance used to determine the interleaving order. If <see langword="null"/>, a new instance of <see cref="Random"/> is created.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="items">The items to be mixed with the sequence. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields elements from both <paramref name="sequence"/> and <paramref name="items"/> in random order until both are fully enumerated.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IAsyncEnumerable<T> MixAsync<T>(this IAsyncEnumerable<T> sequence, Random? random = null, CancellationToken cancellationToken = default, params T[] items)
        => sequence.MixAsync(items.ToAsyncEnumerable(), random, cancellationToken);

    /// <summary>
    /// Divides a sequence into smaller batches of a specified size.
    /// </summary>
    /// <remarks>This method is useful for processing large asynchronous sequences in smaller chunks.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to be divided into batches. Cannot be <see langword="null"/>.</param>
    /// <param name="batchSize">The maximum number of elements in each batch. Must be greater than 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An enumerable of batches, where each batch is a list containing up to <paramref name="batchSize"/> elements.
    /// The last batch may contain fewer elements if the total number of elements in the sequence is not evenly divisible by <paramref name="batchSize"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="batchSize"/> is 0 or less.</exception>
    public static async IAsyncEnumerable<List<T>> BatchAsync<T>(this IAsyncEnumerable<T> sequence, int batchSize, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (batchSize <= 0) throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than zero.");

        var batch = new List<T>(batchSize);
        
        await foreach (var item in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

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
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> containing only distinct elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    public static async IAsyncEnumerable<T> DistinctByAsync<T>(this IAsyncEnumerable<T> source, IEqualityComparer<T>? equalityComparer = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var seenElements = new HashSet<T>(equalityComparer ?? EqualityComparer<T>.Default);

        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

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
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> containing only distinct elements according to the key selector.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
    public static async IAsyncEnumerable<T> DistinctByAsync<T, TKey>(this IAsyncEnumerable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey>? equalityComparer = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        var seenKeys = new HashSet<TKey>(equalityComparer ?? EqualityComparer<TKey>.Default);

        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (seenKeys.Add(keySelector(element)))
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
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> containing only distinct elements according to the key selector.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.</exception>
    public static async IAsyncEnumerable<T> DistinctByAsync<T, TKey>(this IAsyncEnumerable<T> source, Func<T, CancellationToken?, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? equalityComparer = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

        var seenKeys = new HashSet<TKey>(equalityComparer ?? EqualityComparer<TKey>.Default);

        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (seenKeys.Add(await keySelector(element, cancellationToken).ConfigureAwait(false)))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Returns an enumerable that allows peeking at the first element of the sequence without consuming it.
    /// </summary>
    /// <remarks>
    /// This method is useful for examining the first element of an asynchronous sequence and still allowing to fully enumerate the returned sequence.
    /// Note that the returned sequence is a different type and instance than <paramref name="sequence"/>.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence to peek into. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A tuple which contains the sequence to start/continue enumeration (<c>Sequence</c>) and the first element (<c>FirstElement</c>, can be <see langword="null"/>).</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    public static async Task<(IAsyncEnumerable<T> Sequence, T? FirstElement)> PeekAsync<T>(this IAsyncEnumerable<T> sequence, CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));

        T? firstElement = default;
        bool hasFirstElement = false;
        
        await using var enumerator = sequence.GetAsyncEnumerator(cancellationToken);
        hasFirstElement = await enumerator.MoveNextAsync().ConfigureAwait(false);
        
        if (hasFirstElement)
        {
            firstElement = enumerator.Current;
        }

        return (new PeekedAsyncEnumerable<T>(enumerator, hasFirstElement, firstElement, cancellationToken), firstElement);
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
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The minimum element or the default value of <typeparamref name="T"/> if the sequence is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static async ValueTask<T?> MinByOrDefaultAsync<T, TKey>(this IAsyncEnumerable<T> sequence, Func<T, TKey> selector, CancellationToken cancellationToken = default)
        where TKey : IComparable<TKey>
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        T? minElement = default;
        TKey? minValue = default;
        bool hasValue = false;

        await foreach (var element in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var value = selector(element);
            
            if (!hasValue || value.CompareTo(minValue!) < 0)
            {
                minElement = element;
                minValue = value;
                hasValue = true;
            }
        }

        return hasValue ? minElement : default;
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
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The maximum element or the default value of <typeparamref name="T"/> if the sequence is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
    public static async ValueTask<T?> MaxByOrDefaultAsync<T, TKey>(this IAsyncEnumerable<T> sequence, Func<T, TKey> selector, CancellationToken cancellationToken = default)
        where TKey : IComparable<TKey>
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        T? maxElement = default;
        TKey? maxValue = default;
        bool hasValue = false;

        await foreach (var element in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var value = selector(element);
            
            if (!hasValue || value.CompareTo(maxValue!) > 0)
            {
                maxElement = element;
                maxValue = value;
                hasValue = true;
            }
        }

        return hasValue ? maxElement : default;
    }

    /// <summary>
    /// Disposes all <see cref="IAsyncDisposable"/> objects within the sequence and optionally the sequence implementation itself if it implements <see cref="IAsyncDisposable"/>.
    /// </summary>
    /// <remarks>If <paramref name="includeSynchronous"/> is <see langword="true"/>, <see cref="IAsyncDisposable.DisposeAsync"/> is called before <see cref="IDisposable.Dispose"/>.</remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence of objects to dispose. Cannot be <see langword="null"/>.</param>
    /// <param name="includeSynchronous"><see langword="true"/> if synchronous dispose (<see cref="IDisposable"/>) shall also be called (same as calling <see cref="EnumerableExtensions.DisposeAll{T}(IEnumerable{T})"/>), <see langword="false"/> if only <see cref="IAsyncDisposable"/> shall be called. Default value is <see langword="true"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns><see langword="true"/> if at least one object or the sequence itself was disposed, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
    /// <remarks>If <paramref name="includeSynchronous"/> is <see langword="true"/>, <see cref="IAsyncDisposable"/> is called befor <see cref="IDisposable"/>.</remarks>
    public static async ValueTask<bool> DisposeAllAsync<T>(this IAsyncEnumerable<T> sequence, bool includeSynchronous = true, CancellationToken cancellationToken = default)
    {
        if (sequence is null) throw new ArgumentNullException(nameof(sequence));

        var disposed = false;

        await foreach (var item in sequence.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (item is IAsyncDisposable disposable1)
            {
                await disposable1.DisposeAsync().ConfigureAwait(false);
                disposed = true;
            }

            if (includeSynchronous && item is IDisposable disposable2)
            {
                disposable2.Dispose();
                disposed = true;
            }
        }

        if (sequence is IAsyncDisposable disposable3)
        {
            await disposable3.DisposeAsync().ConfigureAwait(false);
            disposed = true;
        }

        if (includeSynchronous && sequence is IDisposable disposable4)
        {
            disposable4.Dispose();
            disposed = true;
        }

        return disposed;
    }
    
    private static async IAsyncEnumerable<T> EmptyAsync<T>()
    {
        await Task.CompletedTask;
        yield break;
    }

    private sealed class PeekedAsyncEnumerable<T>(
        IAsyncEnumerator<T> _enumerator,
        bool _hasFirstElement,
        T? _firstElement,
        CancellationToken _cancellationToken)
        : IAsyncEnumerable<T>
    {
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new PeekedAsyncEnumerator<T>(
                _enumerator,
                _hasFirstElement,
                _firstElement,
                CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken, cancellationToken).Token);
        }
    }

    private sealed class PeekedAsyncEnumerator<T>(
        IAsyncEnumerator<T> _enumerator,
        bool _hasFirstElement,
        T? _firstElement,
        CancellationToken _cancellationToken)
        : IAsyncEnumerator<T>
    {
        private bool _isFirstMove = true;

        public T Current { get; private set; } = default!;

        public async ValueTask<bool> MoveNextAsync()
        {
            _cancellationToken.ThrowIfCancellationRequested();
            
            if (_isFirstMove)
            {
                if (_hasFirstElement)
                {
                    Current = _firstElement!;
                    _hasFirstElement = false;
                    _isFirstMove = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
            {
                Current = _enumerator.Current;
                return true;
            }
            else
            {
                return false;
            }
        }

        public ValueTask DisposeAsync() => _enumerator.DisposeAsync();
    }
}