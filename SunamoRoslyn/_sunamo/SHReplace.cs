namespace SunamoRoslyn._sunamo;

/// <summary>
/// String helper for replacement operations.
/// </summary>
internal class SHReplace
{
    /// <summary>
    /// Replaces only the first occurrence of the search term in the input.
    /// </summary>
    /// <param name="input">The input text.</param>
    /// <param name="what">The text to find.</param>
    /// <param name="replacement">The replacement text.</param>
    /// <returns>The text with the first occurrence replaced.</returns>
    internal static string ReplaceOnce(string input, string what, string replacement)
    {
        if (what == "") return input;
        var index = input.IndexOf(what);
        if (index == -1) return input;
        return input.Substring(0, index) + replacement + input.Substring(index + what.Length);
    }
}
