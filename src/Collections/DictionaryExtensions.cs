
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
    public static int ForEachPair<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (action is null) throw new ArgumentNullException(nameof(action));

        var index = 0;

        foreach (var element in dictionary)
        {
            action(element.Key, element.Value);
            index++;
        }

        return index;
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
    /// <remarks>This method is useful for safely retrieving values from a dictionary without throwing an exception if the key does not exist.</remarks>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary from which the item will be retrieved. Cannot be <see langword="null"/>.</param>
    /// <param name="key">The key of the item to retrieve from the dictionary. Cannot be <see langword="null"/>.</param>
    /// <param name="defaultValue">The default value to return if the index is out of bounds. The default value is <see langword="default"/>.</param>
    /// <returns>The value associated with <paramref name="key"/> or <paramref name="defaultValue"/> if the dictionary does not contain the specified key.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = default)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (key is null) throw new ArgumentNullException(nameof(key));

        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }

    /// <summary>
    /// Retrieves the value associated with the specified key from the dictionary, or creates and adds a new value if the key does not exist.
    /// </summary>
    /// <remarks>This method is useful for scenarios where you need to ensure a key exists in the dictionary and initialize its value if it does not. The <paramref name="valueFactory"/> is only invoked if the key is not already present in the dictionary.</remarks>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to retrieve the value from or add the new value to. Cannot be <see langword="null"/>.</param>
    /// <param name="key">The key whose associated value is to be retrieved or created. Cannot be <see langword="null"/>.</param>
    /// <param name="valueFactory">A function that generates a value for the specified key if the key does not exist in the dictionary. Cannot be <see langword="null"/>.</param>
    /// <returns>The value associated with the specified key if it exists, the newly created value otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>, <paramref name="key"/>, or <paramref name="valueFactory"/> is <see langword="null"/>.</exception>
    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (key is null) throw new ArgumentNullException(nameof(key));
        if (valueFactory is null) throw new ArgumentNullException(nameof(valueFactory));

        if (!dictionary.TryGetValue(key, out var value))
        {
            value = valueFactory(key);
            dictionary.Add(key, value);
        }

        return value;
    }

    /// <summary>
    /// Adds a key-value pair to the dictionary if the key does not exist, or updates the value for the key if it already exists.
    /// </summary>
    /// <remarks>This method is useful for ensuring that a key-value pair is present in the dictionary with the desired value.</remarks>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to modify. Cannot be <see langword="null"/>.</param>
    /// <param name="key">The key to add or update. Cannot be <see langword="null"/>.</param>
    /// <param name="value">The value to associate with the key.</param>
    /// <returns>The value that was added or updated in the dictionary.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
    public static TValue UpdateOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
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

    /// <summary>
    /// Updates the value associated with the specified key in the dictionary, or creates a new key-value pair if the
    /// key does not exist.
    /// </summary>
    /// <remarks>This method is useful for scenarios where you want to ensure that a key exists in the dictionary and its value is updated or created based on a factory function.</remarks>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to update or create the key-value pair in. Cannot be <see langword="null"/>.</param>
    /// <param name="key">The key to update or add. Cannot be <see langword="null"/>.</param>
    /// <param name="valueFactory">A function that generates the value to associate with the key. Cannot be <see langword="null"/>.</param>
    /// <returns>The value that was added to the dictionary or used to update the existing key.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>, <paramref name="key"/>, or <paramref name="valueFactory"/> is <see langword="null"/>.</exception>
    public static TValue UpdateOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (key is null) throw new ArgumentNullException(nameof(key));
        if (valueFactory is null) throw new ArgumentNullException(nameof(valueFactory));

        var value = valueFactory(key);

        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
        }
        else
        {
            dictionary[key] = value;
        }
        return value;
    }

    /// <summary>
    /// Retrieves the key associated with the specified value in the dictionary.
    /// </summary>
    /// <remarks>If multiple keys in the dictionary are associated with the specified value, the first key encountered is returned.</remarks>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to search for the specified value. Cannot be <see langword="null"/>.</param>
    /// <param name="value">The value to locate in the dictionary.</param>
    /// <param name="equalityComparer">An optional equality comparer to use for comparing values. If <see langword="null"/>, the default equality
    /// comparer for <typeparamref name="TValue"/> is used.</param>
    /// <returns>The key associated with the specified value if found; otherwise, the default value of <typeparamref
    /// name="TKey"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
    public static TKey? GetKeyForValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value, IEqualityComparer<TValue>? equalityComparer = null)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));

        equalityComparer ??= EqualityComparer<TValue>.Default;

        foreach (var pair in dictionary)
        {
            if (equalityComparer.Equals(pair.Value, value))
            {
                return pair.Key;
            }
        }

        return default;
    }

    /// <summary>
    /// Adds a new entry to the dictionary if the specified key does not already exist.
    /// </summary>
    /// <remarks>This method checks whether the specified <paramref name="key"/> exists in the <paramref
    /// name="dictionary"/>. If the key does not exist, it creates a new instance of the specified <paramref
    /// name="type"/> and adds it to the dictionary with the given key. The method uses <see
    /// cref="Activator.CreateInstance(Type)"/> to create the value, so the specified type must have a parameterless
    /// constructor.</remarks>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to which the entry will be added.</param>
    /// <param name="key">The key to check for existence in the dictionary.</param>
    /// <param name="type">The type of the value to create and add to the dictionary if the key does not exist. This type must have a
    /// parameterless constructor.</param>
    /// <returns><see langword="true"/> if a new entry was added to the dictionary, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>, <paramref name="key"/>, or <paramref name="type"/> is <see langword="null"/>.</exception>
    public static bool AddIfNecessary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Type type)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (key is null) throw new ArgumentNullException(nameof(key));
        if (type is null) throw new ArgumentNullException(nameof(type));

        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, (TValue)Activator.CreateInstance(type)!);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Adds a new entry to the dictionary if the specified key does not already exist.
    /// </summary>
    /// <remarks>This method checks whether the specified <paramref name="key"/> exists in the <paramref
    /// name="dictionary"/>. If the key does not exist, it creates a new instance of the specified <typeparamref name="TValue"/> 
    /// and adds it to the dictionary with the given key. The method uses <see
    /// cref="Activator.CreateInstance(Type)"/> to create the value, so the specified type must have a parameterless
    /// constructor.</remarks>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to which the entry will be added.</param>
    /// <param name="key">The key to check for existence in the dictionary.</param>
    /// <returns><see langword="true"/> if a new entry was added to the dictionary, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
    public static bool AddIfNecessary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        => AddIfNecessary(dictionary, key, typeof(TValue));

    /// <summary>
    /// Tries to get the value associated with the specified key and removes it from the dictionary if found.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to search.</param>
    /// <param name="key">The key to look for.</param>
    /// <param name="value">The value associated with the key, if found.</param>
    /// <returns><see langword="true"/> if the key was found and the value was removed; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
    public static bool TryGetAndRemoveValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue? value)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (key is null) throw new ArgumentNullException(nameof(key));

        if (dictionary.TryGetValue(key, out value))
        {
            dictionary.Remove(key);
            return true;
        }

        value = default;
        return false;
    }
}
