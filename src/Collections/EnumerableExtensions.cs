
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
        foreach (var element in sequence)
        {
            action(element);
        }
    }
}
