namespace SunamoRoslyn._public;

// Cannot be derived from FiltersNotTranslateAble because of easy finding of CsFileFilter instances.
public partial class CsFileFilterRoslyn
{
    public class EndArgs
    {
        public bool designerCs;
        public bool DesignerCs;
        public bool gCs;
        public bool gICs;
        public bool iCs;
        public bool notTranslateAble;
        public bool sharedCs;
        public bool tmp;
        public bool TMP;
        public bool xamlCs;

        // False means not to index, true means to index.
        public EndArgs(bool designerCs, bool xamlCs, bool sharedCs, bool iCs, bool gCs, bool tmp, bool TMP, bool DesignerCs)
        {
            this.designerCs = designerCs;
            this.xamlCs = xamlCs;
            this.sharedCs = sharedCs;
            this.iCs = iCs;
            this.gCs = gCs;
            this.tmp = tmp;
            this.TMP = TMP;
            this.DesignerCs = DesignerCs;
        }
    }

    public bool AllowOnly(string path)
    {
        return AllowOnly(path, true);
    }

    public bool AllowOnly(string path, bool alsoEnds)
    {
        var isEndMatch = true;
        return !AllowOnly(path, e!, c!, ref isEndMatch, alsoEnds);
    }

    public bool AllowOnlyContains(string path)
    {
        return !AllowOnlyContains(path, c!);
    }
}
