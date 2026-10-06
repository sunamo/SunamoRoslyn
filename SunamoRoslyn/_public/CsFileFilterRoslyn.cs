namespace SunamoRoslyn._public;

// Cannot be derived from FiltersNotTranslateAble because of easy finding of CsFileFilter instances.
public partial class CsFileFilterRoslyn
{
    private static bool? returnValue;
    private ContainsArgs? c;
    private EndArgs? e;

    // In default everything is false. Call a Set method to configure.
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

    public List<string> GetFilesFiltered(string path, string searchPattern, SearchOption searchOption)
    {
        var files = Directory.GetFiles(path, searchPattern, searchOption).ToList();
        files.RemoveAll(AllowOnly);
        files.RemoveAll(AllowOnlyContains);
        return files;
    }

    public static bool AllowOnly(string path, EndArgs? end, ContainsArgs? c)
    {
        var isEndMatch = false;
        return AllowOnly(path, end, c, ref isEndMatch, true);
    }

    // Also for master.designer.cs and aspx.designer.cs. End and contains args can be null.
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

    public void Set(EndArgs endArgs, ContainsArgs c)
    {
        e = endArgs;
        this.c = c;
    }

    public void SetDefault()
    {
        e = new EndArgs(false, true, true, false, false, false, false, false);
        c = new ContainsArgs(false, false, false);
    }

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

    public class Contains
    {
        public const string notTranslateAbleFp = "NotTranslateAble";

        public static string objFp = @"\obj\";

        public static string binFp = @"\bin\";

        public static string tildaRFFp = "~RF";

        public static List<string>? UnindexablePathEnds;

        // The list is modified to leave only unindexed entries.
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

    public class ContainsArgs
    {
        // False means not to index, true means to index.
        public bool binFp;

        public bool objFp;

        public bool tildaRF;

        public ContainsArgs(bool objFp, bool binFp, bool tildaRF)
        {
            this.objFp = objFp;
            this.binFp = binFp;
            this.tildaRF = tildaRF;
        }
    }

    public class End
    {
        public const string NotTranslateAblePp = "NotTranslateAble";
        public const string designerCsPp = ".designer.cs";
        public const string DesignerCsPp = ".Designer.cs";
        public const string xamlCsPp = ".xaml.cs";
        public const string sharedCsPp = "Shared.cs";
        public const string iCsPp = ".i.cs";
        public const string gICsPp = ".g.i.cs";
        public const string gCsPp = ".g.cs";
        public const string tmpPp = ".tmp";
        public const string TMPPp = ".TMP";

        public static List<string>? UnindexablePathEnds;

        // The list is modified to leave only unindexed entries.
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
