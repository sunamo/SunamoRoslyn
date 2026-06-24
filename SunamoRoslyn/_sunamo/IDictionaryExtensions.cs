namespace SunamoRoslyn._sunamo;

internal static class IDictionaryExtensions
{
    internal static void AddIfNotExists<T, U>(this IDictionary<T, U> dictionary, T key, U value)
    {
        if (!dictionary.ContainsKey(key)) dictionary.Add(key, value);
    }
}
