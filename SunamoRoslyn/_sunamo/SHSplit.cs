namespace SunamoRoslyn._sunamo;

/// <summary>
/// String helper for split operations.
/// </summary>
internal class SHSplit
{
    /// <summary>
    /// Splits the text by the specified delimiters without removing empty entries.
    /// </summary>
    /// <param name="text">The text to split.</param>
    /// <param name="delimiters">The delimiter strings.</param>
    /// <returns>List of split parts.</returns>
    internal static List<string> SplitNone(string text, params string[] delimiters)
    {
        return text.Split(delimiters, StringSplitOptions.None).ToList();
    }
}
