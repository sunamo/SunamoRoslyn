namespace SunamoRoslyn._sunamo;

internal class SHReplace
{
    internal static string ReplaceOnce(string input, string what, string replacement)
    {
        if (what == "") return input;
        var index = input.IndexOf(what);
        if (index == -1) return input;
        return input.Substring(0, index) + replacement + input.Substring(index + what.Length);
    }
}
