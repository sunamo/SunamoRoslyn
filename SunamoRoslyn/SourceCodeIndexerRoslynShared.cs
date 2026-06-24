namespace SunamoRoslyn;

public partial class SourceCodeIndexerRoslyn
{
    public
            async Task ProcessFileAsync
    (string file, bool isFromFileSystemWatcher)
    {
        await
        ProcessFile(file, NamespaceCodeElementsType.All, ClassCodeElementsType.All, false, isFromFileSystemWatcher);
    }

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

    public bool IsCallingIsToIndexed { get; set; } = false;

    internal static List<T> GetValues<T>()
        where T : struct
    {
        return GetValues<T>(false, true);
    }

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
