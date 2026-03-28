namespace SunamoRoslyn._public;

/// <summary>
/// Partial class providing additional filter methods for C# source files.
/// Cannot be derived from FiltersNotTranslateAble because of easy finding of CsFileFilter instances.
/// </summary>
public partial class CsFileFilterRoslyn
{
    /// <summary>
    /// Arguments for ending-based file path filtering.
    /// </summary>
    public class EndArgs
    {
        /// <summary>Whether to include .designer.cs files.</summary>
        public bool designerCs;
        /// <summary>Whether to include .Designer.cs files.</summary>
        public bool DesignerCs;
        /// <summary>Whether to include .g.cs files.</summary>
        public bool gCs;
        /// <summary>Whether to include .g.i.cs files.</summary>
        public bool gICs;
        /// <summary>Whether to include .i.cs files.</summary>
        public bool iCs;
        /// <summary>Whether to include NotTranslateAble files.</summary>
        public bool notTranslateAble;
        /// <summary>Whether to include Shared.cs files.</summary>
        public bool sharedCs;
        /// <summary>Whether to include .tmp files.</summary>
        public bool tmp;
        /// <summary>Whether to include .TMP files.</summary>
        public bool TMP;
        /// <summary>Whether to include .xaml.cs files.</summary>
        public bool xamlCs;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndArgs"/> class.
        /// False means not to index, true means to index.
        /// </summary>
        /// <param name="designerCs">Whether to include .designer.cs files.</param>
        /// <param name="xamlCs">Whether to include .xaml.cs files.</param>
        /// <param name="sharedCs">Whether to include Shared.cs files.</param>
        /// <param name="iCs">Whether to include .i.cs files.</param>
        /// <param name="gCs">Whether to include .g.cs files.</param>
        /// <param name="tmp">Whether to include .tmp files.</param>
        /// <param name="TMP">Whether to include .TMP files.</param>
        /// <param name="DesignerCs">Whether to include .Designer.cs files.</param>
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
    /// Determines whether a file path is allowed based on the instance ending filters.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <returns>True if the file should be removed from the list; otherwise false.</returns>
    public bool AllowOnly(string path)
    {
        return AllowOnly(path, true);
    }

    /// <summary>
    /// Determines whether a file path is allowed based on the instance filters.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <param name="alsoEnds">Whether to also check ending filters.</param>
    /// <returns>True if the file should be removed from the list; otherwise false.</returns>
    public bool AllowOnly(string path, bool alsoEnds)
    {
        var isEndMatch = true;
        return !AllowOnly(path, e!, c!, ref isEndMatch, alsoEnds);
    }

    /// <summary>
    /// Determines whether a file path is allowed based on the instance contains filters.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <returns>True if the file should be removed from the list; otherwise false.</returns>
    public bool AllowOnlyContains(string path)
    {
        return !AllowOnlyContains(path, c!);
    }
}
