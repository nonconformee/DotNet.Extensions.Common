
namespace nonconformee.DotNet.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="IDictionary{TKey, TValue}"/>.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Performs the specified action on each element of the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary of elements to iterate over. Cannot be <see langword="null"/>.</param>
    /// <param name="action">The action to perform on each element. Cannot be <see langword="null"/>.</param>
    /// <returns>The original dictionary for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IDictionary<TKey, TValue> ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
    {
        foreach (var element in dictionary)
        {
            action(element.Key, element.Value);
        }

        return dictionary;
    }

    /// <summary>
    /// Adds multiple items to the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the dictionary. Cannot be <see langword="null"/>.</param>
    /// <returns>The original dictionary for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IDictionary<TKey, TValue> AddMulti<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        foreach (var item in items)
        {
            dictionary.Add(item);
        }

        return dictionary;
    }

    /// <summary>
    /// Adds multiple items to the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the dictionary. Cannot be <see langword="null"/>.</param>
    /// <returns>The original dictionary for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static IDictionary<TKey, TValue> AddMulti<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, params KeyValuePair<TKey, TValue>[] items)
    {
        foreach (var item in items)
        {
            dictionary.Add(item);
        }

        return dictionary;
    }

    /// <summary>
    /// Adds one item to the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="key">The key of the item to add to the dictionary. Cannot be <see langword="null"/>.</param>
    /// <param name="value">The value of the item to add to the dictionary. Can be <see langword="null"/>.</param>
    /// <returns>The original dictionary for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
    /// <remarks><see cref="AddMulti{TKey,TValue}(IDictionary{TKey,TValue},TKey,TValue)"/> exists to add to the dictionary and allows further processing by returning its instance (e.g. in fluent chaining of collection operations).</remarks>
    public static IDictionary<TKey, TValue> AddMulti<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        dictionary.Add(key, value);
        return dictionary;
    }

    /// <summary>
    /// Removes multiple items from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="keys">The keys to remove from the dictionary. Cannot be <see langword="null"/>.</param>
    /// <returns>The original dictionary for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="keys"/> is <see langword="null"/>.</exception>
    public static IDictionary<TKey, TValue> RemoveMulti<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
    {
        foreach (var key in keys)
        {
            dictionary.Remove(key);
        }

        return dictionary;
    }

    /// <summary>
    /// Removes multiple items from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="keys">The keys to remove from the dictionary. Cannot be <see langword="null"/>.</param>
    /// <returns>The original dictionary for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="keys"/> is <see langword="null"/>.</exception>
    public static IDictionary<TKey, TValue> RemoveMulti<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, params TKey[] keys)
    {
        foreach (var key in keys)
        {
            dictionary.Remove(key);
        }

        return dictionary;
    }

    /// <summary>
    /// Empties the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary of elements to empty. Cannot be <see langword="null"/>.</param>
    /// <returns>The original dictionary for further processing.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
    /// <remarks><see cref="Empty{TKey,TValue}(IDictionary{TKey,TValue})"/> exists to clear the dictionary and allows further processing by returning its instance (e.g. in fluent chaining of collection operations).</remarks>
    public static IDictionary<TKey, TValue> Empty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        dictionary.Clear();
        return dictionary;
    }

    /// <summary>
    /// Gets a value from the dictionary or returns the default value for the type if the dictionary does not contain the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary from which the item will be retrieved. Cannot be <see langword="null"/>.</param>
    /// <param name="key">The key of the item to retrieve from the dictionary. Cannot be <see langword="null"/>.</param>
    /// <returns>The value associated with <paramref name="key"/> or <see langword="default"/> of <typeparamref name="TValue"/> if the dictionary does not contain the specified key</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.TryGetValue(key, out var value) ? value : default;
    }
}
