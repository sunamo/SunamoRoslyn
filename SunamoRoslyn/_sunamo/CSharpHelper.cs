namespace SunamoRoslyn._sunamo;

internal static class CSharpHelper
{
    internal static List<string> Usings(CompilationUnitSyntax root)
    {
        List<string> result = new();
        foreach (UsingDirectiveSyntax usingDirective in root.Usings)
        {
            result.Add(usingDirective.Name?.ToString() ?? string.Empty);
        }
        return result;
    }
}
