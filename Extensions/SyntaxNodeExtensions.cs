namespace Roslyn.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static SyntaxNode NoTrivia(this SyntaxNode n)
        {
            return RoslynHelper.WithoutAllTrivia(n);
        }
    }
}
