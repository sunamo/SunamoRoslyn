namespace SunamoRoslyn.Services;

/// <summary>
/// Provides functionality for removing comments from C# source code using Roslyn.
/// </summary>
public class RoslynCommentService
{
    /// <summary>
    /// Removes all comments (single-line, multi-line, and documentation) from the given C# source code.
    /// </summary>
    /// <param name="code">The C# source code to remove comments from.</param>
    /// <returns>The source code with all comments removed.</returns>
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
/// A syntax rewriter that removes all comment trivia from a C# syntax tree.
/// </summary>
public class CommentRemover : CSharpSyntaxRewriter
{
    /// <summary>
    /// Visits a trivia node and removes it if it is a comment.
    /// </summary>
    /// <param name="trivia">The syntax trivia to visit.</param>
    /// <returns>An empty trivia if the input is a comment; otherwise, the original trivia.</returns>
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