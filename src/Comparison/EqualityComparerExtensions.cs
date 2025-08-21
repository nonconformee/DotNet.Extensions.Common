
using System.Collections;

namespace nonconformee.DotNet.Extensions.Comparison;

/// <summary>
/// Provides extension methods for <see cref="IEqualityComparer{T}"/>.
/// </summary>
public static class EqualityComparerExtensions
{
    /// <summary>
    /// Converts a non-generic <see cref="IEqualityComparer"/> to a generic <see cref="IEqualityComparer{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="comparer">The non-generic equality comparer to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A generic <see cref="IEqualityComparer{T}"/> that uses the specified non-generic comparer for equality checks
    /// and hash code generation. If the input comparer already implements <see cref="IEqualityComparer{T}"/>, it is
    /// returned directly.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public static IEqualityComparer<T> ToGeneric<T>(this IEqualityComparer comparer)
    {
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        if (comparer is IEqualityComparer<T> genericComparer)
        {
            return genericComparer;
        }

        return new FuncEqualityComparer<T>(
            (x, y) => comparer.Equals(x, y),
            x => comparer.GetHashCode(x!));
    }

    /// <summary>
    /// Converts a generic <see cref="IEqualityComparer{T}"/> to a non-generic <see cref="IEqualityComparer"/>.
    /// </summary>
    /// <remarks>If the provided <paramref name="comparer"/> already implements <see
    /// cref="IEqualityComparer"/>, it is returned directly. Otherwise, a new non-generic comparer is created that
    /// delegates equality checks and hash code generation to the provided generic comparer.</remarks>
    /// <typeparam name="T">The type of objects compared by the generic comparer.</typeparam>
    /// <param name="comparer">The generic equality comparer to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A non-generic <see cref="IEqualityComparer"/> that uses the specified generic comparer for equality comparisons.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public static IEqualityComparer ToNonGeneric<T>(this IEqualityComparer<T> comparer)
    {
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        if (comparer is IEqualityComparer nonGenericComparer)
        {
            return nonGenericComparer;
        }

        return new FuncEqualityComparer<object>(
            (x, y) => comparer.Equals((T?)x, (T?)y),
            x => comparer.GetHashCode((T)x!));
    }

    /// <summary>
    /// Creates an <see cref="IEqualityComparer{T}"/> from a function that compares two objects for equality.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="equals">The delegate which can be used for comparison. Cannot be <see langword="null"/>.</param>
    /// <param name="getHashCode">The delegate which can be used for calculating hash codes. Can be <see langword="null"/> in which case <see cref="object.GetHashCode"/> of the object itself is called.</param>
    /// <returns>The comparer which uses the comparison function.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="equals"/> is <see langword="null"/>.</exception>
    public static IEqualityComparer<T> ToEqualityComparer<T>(this Func<T?, T?, bool> equals, Func<T?, int>? getHashCode = null)
    {
        if (equals is null) throw new ArgumentNullException(nameof(equals));

        return new FuncEqualityComparer<T>(equals, getHashCode ?? (x => x?.GetHashCode() ?? 0));
    }

    /// <summary>
    /// Converts an <see cref="IEqualityComparer{T}"/> to a function that compares two objects for equality.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="comparer">The comparer to convert into a comparison function. Cannot be <see langword="null"/>.</param>
    /// <returns>The comparison function.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public static Func<T?, T?, bool> ToEqualityComparerFunc<T>(this IEqualityComparer<T> comparer)
    {
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        return (x, y) => comparer.Equals(x, y);
    }

    /// <summary>
    /// Creates an <see cref="IEqualityComparer{T}"/> that inverts the comparison of the specified comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="comparer">The comparer whose equality comparison is to be inverted. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="IEqualityComparer{T}"/> that inverts the equality comparison result of the specified <paramref name="comparer"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
    public static IEqualityComparer<T> Invert<T>(this IEqualityComparer<T> comparer)
    {
        if (comparer is null) throw new ArgumentNullException(nameof(comparer));

        return new InvertedEqualityComparer<T>(comparer);
    }

    private sealed class InvertedEqualityComparer<T>(
        IEqualityComparer<T> _comparer)
        : IEqualityComparer<T>, IEqualityComparer
    {
        public bool Equals(T? x, T? y) => !_comparer.Equals(x, y);
        public int GetHashCode(T obj) => _comparer.GetHashCode(obj!);

        bool IEqualityComparer.Equals(object? x, object? y) => Equals((T?)x, (T?)y);
        int IEqualityComparer.GetHashCode(object obj) => GetHashCode((T?)obj!);
    }

    private sealed class FuncEqualityComparer<T>(
        Func<T?, T?, bool> _equals,
        Func<T?, int> _getHashCode)
        : IEqualityComparer<T>, IEqualityComparer
    {
        public bool Equals(T? x, T? y) => _equals(x, y);
        public int GetHashCode(T obj) => _getHashCode(obj!);

        bool IEqualityComparer.Equals(object? x, object? y) => throw new NotImplementedException();
        int IEqualityComparer.GetHashCode(object obj) => throw new NotImplementedException();
    }
}
