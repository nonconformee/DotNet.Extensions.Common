
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
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (action is null) throw new ArgumentNullException(nameof(action));

        foreach (var element in dictionary)
        {
            action(element.Key, element.Value);
        }
    }

    /// <summary>
    /// Adds multiple items to the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the dictionary. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (items is null) throw new ArgumentNullException(nameof(items));
        
        foreach (var item in items)
        {
            dictionary.Add(item);
        }
    }

    /// <summary>
    /// Adds multiple items to the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to which the items will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="items">The items to add to the dictionary. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="items"/> is <see langword="null"/>.</exception>
    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, params KeyValuePair<TKey, TValue>[] items)
        => dictionary.AddRange((IEnumerable<KeyValuePair<TKey, TValue>>)items);

    /// <summary>
    /// Removes multiple items from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="keys">The keys to remove from the dictionary. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="keys"/> is <see langword="null"/>.</exception>
    public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (keys is null) throw new ArgumentNullException(nameof(keys));

        foreach (var key in keys)
        {
            dictionary.Remove(key);
        }
    }

    /// <summary>
    /// Removes multiple items from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary from which the items will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="keys">The keys to remove from the dictionary. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="keys"/> is <see langword="null"/>.</exception>
    public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, params TKey[] keys)
        => dictionary.RemoveRange((IEnumerable<TKey>)keys);

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
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (key is null) throw new ArgumentNullException(nameof(key));

        return dictionary.TryGetValue(key, out var value) ? value : default;
    }

    /// <summary>
    /// Adds a key-value pair to the dictionary if the key does not exist, or updates the value for the key if it already exists.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to modify. Cannot be <see langword="null"/>.</param>
    /// <param name="key">The key to add or update. Cannot be <see langword="null"/>.</param>
    /// <param name="value">The value to associate with the key.</param>
    /// <returns>The value that was added or updated in the dictionary.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
    public static TValue SetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (key is null) throw new ArgumentNullException(nameof(key));
        
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
        
        return value;
    }
}
