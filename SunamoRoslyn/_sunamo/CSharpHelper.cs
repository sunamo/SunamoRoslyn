// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn._sunamo;

internal static class CSharpHelper
{
    internal static List<string> Usings(CompilationUnitSyntax root)
    {
        List<string> result = new();
        foreach (UsingDirectiveSyntax usingDirective in root.Usings)
        {
            // Z�sk�n� n�zvu using direktivy
            result.Add(usingDirective.Name.ToString());
        }
        return result;
    }
}