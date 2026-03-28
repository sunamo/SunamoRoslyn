namespace SunamoRoslyn;

/// <summary>
/// Shared methods for SourceCodeIndexerRoslyn including file filtering and enum value helpers.
/// </summary>
public partial class SourceCodeIndexerRoslyn
{
    /// <summary>
    /// Processes a file triggered by a FileSystemWatcher event.
    /// </summary>
    /// <param name="file">Full path to the source file.</param>
    /// <param name="isFromFileSystemWatcher">Whether the call originates from a file system watcher event.</param>
    public
#if ASYNC
            async Task ProcessFileAsync
#else
    void ProcessFile
#endif
    (string file, bool isFromFileSystemWatcher)
    {
#if ASYNC
        await
#endif
        ProcessFile(file, NamespaceCodeElementsType.All, ClassCodeElementsType.All, false, isFromFileSystemWatcher);
    }

    /// <summary>
    /// Determines whether a file's folder path should be indexed based on filter rules.
    /// </summary>
    /// <param name="pathFile">Full path to the file.</param>
    /// <param name="alsoEnds">Whether to also check path endings.</param>
    /// <returns>True if the file's folder is indexable.</returns>
    public bool IsToIndexedFolder(string pathFile, bool alsoEnds)
    {
        var unindexableFiles = UnindexableFiles.Instance;
        bool isEndMatch = false;
        if (!CsFileFilterRoslyn.AllowOnly(pathFile, EndArgs, ContainsArgs, ref isEndMatch, alsoEnds))
        {
            if (isEndMatch)
            {
                unindexableFiles.UnindexablePathEndsFiles.Add(pathFile);
            }
            else
            {
                unindexableFiles.UnindexablePathPartsFiles.Add(pathFile);
            }

            return false;
        }

        if (PathStarts != null && CA.StartWith(PathStarts, pathFile) != null)
        {
            unindexableFiles.UnindexablePathStartsFiles.Add(pathFile);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether a file should be indexed based on all filter criteria.
    /// </summary>
    /// <param name="pathFile">Full path to the file.</param>
    /// <returns>True if the file should be indexed.</returns>
    public bool IsToIndexed(string pathFile)
    {
#region All 4 for which is checked
        if (EndsOther != null && CA.ReturnWhichContainsIndexes(EndsOther, pathFile).Count > 0)
        {
            return false;
        }

        var unindexableFiles = UnindexableFiles.Instance;
        var fileName = Path.GetFileName(pathFile);
        if (FileNames != null && CA.ReturnWhichContainsIndexes(FileNames, fileName).Count > 0)
        {
            unindexableFiles.UnindexableFileNamesFiles.Add(pathFile);
            return false;
        }

#endregion
        return IsToIndexedFolder(pathFile, true);
    }

    /// <summary>
    /// Whether IsToIndexed is currently being called (prevents recursive checks).
    /// </summary>
    public bool IsCallingIsToIndexed { get; set; } = false;

    /// <summary>
    /// Gets all enum values excluding Nope/None.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>List of enum values without Nope/None.</returns>
    internal static List<T> GetValues<T>()
        where T : struct
    {
        return GetValues<T>(false, true);
    }

    /// <summary>
    /// Gets enum values with optional inclusion of Nope and Shared values.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="isIncludingNope">Whether to include the Nope value.</param>
    /// <param name="isIncludingShared">Whether to include the Shared value.</param>
    /// <returns>Filtered list of enum values.</returns>
    internal static List<T> GetValues<T>(bool isIncludingNope, bool isIncludingShared)
        where T : struct
    {
        var enumType = typeof(T);
        var values = Enum.GetValues(enumType).Cast<T>().ToList();
        T parsedValue;
        if (!isIncludingNope)
        {
            if (Enum.TryParse<T>(CodeElementsConstants.NopeValue, out parsedValue))
            {
                values.Remove(parsedValue);
            }
        }

        if (!isIncludingShared)
        {
            if (enumType.Name == "MySites")
            {
                if (Enum.TryParse<T>("Shared", out parsedValue))
                {
                    values.Remove(parsedValue);
                }
            }
            else
            {
                if (Enum.TryParse<T>("Sha", out parsedValue))
                {
                    values.Remove(parsedValue);
                }
            }
        }

        if (Enum.TryParse<T>(CodeElementsConstants.NoneValue, out parsedValue))
        {
            values.Remove(parsedValue);
        }

        return values;
    }
}
