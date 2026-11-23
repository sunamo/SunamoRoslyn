namespace SunamoRoslyn._public;

// Cannot be derived from FiltersNotTranslateAble because of easy finding of CsFileFilter instances.
/// <summary>
/// Represents the cs file filter roslyn type.
/// </summary>
public partial class CsFileFilterRoslyn
{
    private static bool? returnValue;
    private ContainsArgs? c;
    private EndArgs? e;

    // In default everything is false. Call a Set method to configure.
    /// <summary>
    /// Initializes a new instance of the cs file filter roslyn class.
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
    /// Get files filtered.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="searchOption">The search option.</param>
    /// <returns>The get files filtered result.</returns>
    public List<string> GetFilesFiltered(string path, string searchPattern, SearchOption searchOption)
    {
        var files = Directory.GetFiles(path, searchPattern, searchOption).ToList();
        files.RemoveAll(AllowOnly);
        files.RemoveAll(AllowOnlyContains);
        return files;
    }

    /// <summary>
    /// Allow only.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="end">The end.</param>
    /// <param name="c">The c.</param>
    /// <returns>The allow only result.</returns>
    public static bool AllowOnly(string path, EndArgs? end, ContainsArgs? c)
    {
        var isEndMatch = false;
        return AllowOnly(path, end, c, ref isEndMatch, true);
    }

    // Also for master.designer.cs and aspx.designer.cs. End and contains args can be null.
    /// <summary>
    /// Allow only.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="end">The end.</param>
    /// <param name="c">The c.</param>
    /// <param name="isEndMatch">The is end match.</param>
    /// <param name="alsoEnds">The also ends.</param>
    /// <returns>The allow only result.</returns>
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
    /// Set.
    /// </summary>
    /// <param name="endArgs">The end args.</param>
    /// <param name="c">The c.</param>
    public void Set(EndArgs endArgs, ContainsArgs c)
    {
        e = endArgs;
        this.c = c;
    }

    /// <summary>
    /// Set default.
    /// </summary>
    public void SetDefault()
    {
        e = new EndArgs(false, true, true, false, false, false, false, false);
        c = new ContainsArgs(false, false, false);
    }

    /// <summary>
    /// Get contains by flags.
    /// </summary>
    /// <param name="isNegating">The is negating.</param>
    /// <returns>The get contains by flags result.</returns>
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
    /// Get ending by flags.
    /// </summary>
    /// <param name="isNegating">The is negating.</param>
    /// <returns>The get ending by flags result.</returns>
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
    /// Allow only contains.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="c">The c.</param>
    /// <returns>The allow only contains result.</returns>
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
    /// Represents the contains type.
    /// </summary>
    public class Contains
    {
        /// <summary>
        /// The not translate able fp constant.
        /// </summary>
        public const string notTranslateAbleFp = "NotTranslateAble";

        /// <summary>
        /// The obj fp.
        /// </summary>
        public static string objFp = @"\obj\";

        /// <summary>
        /// The bin fp.
        /// </summary>
        public static string binFp = @"\bin\";

        /// <summary>
        /// The tilda RF fp.
        /// </summary>
        public static string tildaRFFp = "~RF";

        /// <summary>
        /// The unindexable path ends.
        /// </summary>
        public static List<string>? UnindexablePathEnds;

        // The list is modified to leave only unindexed entries.
        /// <summary>
        /// Fill end from file list.
        /// </summary>
        /// <param name="unindexablePathEnds">The unindexable path ends.</param>
        /// <returns>The fill end from file list result.</returns>
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
    /// Represents the contains args type.
    /// </summary>
    public class ContainsArgs
    {
        // False means not to index, true means to index.
        /// <summary>
        /// The bin fp.
        /// </summary>
        public bool binFp;

        /// <summary>
        /// The obj fp.
        /// </summary>
        public bool objFp;

        /// <summary>
        /// The tilda RF.
        /// </summary>
        public bool tildaRF;

        /// <summary>
        /// Contains args.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>The contains args result.</returns>
        public ContainsArgs(bool objFp, bool binFp, bool tildaRF)
        {
            this.objFp = objFp;
            this.binFp = binFp;
            this.tildaRF = tildaRF;
        }
    }

    /// <summary>
    /// Represents the end type.
    /// </summary>
    public class End
    {
        /// <summary>
        /// The not translate able pp constant.
        /// </summary>
        public const string NotTranslateAblePp = "NotTranslateAble";
        /// <summary>
        /// The designer cs pp constant.
        /// </summary>
        public const string designerCsPp = ".designer.cs";
        /// <summary>
        /// The designer cs pp constant.
        /// </summary>
        public const string DesignerCsPp = ".Designer.cs";
        /// <summary>
        /// The xaml cs pp constant.
        /// </summary>
        public const string xamlCsPp = ".xaml.cs";
        /// <summary>
        /// The shared cs pp constant.
        /// </summary>
        public const string sharedCsPp = "Shared.cs";
        /// <summary>
        /// The i cs pp constant.
        /// </summary>
        public const string iCsPp = ".i.cs";
        /// <summary>
        /// The g i cs pp constant.
        /// </summary>
        public const string gICsPp = ".g.i.cs";
        /// <summary>
        /// The g cs pp constant.
        /// </summary>
        public const string gCsPp = ".g.cs";
        /// <summary>
        /// The tmp pp constant.
        /// </summary>
        public const string tmpPp = ".tmp";
        /// <summary>
        /// The TMP pp constant.
        /// </summary>
        public const string TMPPp = ".TMP";

        /// <summary>
        /// The unindexable path ends.
        /// </summary>
        public static List<string>? UnindexablePathEnds;

        // The list is modified to leave only unindexed entries.
        /// <summary>
        /// Initializes a new instance of the fill end from file list class.
        /// </summary>
        /// <param name="objFp">The obj fp.</param>
        /// <param name="binFp">The bin fp.</param>
        /// <param name="tildaRF">The tilda RF.</param>
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
