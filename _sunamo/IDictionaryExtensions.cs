namespace SunamoRoslyn._sunamo;

internal static class IDictionaryExtensions
{
    internal static void AddIfNotExists<T, U>(this IDictionary<T, U> d, T t, U u)
    {
        if (!d.ContainsKey(t)) d.Add(t, u);
    }
}