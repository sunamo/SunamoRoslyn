// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoRoslyn._sunamo;
internal class SHJoinPairs
{
    internal static string JoinPairs(string firstDelimiter, string secondDelimiter, params string[] parts)
    {
        //InitApp.TemplateLogger.NotEvenNumberOfElements(type, "JoinPairs", @"args", parts);
        //InitApp.TemplateLogger.AnyElementIsNull(type, "JoinPairs", @"args", parts);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < parts.Length; i++)
        {
            stringBuilder.Append(parts[i++] + firstDelimiter);
            stringBuilder.Append(parts[i] + secondDelimiter);
        }
        return stringBuilder.ToString();
    }
}