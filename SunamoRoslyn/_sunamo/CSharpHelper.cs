namespace SunamoRoslyn._sunamo;

/// <summary>
/// Helper for extracting information from C# syntax trees.
/// </summary>
internal static class CSharpHelper
{
    /// <summary>
    /// Gets all using directive names from the compilation unit.
    /// </summary>
    /// <param name="root">The compilation unit syntax node.</param>
    /// <returns>List of using directive names.</returns>
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
