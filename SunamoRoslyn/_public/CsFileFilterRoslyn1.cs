namespace SunamoRoslyn._public;

// Cannot be derived from FiltersNotTranslateAble because of easy finding of CsFileFilter instances.
public partial class CsFileFilterRoslyn
{
    /// <summary>
    /// Represents the end args type.
    /// </summary>
    public class EndArgs
    {
        /// <summary>
        /// The designer cs.
        /// </summary>
        public bool designerCs;
        /// <summary>
        /// The designer cs.
        /// </summary>
        public bool DesignerCs;
        /// <summary>
        /// The g cs.
        /// </summary>
        public bool gCs;
        /// <summary>
        /// The g i cs.
        /// </summary>
        public bool gICs;
        /// <summary>
        /// The i cs.
        /// </summary>
        public bool iCs;
        /// <summary>
        /// The not translate able.
        /// </summary>
        public bool notTranslateAble;
        /// <summary>
        /// The shared cs.
        /// </summary>
        public bool sharedCs;
        /// <summary>
        /// The tmp.
        /// </summary>
        public bool tmp;
        /// <summary>
        /// The TMP.
        /// </summary>
        public bool TMP;
        /// <summary>
        /// The xaml cs.
        /// </summary>
        public bool xamlCs;

        // False means not to index, true means to index.
        /// <summary>
        /// Initializes a new instance of the end args class.
        /// </summary>
        /// <param name="designerCs">The designer cs.</param>
        /// <param name="xamlCs">The xaml cs.</param>
        /// <param name="sharedCs">The shared cs.</param>
        /// <param name="iCs">The i cs.</param>
        /// <param name="gCs">The g cs.</param>
        /// <param name="tmp">The tmp.</param>
        /// <param name="TMP">The TMP.</param>
        /// <param name="DesignerCs">The designer cs.</param>
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

    /// <summary>
    /// Allow only.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The allow only result.</returns>
    public bool AllowOnly(string path)
    {
        return AllowOnly(path, true);
    }

    /// <summary>
    /// Allow only.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="alsoEnds">The also ends.</param>
    /// <returns>The allow only result.</returns>
    public bool AllowOnly(string path, bool alsoEnds)
    {
        var isEndMatch = true;
        return !AllowOnly(path, e!, c!, ref isEndMatch, alsoEnds);
    }

    /// <summary>
    /// Allow only contains.
    /// </summary>
    /// <param name="designerCs">The designer cs.</param>
    /// <param name="xamlCs">The xaml cs.</param>
    /// <param name="sharedCs">The shared cs.</param>
    /// <param name="iCs">The i cs.</param>
    /// <param name="gCs">The g cs.</param>
    /// <param name="tmp">The tmp.</param>
    /// <param name="TMP">The TMP.</param>
    /// <param name="DesignerCs">The designer cs.</param>
    /// <returns>The allow only contains result.</returns>
    public bool AllowOnlyContains(string path)
    {
        return !AllowOnlyContains(path, c!);
    }
}
