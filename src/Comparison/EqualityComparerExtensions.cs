
namespace nonconformee.DotNet.Extensions.Comparison;

/// <summary>
/// Provides extension methods for <see cref="IEqualityComparer{T}"/>.
/// </summary>
public static class EqualityComparerExtensions
{
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

    private sealed class InvertedEqualityComparer<T>(
        IEqualityComparer<T> _comparer)
        : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y) => !_comparer.Equals(x, y);
        public int GetHashCode(T obj) => _comparer.GetHashCode(obj);
    }

    private sealed class FuncEqualityComparer<T>(
        Func<T?, T?, bool> _equals,
        Func<T?, int> _getHashCode)
        : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y) => _equals(x, y);
        public int GetHashCode(T obj) => _getHashCode(obj);
    }
}
