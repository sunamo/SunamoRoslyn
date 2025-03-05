namespace SunamoRoslyn._sunamo;

internal class BTS
{
    internal static bool Is(bool binFp, bool n)
    {
        if (n) return !binFp;
        return binFp;
    }
}