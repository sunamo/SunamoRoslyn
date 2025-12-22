namespace SunamoRoslyn._sunamo;

/// <summary>
/// Nemůžu dědit protože vše tu musí být internal
/// Ale jinak musí být internal kvůli SourceCodeIndexerRoslyn
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
internal class FsWatcherDictionary<T, U> : IDictionary<T, U>
{
    private readonly Dictionary<T, U> d = new();

    internal U this[T key]
    {
        get
        {
            if (d.ContainsKey(key)) return d[key];
            return default;
        }
        set => d[key] = value;
    }

    internal ICollection<T> Keys => d.Keys;
    internal ICollection<U> Values => d.Values;
    internal int Count => d.Count;
    internal bool IsReadOnly => false;

    internal void Add(T key, U value)
    {
        lock (d)
        {
            if (!d.ContainsKey(key)) d.Add(key, value);
        }
    }

    internal void Add(KeyValuePair<T, U> item)
    {
        Add(item.Key, item.Value);
    }

    internal void Clear()
    {
        d.Clear();
    }

    internal bool Contains(KeyValuePair<T, U> item)
    {
        return d.Contains(item);
    }

    internal bool ContainsKey(T key)
    {
        return d.ContainsKey(key);
    }

    internal void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < d.Count)
            throw new ArgumentException("Array is too small");

        ((ICollection<KeyValuePair<T, U>>)d).CopyTo(array, arrayIndex);
    }

    internal IEnumerator<KeyValuePair<T, U>> GetEnumerator()
    {
        return d.GetEnumerator();
    }

    internal bool Remove(T key)
    {
        return d.Remove(key);
    }

    internal bool Remove(KeyValuePair<T, U> item)
    {
        return d.Remove(item.Key);
    }

    internal bool TryGetValue(T key, out U value)
    {
        var vr = d.TryGetValue(key, out value);
        return vr;
    }

    // Explicit interface implementations
    U IDictionary<T, U>.this[T key]
    {
        get => this[key];
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
        return d.GetEnumerator();
    }
}