namespace SunamoRoslyn.Services;

/// <summary>
/// Represents the roslyn comment service type.
/// </summary>
public class RoslynCommentService
{
    /// <summary>
    /// Remove comments.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <returns>The remove comments result.</returns>
    public string RemoveComments(string code)
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var rewriter = new CommentRemover();
        var newRoot = rewriter.Visit(root);
        return newRoot.ToFullString();
    }
}

/// <summary>
/// Represents the comment remover type.
/// </summary>
public class CommentRemover : CSharpSyntaxRewriter
{
    /// <summary>
    /// Visit trivia.
    /// </summary>
    /// <param name="trivia">The trivia.</param>
    /// <returns>The visit trivia result.</returns>
    public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
    {
        if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia)
 ||
            trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
            trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
            trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
        {
            return default; // Return an empty trivia
        }
        return base.VisitTrivia(trivia);
    }
}
