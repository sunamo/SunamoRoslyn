namespace SunamoRoslyn._sunamo;

/// <summary>
/// Service providing whitespace character definitions.
/// </summary>
internal class WhitespaceCharService
{
    /// <summary>
    /// List of whitespace characters.
    /// </summary>
    internal List<char> WhiteSpaceChars = new();

    /// <summary>
    /// Unicode code points for whitespace characters.
    /// </summary>
    internal readonly List<int> WhiteSpacesCodes = new(new[]
{
9, 10, 11, 12, 13, 32, 133, 160, 5760, 6158, 8192, 8193, 8194, 8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202,
8232, 8233, 8239, 8287, 12288
});
}
