namespace SunamoRoslyn._sunamo;

/// <summary>
/// Singleton containing lists of file patterns that should not be indexed.
/// </summary>
internal class UnindexableFiles
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    internal static UnindexableFiles Instance = new UnindexableFiles();

    private UnindexableFiles()
    {
    }

    /// <summary>
    /// Path parts that make a file unindexable.
    /// </summary>
    internal List<string> UnindexablePathPartsFiles { get; set; } = new List<string>();

    /// <summary>
    /// File names that make a file unindexable.
    /// </summary>
    internal List<string> UnindexableFileNamesFiles { get; set; } = new List<string>();

    /// <summary>
    /// Exact file names that make a file unindexable.
    /// </summary>
    internal List<string> UnindexableFileNamesExactlyFiles { get; set; } = new List<string>();

    /// <summary>
    /// Path endings that make a file unindexable.
    /// </summary>
    internal List<string> UnindexablePathEndsFiles { get; set; } = new List<string>();

    /// <summary>
    /// Path starts that make a file unindexable.
    /// </summary>
    internal List<string> UnindexablePathStartsFiles { get; set; } = new List<string>();
}
