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