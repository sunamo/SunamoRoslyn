namespace SunamoRoslyn._public;

/// <summary>
/// Represents a code element found during source code search or indexing.
/// </summary>
public class FoundedCodeElement : IComparable<FoundedCodeElement>
{
    /// <summary>
    /// The starting position of the code element. Is -1 if location is not known (search in content etc.).
    /// </summary>
    public int From { get; set; }

    /// <summary>
    /// The length of the code element.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// The line number where the code element was found.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FoundedCodeElement"/> class.
    /// </summary>
    /// <param name="line">The line number.</param>
    /// <param name="from">The starting position.</param>
    /// <param name="length">The length of the element.</param>
    public FoundedCodeElement(int line, int from, int length)
    {
        Length = length;
        Line = line;
        From = from;
    }

    /// <summary>
    /// Compares this instance to another <see cref="FoundedCodeElement"/>.
    /// </summary>
    /// <param name="other">The other element to compare to.</param>
    /// <returns>A value indicating the relative order.</returns>
    public int CompareTo(FoundedCodeElement? other)
    {
        return 0;
    }
}
