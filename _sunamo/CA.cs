namespace SunamoRoslyn._sunamo;
internal class CA
{
    public static List<string> Prepend(string v, List<string> toReplace)
    {
        for (var i = 0; i < toReplace.Count; i++)
            if (!toReplace[i].StartsWith(v))
                toReplace[i] = v + toReplace[i];
        return toReplace;
    }
    public static void RemoveLines(List<string> lines, List<int> removeLines)
    {
        removeLines.Sort();
        for (var i = removeLines.Count - 1; i >= 0; i--)
        {
            var dx = removeLines[i];
            lines.RemoveAt(dx);
        }
    }
    internal static List<string> RemoveStringsEmptyTrimBefore(List<string> mySites)
    {
        for (var i = mySites.Count - 1; i >= 0; i--)
            if (mySites[i].Trim() == string.Empty)
                mySites.RemoveAt(i);
        return mySites;
    }
}