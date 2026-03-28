namespace SunamoRoslyn._sunamo;

/// <summary>
/// Extension methods for StringBuilder.
/// </summary>
internal static class StringBuilderExtensions
{
    /// <summary>
    /// Appends text followed by a postfix to the StringBuilder.
    /// </summary>
    /// <param name="stringBuilder">The StringBuilder instance.</param>
    /// <param name="postfix">The postfix to append after the text.</param>
    /// <param name="text">The text to append.</param>
    internal static void AddItem(this StringBuilder stringBuilder, string postfix, string text)
    {
        stringBuilder.Append(text + postfix);
    }
}
