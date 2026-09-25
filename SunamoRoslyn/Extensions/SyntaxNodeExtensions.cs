namespace SunamoRoslyn.Extensions;

public static class SyntaxNodeExtensions
{
    public static SyntaxNode NoTrivia(this SyntaxNode syntaxNode)
    {
        return RoslynHelper.WithoutAllTrivia(syntaxNode);
    }
}
