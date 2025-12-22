namespace SunamoRoslyn._sunamo;

internal class UnindexableFiles
{
    internal static UnindexableFiles uf = new UnindexableFiles();
    private UnindexableFiles()
    {
    }
    internal List<string> unindexablePathPartsFiles = new List<string>();
    internal List<string> unindexableFileNamesFiles = new List<string>();
    internal List<string> unindexableFileNamesExactlyFiles = new List<string>();
    internal List<string> unindexablePathEndsFiles = new List<string>();
    internal List<string> unindexablePathStartsFiles = new List<string>();
}