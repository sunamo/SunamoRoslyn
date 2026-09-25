namespace SunamoRoslyn._sunamo;

/// <summary>
/// Extension methods for IDictionary.
/// </summary>
internal static class IDictionaryExtensions
{
    /// <summary>
    /// Adds the key-value pair only if the key does not already exist.
    /// </summary>
    /// <typeparam name="T">The key type.</typeparam>
    /// <typeparam name="U">The value type.</typeparam>
    /// <param name="dictionary">The dictionary to add to.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    internal static void AddIfNotExists<T, U>(this IDictionary<T, U> dictionary, T key, U value)
    {
        if (!dictionary.ContainsKey(key)) dictionary.Add(key, value);
    }
}
