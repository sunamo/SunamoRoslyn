namespace SunamoRoslyn.Extensions;

/// <summary>
/// Represents the syntax node extensions type.
/// </summary>
public static class SyntaxNodeExtensions
{
    /// <summary>
    /// No trivia.
    /// </summary>
    /// <param name="syntaxNode">The syntax node.</param>
    /// <returns>The no trivia result.</returns>
    public static SyntaxNode NoTrivia(this SyntaxNode syntaxNode)
    {
        return RoslynHelper.WithoutAllTrivia(syntaxNode);
    }
}
