// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn.Extensions;

public static class SyntaxNodeExtensions
{
    public static SyntaxNode NoTrivia(this SyntaxNode n)
    {
        return RoslynHelper.WithoutAllTrivia(n);
    }
}
