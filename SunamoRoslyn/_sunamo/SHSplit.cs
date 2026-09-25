namespace SunamoRoslyn._sunamo;

internal class SHSplit
{
    internal static List<string> SplitNone(string text, params string[] delimiters)
        => text.Split(delimiters, StringSplitOptions.None).ToList();
}
