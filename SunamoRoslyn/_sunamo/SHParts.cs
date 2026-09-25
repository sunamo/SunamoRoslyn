namespace SunamoRoslyn._sunamo;

internal class SHParts
{
    internal static string RemoveAfterFirst(string text, string separator)
    {
        var index = text.IndexOf(separator);
        if (index == -1 || index == text.Length - 1)
        {
            return text;
        }
        return text.Remove(index);
    }
}
