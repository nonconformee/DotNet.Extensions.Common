
namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="ISet{T}" />.
/// </summary>
/// <remarks>This class contains static methods that extend the functionality of set-related types,  such as <see
/// cref="HashSet{T}"/> or other collections implementing <see cref="ISet{T}"/>. Use these methods to simplify common
/// operations on sets.</remarks>
public static class SetExtensions
{
    /// <summary>
    /// Adds a range of items to the set.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    /// <param name="set"> The set to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items"> The items to add to the set. Cannot be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if at least one item of <paramref name="items"/> was added to the set, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="set"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static bool AddRange<T>(this ISet<T> set, IEnumerable<T> items)
    {
        bool added = false;
        
        foreach (var item in items)
        {
            added |= set.Add(item);
        }

        return added;
    }

    /// <summary>
    /// Adds a range of items to the set.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    /// <param name="set"> The set to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items"> The items to add to the set. Cannot be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if at least one item of <paramref name="items"/> was added to the set, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="set"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static bool AddRange<T>(this ISet<T> set, params T[] items)
    {
        bool added = false;

        foreach (var item in items)
        {
            added |= set.Add(item);
        }

        return added;
    }
}
