namespace SunamoRoslyn._sunamo;

/// <summary>
/// Regex patterns used across the project.
/// </summary>
internal class RegexHelper
{
    /// <summary>
    /// Case-insensitive regex matching br HTML tags.
    /// </summary>
    internal static Regex BrTagCaseInsensitiveRegex = new Regex(@"<br\s*/?>");
}
