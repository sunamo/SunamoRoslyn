namespace SunamoRoslyn._sunamo;

/// <summary>
/// Dictionary implementation for file system watcher operations. Must be internal due to SourceCodeIndexerRoslyn usage.
/// </summary>
/// <typeparam name="T">The key type.</typeparam>
/// <typeparam name="U">The value type.</typeparam>
internal class FsWatcherDictionary<T, U> : IDictionary<T, U>
    where T : notnull
{
    private readonly Dictionary<T, U> dictionary = new();

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The value or default if not found.</returns>
    internal U? this[T key]
    {
        get
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            return default;
        }
        set => dictionary[key] = value!;
    }

    /// <summary>
    /// Gets the collection of keys.
    /// </summary>
    internal ICollection<T> Keys => dictionary.Keys;

    /// <summary>
    /// Gets the collection of values.
    /// </summary>
    internal ICollection<U> Values => dictionary.Values;

    /// <summary>
    /// Gets the number of elements.
    /// </summary>
    internal int Count => dictionary.Count;

    /// <summary>
    /// Gets whether the dictionary is read-only.
    /// </summary>
    internal bool IsReadOnly => false;

    /// <summary>
    /// Adds a key-value pair, ignoring if key already exists.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    internal void Add(T key, U value)
    {
        lock (dictionary)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, value);
        }
    }

    /// <summary>
    /// Adds a key-value pair from a KeyValuePair.
    /// </summary>
    /// <param name="keyValuePair">The key-value pair to add.</param>
    internal void Add(KeyValuePair<T, U> keyValuePair)
    {
        Add(keyValuePair.Key, keyValuePair.Value);
    }

    /// <summary>
    /// Clears all entries from the dictionary.
    /// </summary>
    internal void Clear()
    {
        dictionary.Clear();
    }

    /// <summary>
    /// Checks if the dictionary contains a specific key-value pair.
    /// </summary>
    /// <param name="keyValuePair">The key-value pair to check.</param>
    /// <returns>True if found.</returns>
    internal bool Contains(KeyValuePair<T, U> keyValuePair)
    {
        return dictionary.Contains(keyValuePair);
    }

    /// <summary>
    /// Checks if the dictionary contains the specified key.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the key exists.</returns>
    internal bool ContainsKey(T key)
    {
        return dictionary.ContainsKey(key);
    }

    /// <summary>
    /// Copies the dictionary entries to an array.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The starting index in the array.</param>
    internal void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < dictionary.Count)
            throw new ArgumentException("Array is too small");

        ((ICollection<KeyValuePair<T, U>>)dictionary).CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the dictionary.
    /// </summary>
    /// <returns>The enumerator.</returns>
    internal IEnumerator<KeyValuePair<T, U>> GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    /// <summary>
    /// Removes the element with the specified key.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    /// <returns>True if the element was removed.</returns>
    internal bool Remove(T key)
    {
        return dictionary.Remove(key);
    }

    /// <summary>
    /// Removes the specified key-value pair.
    /// </summary>
    /// <param name="keyValuePair">The key-value pair to remove.</param>
    /// <returns>True if the element was removed.</returns>
    internal bool Remove(KeyValuePair<T, U> keyValuePair)
    {
        return dictionary.Remove(keyValuePair.Key);
    }

    /// <summary>
    /// Tries to get the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key to look up.</param>
    /// <param name="value">The found value.</param>
    /// <returns>True if the key was found.</returns>
    internal bool TryGetValue(T key, out U value)
    {
        var result = dictionary.TryGetValue(key, out value!);
        return result;
    }

    U IDictionary<T, U>.this[T key]
    {
        get => this[key]!;
        set => this[key] = value;
    }

    ICollection<T> IDictionary<T, U>.Keys => Keys;
    ICollection<U> IDictionary<T, U>.Values => Values;
    int ICollection<KeyValuePair<T, U>>.Count => Count;
    bool ICollection<KeyValuePair<T, U>>.IsReadOnly => IsReadOnly;

    void IDictionary<T, U>.Add(T key, U value) => Add(key, value);
    void ICollection<KeyValuePair<T, U>>.Add(KeyValuePair<T, U> item) => Add(item);
    void ICollection<KeyValuePair<T, U>>.Clear() => Clear();
    bool ICollection<KeyValuePair<T, U>>.Contains(KeyValuePair<T, U> item) => Contains(item);
    bool IDictionary<T, U>.ContainsKey(T key) => ContainsKey(key);
    void ICollection<KeyValuePair<T, U>>.CopyTo(KeyValuePair<T, U>[] array, int arrayIndex) => CopyTo(array, arrayIndex);
    IEnumerator<KeyValuePair<T, U>> IEnumerable<KeyValuePair<T, U>>.GetEnumerator() => GetEnumerator();
    bool IDictionary<T, U>.Remove(T key) => Remove(key);
    bool ICollection<KeyValuePair<T, U>>.Remove(KeyValuePair<T, U> item) => Remove(item);
    bool IDictionary<T, U>.TryGetValue(T key, out U value) => TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }
}