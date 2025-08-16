
namespace nonconformee.DotNet.Extensions.Comparison;

/// <summary>
/// Provides extension methods for <see cref="IEqualityComparer{T}"/>.
/// </summary>
public static class EqualityComparerExtensions
{
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

    private sealed class FuncEqualityComparer<T>(
        Func<T?, T?, bool> _equals,
        Func<T?, int> _getHashCode)
        : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y) => _equals(x, y);
        public int GetHashCode(T obj) => _getHashCode(obj);
    }
}
