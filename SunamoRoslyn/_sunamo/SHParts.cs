namespace SunamoRoslyn._sunamo;

/// <summary>
/// String helper for extracting parts of strings.
/// </summary>
internal class SHParts
{
    /// <summary>
    /// Removes everything after the first occurrence of the separator.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <param name="separator">The separator to search for.</param>
    /// <returns>The text up to the separator.</returns>
    internal static string RemoveAfterFirst(string text, string separator)
    {
        int index = text.IndexOf(separator);
        if (index == -1 || index == text.Length - 1)
        {
            return text;
        }
        string result = text.Remove(index);
        return result;
    }
}
