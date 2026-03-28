namespace SunamoRoslyn.Extensions;

/// <summary>
/// Provides extension methods for <see cref="SyntaxNode"/> objects.
/// </summary>
public static class SyntaxNodeExtensions
{
    /// <summary>
    /// Removes all trivia (whitespace, comments, etc.) from the given syntax node.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to strip trivia from.</param>
    /// <returns>A new syntax node with all trivia removed.</returns>
    public static SyntaxNode NoTrivia(this SyntaxNode syntaxNode)
    {
        return RoslynHelper.WithoutAllTrivia(syntaxNode);
    }
}