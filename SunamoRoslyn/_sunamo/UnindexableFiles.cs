namespace SunamoRoslyn._sunamo;

internal class UnindexableFiles
{
    internal static UnindexableFiles Instance = new UnindexableFiles();

    private UnindexableFiles()
    {
    }

    internal List<string> UnindexablePathPartsFiles { get; set; } = new List<string>();

    internal List<string> UnindexableFileNamesFiles { get; set; } = new List<string>();

    internal List<string> UnindexableFileNamesExactlyFiles { get; set; } = new List<string>();

    internal List<string> UnindexablePathEndsFiles { get; set; } = new List<string>();

    internal List<string> UnindexablePathStartsFiles { get; set; } = new List<string>();
}
