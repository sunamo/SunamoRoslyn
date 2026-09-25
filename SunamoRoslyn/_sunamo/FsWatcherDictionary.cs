namespace SunamoRoslyn._sunamo;

// Dictionary implementation for file system watcher operations. Must be internal due to SourceCodeIndexerRoslyn usage.
internal class FsWatcherDictionary<T, U> : IDictionary<T, U>
    where T : notnull
{
    private readonly Dictionary<T, U> dictionary = new();

    internal U? this[T key]
    {
        get
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            return default;
        }
        set => dictionary[key] = value!;
    }

    internal ICollection<T> Keys => dictionary.Keys;
    internal ICollection<U> Values => dictionary.Values;
    internal int Count => dictionary.Count;
    internal bool IsReadOnly => false;

    internal void Add(T key, U value)
    {
        lock (dictionary)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, value);
        }
    }

    internal void Add(KeyValuePair<T, U> keyValuePair)
    {
        Add(keyValuePair.Key, keyValuePair.Value);
    }

    internal void Clear()
    {
        dictionary.Clear();
    }

    internal bool Contains(KeyValuePair<T, U> keyValuePair) => dictionary.Contains(keyValuePair);

    internal bool ContainsKey(T key) => dictionary.ContainsKey(key);

    internal void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex)
    {
        if (array is null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < dictionary.Count)
            throw new ArgumentException("Array is too small");

        ((ICollection<KeyValuePair<T, U>>)dictionary).CopyTo(array, arrayIndex);
    }

    internal IEnumerator<KeyValuePair<T, U>> GetEnumerator() => dictionary.GetEnumerator();

    internal bool Remove(T key) => dictionary.Remove(key);

    internal bool Remove(KeyValuePair<T, U> keyValuePair) => dictionary.Remove(keyValuePair.Key);

    internal bool TryGetValue(T key, out U value) => dictionary.TryGetValue(key, out value!);

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

    IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();
}
