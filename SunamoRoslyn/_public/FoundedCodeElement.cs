namespace SunamoRoslyn._public;

public class FoundedCodeElement : IComparable<FoundedCodeElement>
{
    // Is -1 if location is not known (search in content etc.).
    public int From { get; set; }

    public int Length { get; set; }

    public int Line { get; set; }

    public FoundedCodeElement(int line, int from, int length)
    {
        Length = length;
        Line = line;
        From = from;
    }

    public int CompareTo(FoundedCodeElement? other)
    {
        return 0;
    }
}
