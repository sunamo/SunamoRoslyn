// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn._sunamo;

internal static class StringBuilderExtensions
{
    internal static void AddItem(this StringBuilder sb, string postfix, string text)
    {
        sb.Append(text + postfix);
    }
}