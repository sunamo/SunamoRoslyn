namespace SunamoRoslyn._public;

/// <summary>
/// Filters C# source files based on path endings and contained substrings.
/// Cannot be derived from FiltersNotTranslateAble because of easy finding of CsFileFilter instances.
/// </summary>
public partial class CsFileFilterRoslyn
{
    private static bool? returnValue;
    private ContainsArgs? c;
    private EndArgs? e;

    /// <summary>
    /// Initializes a new instance of the <see cref="CsFileFilterRoslyn"/> class.
    /// In default everything is false. Call a Set method to configure.
    /// </summary>
    public CsFileFilterRoslyn()
    {
    }

    private static bool? ReturnValue
    {
        get => returnValue;
        set
        {
            returnValue = value;
        }
    }

    /// <summary>
    /// Gets files matching the search pattern and filters them.
    /// </summary>
    /// <param name="path">The directory path to search in.</param>
    /// <param name="searchPattern">The file search pattern.</param>
    /// <param name="searchOption">The search option (top directory or all subdirectories).</param>
    /// <returns>A filtered list of file paths.</returns>
    public List<string> GetFilesFiltered(string path, string searchPattern, SearchOption searchOption)
    {
        var files = Directory.GetFiles(path, searchPattern, searchOption).ToList();
        files.RemoveAll(AllowOnly);
        files.RemoveAll(AllowOnlyContains);
        return files;
    }

    /// <summary>
    /// Determines whether a file path is allowed based on ending and contains filters.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <param name="end">The ending filter arguments.</param>
    /// <param name="c">The contains filter arguments.</param>
    /// <returns>True if the file is allowed; otherwise false.</returns>
    public static bool AllowOnly(string path, EndArgs? end, ContainsArgs? c)
    {
        var isEndMatch = false;
        return AllowOnly(path, end, c, ref isEndMatch, true);
    }

    /// <summary>
    /// Determines whether a file path is allowed based on ending and contains filters.
    /// Also for master.designer.cs and aspx.designer.cs. End and contains args can be null.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <param name="end">The ending filter arguments.</param>
    /// <param name="c">The contains filter arguments.</param>
    /// <param name="isEndMatch">Set to true if the path was rejected by an ending filter.</param>
    /// <param name="alsoEnds">Whether to also check ending filters.</param>
    /// <returns>True if the file is allowed; otherwise false.</returns>
    public static bool AllowOnly(string path, EndArgs? end, ContainsArgs? c, ref bool isEndMatch, bool alsoEnds)
    {
        ReturnValue = null;
        if (alsoEnds && end != null)
        {
            isEndMatch = true;
            if (!end.designerCs && path.EndsWith(End.designerCsPp))
                ReturnValue = false;
            if (!end.xamlCs && path.EndsWith(End.xamlCsPp))
                ReturnValue = false;
            if (!end.sharedCs && path.EndsWith(End.sharedCsPp))
                ReturnValue = false;
            if (!end.iCs && path.EndsWith(End.iCsPp))
                ReturnValue = false;
            if (!end.gICs && path.EndsWith(End.gICsPp))
                ReturnValue = false;
            if (!end.gCs && path.EndsWith(End.gCsPp))
                ReturnValue = false;
            if (!end.tmp && path.EndsWith(End.tmpPp))
                ReturnValue = false;
            if (!end.TMP && path.EndsWith(End.TMPPp))
                ReturnValue = false;
            if (!end.DesignerCs && path.EndsWith(End.DesignerCsPp))
                ReturnValue = false;
            if (!end.notTranslateAble && path.EndsWith(End.NotTranslateAblePp))
                ReturnValue = false;
        }

        if (ReturnValue.HasValue)
            return ReturnValue.Value;
        isEndMatch = false;
        if (c != null)
        {
            if (!c.binFp && path.Contains(Contains.binFp))
                ReturnValue = false;
            if (!c.objFp && path.Contains(Contains.objFp))
                ReturnValue = false;
            if (!c.tildaRF && path.Contains(Contains.tildaRFFp))
                ReturnValue = false;
        }

        if (ReturnValue.HasValue)
            return ReturnValue.Value;
        return true;
    }

    /// <summary>
    /// Sets the ending and contains filter arguments.
    /// </summary>
    /// <param name="endArgs">The ending filter arguments.</param>
    /// <param name="c">The contains filter arguments.</param>
    public void Set(EndArgs endArgs, ContainsArgs c)
    {
        e = endArgs;
        this.c = c;
    }

    /// <summary>
    /// Sets the default filter configuration.
    /// </summary>
    public void SetDefault()
    {
        e = new EndArgs(false, true, true, false, false, false, false, false);
        c = new ContainsArgs(false, false, false);
    }

    /// <summary>
    /// Gets the contains filter strings matching the current flags.
    /// </summary>
    /// <param name="isNegating">Whether to negate the flag check.</param>
    /// <returns>A list of contains filter strings.</returns>
    public List<string> GetContainsByFlags(bool isNegating)
    {
        var list = new List<string>();
        if (BTS.Is(c!.binFp, isNegating))
            list.Add(Contains.binFp);
        if (BTS.Is(c.objFp, isNegating))
            list.Add(Contains.objFp);
        if (BTS.Is(c.tildaRF, isNegating))
            list.Add(Contains.tildaRFFp);
        return list;
    }

    /// <summary>
    /// Gets the ending filter strings matching the current flags.
    /// </summary>
    /// <param name="isNegating">Whether to negate the flag check.</param>
    /// <returns>A list of ending filter strings.</returns>
    public List<string> GetEndingByFlags(bool isNegating)
    {
        var list = new List<string>();
        if (Is(e!.designerCs, isNegating))
            list.Add(End.designerCsPp);
        if (Is(e.xamlCs, isNegating))
            list.Add(End.xamlCsPp);
        if (Is(e.xamlCs, isNegating))
            list.Add(End.xamlCsPp);
        if (Is(e.sharedCs, isNegating))
            list.Add(End.sharedCsPp);
        if (Is(e.iCs, isNegating))
            list.Add(End.iCsPp);
        if (Is(e.gICs, isNegating))
            list.Add(End.gICsPp);
        if (Is(e.gCs, isNegating))
            list.Add(End.gCsPp);
        if (Is(e.tmp, isNegating))
            list.Add(End.tmpPp);
        if (Is(e.TMP, isNegating))
            list.Add(End.TMPPp);
        if (Is(e.DesignerCs, isNegating))
            list.Add(End.DesignerCsPp);
        if (Is(e.notTranslateAble, isNegating))
            list.Add("NotTranslateAble");
        return list;
    }

    private bool Is(bool value, bool isNegating)
    {
        return BTS.Is(value, isNegating);
    }

    /// <summary>
    /// Checks whether a file path is allowed based on contains filters.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <param name="c">The contains filter arguments.</param>
    /// <returns>True if the file is allowed; otherwise false.</returns>
    public static bool AllowOnlyContains(string path, ContainsArgs c)
    {
        if (!c.objFp && path.Contains(@"\obj\"))
            return false;
        if (!c.binFp && path.Contains(@"\bin\"))
            return false;
        if (!c.tildaRF && path.Contains(@"RF~"))
            return false;
        return true;
    }

    /// <summary>
    /// Provides contains-based path filtering constants and methods.
    /// </summary>
    public class Contains
    {
        /// <summary>
        /// The not-translatable file path filter.
        /// </summary>
        public const string notTranslateAbleFp = "NotTranslateAble";

        /// <summary>
        /// The obj directory path filter.
        /// </summary>
        public static string objFp = @"\obj\";

        /// <summary>
        /// The bin directory path filter.
        /// </summary>
        public static string binFp = @"\bin\";

        /// <summary>
        /// The tilda RF path filter.
        /// </summary>
        public static string tildaRFFp = "~RF";

        /// <summary>
        /// List of unindexable path endings used for filtering.
        /// </summary>
        public static List<string>? UnindexablePathEnds;

        /// <summary>
        /// Fills a <see cref="ContainsArgs"/> from a list of unindexable path endings.
        /// The list is modified to leave only unindexed entries.
        /// </summary>
        /// <param name="unindexablePathEnds">The list of unindexable path endings.</param>
        /// <returns>A configured <see cref="ContainsArgs"/> instance.</returns>
        public static ContainsArgs FillEndFromFileList(List<string> unindexablePathEnds)
        {
            UnindexablePathEnds = unindexablePathEnds;
            var ea = new ContainsArgs(ContainsInList(objFp), ContainsInList(binFp), ContainsInList(tildaRFFp));
            return ea;
        }

        private static bool ContainsInList(string pattern)
        {
            return UnindexablePathEnds!.Contains(pattern);
        }
    }

    /// <summary>
    /// Arguments for contains-based file path filtering.
    /// </summary>
    public class ContainsArgs
    {
        /// <summary>
        /// Whether to include paths containing the bin directory.
        /// </summary>
        public bool binFp;

        /// <summary>
        /// Whether to include paths containing the obj directory.
        /// </summary>
        public bool objFp;

        /// <summary>
        /// Whether to include paths containing the tilda RF pattern.
        /// </summary>
        public bool tildaRF;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainsArgs"/> class.
        /// False means not to index, true means to index.
        /// </summary>
        /// <param name="objFp">Whether to include obj directory paths.</param>
        /// <param name="binFp">Whether to include bin directory paths.</param>
        /// <param name="tildaRF">Whether to include tilda RF paths.</param>
        public ContainsArgs(bool objFp, bool binFp, bool tildaRF)
        {
            this.objFp = objFp;
            this.binFp = binFp;
            this.tildaRF = tildaRF;
        }
    }

    /// <summary>
    /// Provides ending-based path filtering constants and methods.
    /// </summary>
    public class End
    {
        /// <summary>The not-translatable path ending.</summary>
        public const string NotTranslateAblePp = "NotTranslateAble";
        /// <summary>The .designer.cs path ending.</summary>
        public const string designerCsPp = ".designer.cs";
        /// <summary>The .Designer.cs path ending.</summary>
        public const string DesignerCsPp = ".Designer.cs";
        /// <summary>The .xaml.cs path ending.</summary>
        public const string xamlCsPp = ".xaml.cs";
        /// <summary>The Shared.cs path ending.</summary>
        public const string sharedCsPp = "Shared.cs";
        /// <summary>The .i.cs path ending.</summary>
        public const string iCsPp = ".i.cs";
        /// <summary>The .g.i.cs path ending.</summary>
        public const string gICsPp = ".g.i.cs";
        /// <summary>The .g.cs path ending.</summary>
        public const string gCsPp = ".g.cs";
        /// <summary>The .tmp path ending.</summary>
        public const string tmpPp = ".tmp";
        /// <summary>The .TMP path ending.</summary>
        public const string TMPPp = ".TMP";

        /// <summary>
        /// List of unindexable path endings used for filtering.
        /// </summary>
        public static List<string>? UnindexablePathEnds;

        /// <summary>
        /// Fills an <see cref="EndArgs"/> from a list of unindexable path endings.
        /// The list is modified to leave only unindexed entries.
        /// </summary>
        /// <param name="unindexablePathEnds">The list of unindexable path endings.</param>
        /// <returns>A configured <see cref="EndArgs"/> instance.</returns>
        public static EndArgs FillEndFromFileList(List<string> unindexablePathEnds)
        {
            UnindexablePathEnds = unindexablePathEnds;
            var xValue = ContainsAndRemove(xamlCsPp);
            var ea = new EndArgs(ContainsAndRemove(designerCsPp), xValue, ContainsAndRemove(sharedCsPp), ContainsAndRemove(iCsPp), ContainsAndRemove(gCsPp), ContainsAndRemove(tmpPp), ContainsAndRemove(TMPPp), ContainsAndRemove(DesignerCsPp));
            return ea;
        }

        private static bool ContainsAndRemove(string pattern)
        {
            if (UnindexablePathEnds!.Contains(pattern))
            {
                UnindexablePathEnds.Remove(pattern);
                return false;
            }

            return true;
        }
    }
}
