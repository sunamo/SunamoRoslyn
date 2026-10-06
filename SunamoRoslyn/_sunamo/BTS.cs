namespace SunamoRoslyn._sunamo;

internal class BTS
{
    internal static bool Is(bool value, bool isNegating)
    {
        if (isNegating) return !value;
        return value;
    }
}
