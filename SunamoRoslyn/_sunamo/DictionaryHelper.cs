namespace SunamoRoslyn._sunamo;

/// <summary>
/// Helper methods for dictionary operations.
/// </summary>
internal class DictionaryHelper
{
    /// <summary>
    /// Adds a value to the list associated with the key, creating the list if needed.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dict">The dictionary to add to.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value to add.</param>
    /// <param name="isAvoidingDuplicateValues">Whether to skip duplicate values.</param>
    /// <param name="stringDict">Optional parallel string dictionary.</param>
    internal static void AddOrCreate<TKey, TValue>(IDictionary<TKey, List<TValue>> dict, TKey key, TValue value,
        bool isAvoidingDuplicateValues = false, Dictionary<TKey, List<string>>? stringDict = null)
        where TKey : notnull
    {
        AddOrCreate<TKey, TValue, object>(dict, key, value, isAvoidingDuplicateValues, stringDict);
    }

    /// <summary>
    /// Adds a value to the list associated with the key, creating the list if needed. Supports collection keys.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <typeparam name="TCollection">The collection element type for key comparison.</typeparam>
    /// <param name="dict">The dictionary to add to.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value to add.</param>
    /// <param name="isAvoidingDuplicateValues">Whether to skip duplicate values.</param>
    /// <param name="stringDict">Optional parallel string dictionary.</param>
    internal static void AddOrCreate<TKey, TValue, TCollection>(IDictionary<TKey, List<TValue>> dict, TKey key, TValue value,
    bool isAvoidingDuplicateValues = false, Dictionary<TKey, List<string>>? stringDict = null)
        where TKey : notnull
    {
        var isComparingWithString = false;
        if (stringDict != null) isComparingWithString = true;

        if (key is IList && typeof(TCollection) != typeof(Object))
        {
            var keyElements = (IList<TCollection>)key;
            var isContained = false;

            foreach (var item in dict)
            {
                var entryKey = item.Key as IList<TCollection>;
                if (entryKey != null && entryKey.SequenceEqual(keyElements)) isContained = true;
            }

            if (isContained)
            {
                foreach (var item in dict)
                {
                    var entryKey = item.Key as IList<TCollection>;
                    if (entryKey != null && entryKey.SequenceEqual(keyElements))
                    {
                        if (isAvoidingDuplicateValues)
                            if (item.Value.Contains(value))
                                return;
                        item.Value.Add(value);
                    }
                }
            }
            else
            {
                List<TValue> valueList = new();
                valueList.Add(value);
                dict.Add(key, valueList);

                if (isComparingWithString)
                {
                    List<string> stringValueList = new();
                    stringValueList.Add(value?.ToString() ?? string.Empty);
                    stringDict!.Add(key, stringValueList);
                }
            }
        }
        else
        {
            var shouldAdd = true;
            lock (dict)
            {
                if (dict.ContainsKey(key))
                {
                    if (isAvoidingDuplicateValues)
                    {
                        if (dict[key].Contains(value))
                            shouldAdd = false;
                        else if (isComparingWithString)
                            if (stringDict![key].Contains(value?.ToString() ?? string.Empty))
                                shouldAdd = false;
                    }

                    if (shouldAdd)
                    {
                        var existingValues = dict[key];
                        if (existingValues != null) existingValues.Add(value);

                        if (isComparingWithString)
                        {
                            var existingStringValues = stringDict![key];
                            if (existingValues != null) existingStringValues.Add(value?.ToString() ?? string.Empty);
                        }
                    }
                }
                else
                {
                    if (!dict.ContainsKey(key))
                    {
                        List<TValue> valueList = new();
                        valueList.Add(value);
                        dict.Add(key, valueList);
                    }
                    else
                    {
                        dict[key].Add(value);
                    }

                    if (isComparingWithString)
                    {
                        if (!stringDict!.ContainsKey(key))
                        {
                            List<string> stringValueList = new();
                            stringValueList.Add(value?.ToString() ?? string.Empty);
                            stringDict.Add(key, stringValueList);
                        }
                        else
                        {
                            stringDict[key].Add(value?.ToString() ?? string.Empty);
                        }
                    }
                }
            }
        }
    }
}
