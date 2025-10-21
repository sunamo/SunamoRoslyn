// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn._sunamo;

internal class BTS
{
    internal static bool Is(bool binFp, bool n)
    {
        if (n) return !binFp;
        return binFp;
    }
}