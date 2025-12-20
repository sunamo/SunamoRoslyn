// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn._public;
/// <summary>
///     Cant be derived from FiltersNotTranslateAble because easy of finding instances of CsFileFilter
/// </summary>
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
        /// <summary>
        ///     false which not to index, true which to index
        /// </summary>
        /// <param name = "designerCs"></param>
        /// <param name = "xamlCs"></param>
        /// <param name = "sharedCs"></param>
        /// <param name = "iCs"></param>
        /// <param name = "gICs"></param>
        /// <param name = "gCs"></param>
        /// <param name = "tmp"></param>
        /// <param name = "TMP"></param>
        /// <param name = "DesignerCs"></param>
        public EndArgs(bool designerCs, bool xamlCs, bool sharedCs, bool iCs, /*bool gICs,*/ bool gCs, bool tmp, bool TMP, bool DesignerCs)
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

    public bool AllowOnly(string item)
    {
        return AllowOnly(item, true);
    }

    public bool AllowOnly(string item, bool alsoEnds)
    {
        var end2 = true;
        return !AllowOnly(item, e, c, ref end2, alsoEnds);
    }

    public bool AllowOnlyContains(string i)
    {
        return !AllowOnlyContains(i, c);
    }
}