namespace SunamoRoslyn;

public partial class RoslynHelper
{
    /// <summary>
    /// Replaces a syntax node in the tree and walks up to the root, returning both the
    /// replaced node (cast to <typeparamref name="T"/>) and the new root.
    /// CompilationUnitSyntax is also a SyntaxNode.
    /// After calling this method, the caller must reassign the result because old references are invalidated.
    /// </summary>
    /// <typeparam name="T">The type of the replacement node to return.</typeparam>
    /// <param name="originalNode">The original syntax node to replace.</param>
    /// <param name="replacementNode">The replacement syntax node.</param>
    /// <param name="root">Outputs the new root syntax node after replacement.</param>
    /// <returns>The replacement node cast to <typeparamref name="T"/>.</returns>
    public static T? ReplaceNode<T>(SyntaxNode originalNode, SyntaxNode replacementNode, out SyntaxNode root)
        where T : SyntaxNode
    {
        bool isFirst = true;
        T? result = default;
        while (originalNode is SyntaxNode)
        {
            if (originalNode.Parent == null)
            {
                break;
            }

            originalNode = originalNode.Parent.ReplaceNode(originalNode, replacementNode);
            if (isFirst)
            {
                result = (T)replacementNode;
                isFirst = false;
            }

            replacementNode = originalNode;
            if (originalNode.Parent == null)
            {
                break;
            }
            originalNode = originalNode.Parent;
        }

        root = replacementNode;

        return result;
    }

    /// <summary>
    /// Builds a comma-separated string of parameter declarations from a parameter list syntax.
    /// </summary>
    /// <param name="parameterList">The parameter list syntax to extract parameters from.</param>
    /// <returns>A comma-separated string of parameter declarations.</returns>
    private static string GetParameters(ParameterListSyntax parameterList)
    {
        var childNodes = parameterList.ChildNodes();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in childNodes)
        {
            stringBuilder.Append(item.ToFullString() + ", ");
        }

        string result = SH.RemoveLastLetters(stringBuilder.ToString(), 2);
        return result;
    }

    /// <summary>
    /// Determines whether the given modifier list contains the static keyword.
    /// </summary>
    /// <param name="modifiers">The list of syntax tokens representing modifiers.</param>
    /// <returns>True if the modifiers include the static keyword; otherwise, false.</returns>
    public static bool IsStatic(SyntaxTokenList modifiers)
    {
        return modifiers.Where(modifier => modifier.Value?.ToString() == "static").Count() > 0;
    }

    /// <summary>
    /// Removes the generic type parameter portion from a type name.
    /// </summary>
    /// <param name="name">The type name potentially containing generic parameters.</param>
    /// <returns>The type name without the generic portion.</returns>
    public static string NameWithoutGeneric(string name)
    {
        return SHParts.RemoveAfterFirst(name, "<");
    }
}
