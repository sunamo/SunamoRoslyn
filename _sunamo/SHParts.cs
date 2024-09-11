namespace SunamoRoslyn._sunamo;

internal class SHParts
{
    internal static string RemoveAfterFirst(string t, string ch)
    {
        var dex = t.IndexOf(ch);
        if (dex == -1 || dex == t.Length - 1) return t;

        var vr = t.Remove(dex);
        return vr;
    }
}