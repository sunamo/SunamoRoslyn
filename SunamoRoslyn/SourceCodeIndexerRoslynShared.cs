namespace SunamoRoslyn;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class SourceCodeIndexerRoslyn
{
    /// <summary>
    /// used only in FileSystemWatcher
    /// </summary>
    /// <param name = "file"></param>
    /// <param name = "fromFileSystemWatcher"></param>
    public 
#if ASYNC
            async Task ProcessFileAsync
#else
    void ProcessFile
#endif
    (string file, bool fromFileSystemWatcher)
    {
#if ASYNC
        await
#endif
        ProcessFile(file, NamespaceCodeElementsType.All, ClassCodeElementsType.All, false, fromFileSystemWatcher);
    }

    public bool IsToIndexedFolder(string pathFile, bool alsoEnds)
    {
        var uf = UnindexableFiles.uf;
        bool end2 = false;
        if (!CsFileFilterRoslyn.AllowOnly(pathFile, endArgs, containsArgs, ref end2, alsoEnds))
        {
            if (end2)
            {
                uf.unindexablePathEndsFiles.Add(pathFile);
            }
            else
            {
                uf.unindexablePathPartsFiles.Add(pathFile);
            }

            return false;
        }

        if (CA.StartWith(pathStarts, pathFile) != null)
        {
            uf.unindexablePathStartsFiles.Add(pathFile);
            return false;
        }

        return true;
    }

    public bool IsToIndexed(string pathFile)
    {
#region All 4 for which is checked
        if (CA.ReturnWhichContainsIndexes(endsOther, pathFile /*, SearchStrategyRoslyn.FixedSpace*/).Count > 0)
        {
            return false;
        }

        var uf = UnindexableFiles.uf;
        var fn = Path.GetFileName(pathFile);
        if (CA.ReturnWhichContainsIndexes(fileNames, fn /*, SearchStrategyRoslyn.FixedSpace*/).Count > 0)
        {
            uf.unindexableFileNamesFiles.Add(pathFile);
            return false;
        }

#endregion
        return IsToIndexedFolder(pathFile, true);
    }

    public bool isCallingIsToIndexed = false;
    internal static List<T> GetValues<T>()
        where T : struct
    {
        return GetValues<T>(false, true);
    }

    /// <summary>
    /// Get all values expect of Nope/None
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "type"></param>
    internal static List<T> GetValues<T>(bool IncludeNope, bool IncludeShared)
        where T : struct
    {
        var type = typeof(T);
        var values = Enum.GetValues(type).Cast<T>().ToList();
        T nope;
        if (!IncludeNope)
        {
            if (Enum.TryParse<T>(CodeElementsConstants.NopeValue, out nope))
            {
                values.Remove(nope);
            }
        }

        if (!IncludeShared)
        {
            if (type.Name == "MySites")
            {
                if (Enum.TryParse<T>("Shared", out nope))
                {
                    values.Remove(nope);
                }
            }
            else
            {
                if (Enum.TryParse<T>("Sha", out nope))
                {
                    values.Remove(nope);
                }
            }
        }

        if (Enum.TryParse<T>(CodeElementsConstants.NoneValue, out nope))
        {
            values.Remove(nope);
        }

        return values;
    }
}