namespace SunamoRoslyn._sunamo;

/// <summary>
/// File system path helper.
/// </summary>
internal class FS
{
    /// <summary>
    /// Ensures the path ends with a backslash and capitalizes the first character.
    /// </summary>
    /// <param name="path">The path to modify.</param>
    /// <returns>The modified path.</returns>
    internal static string WithEndSlash(ref string path)
    {
        if (path != string.Empty)
        {
            path = path.TrimEnd('\\') + '\\';
        }
        SH.FirstCharUpper(ref path);
        return path;
    }
}
