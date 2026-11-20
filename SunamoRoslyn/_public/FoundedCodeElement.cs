namespace SunamoRoslyn._public;

/// <summary>
/// Represents the founded code element type.
/// </summary>
public class FoundedCodeElement : IComparable<FoundedCodeElement>
{
    // Is -1 if location is not known (search in content etc.).
    /// <summary>
    /// Gets or sets the from.
    /// </summary>
    public int From { get; set; }

    /// <summary>
    /// Gets or sets the length.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Gets or sets the line.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Founded code element.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <param name="from">The from.</param>
    /// <param name="length">The length.</param>
    /// <returns>The founded code element result.</returns>
    public FoundedCodeElement(int line, int from, int length)
    {
        Length = length;
        Line = line;
        From = from;
    }

    /// <summary>
    /// Compare to.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <param name="from">The from.</param>
    /// <param name="length">The length.</param>
    /// <returns>The compare to result.</returns>
    public int CompareTo(FoundedCodeElement? other)
    {
        return 0;
    }
}
