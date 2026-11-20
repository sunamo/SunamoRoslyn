namespace SunamoRoslyn;

public partial class RoslynHelper
{
    // CompilationUnitSyntax is also a SyntaxNode.
    // After calling this method, the caller must reassign the result because old references are invalidated.
    /// <summary>
    /// Replace node t.
    /// </summary>
    /// <typeparam name="T">The type T.</typeparam>
    /// <param name="originalNode">The original node.</param>
    /// <param name="replacementNode">The replacement node.</param>
    /// <param name="root">The root.</param>
    /// <returns>The replace node t result.</returns>
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
    /// Is static.
    /// </summary>
    /// <param name="modifiers">The modifiers.</param>
    /// <returns>The is static result.</returns>
    public static bool IsStatic(SyntaxTokenList modifiers)
    {
        return modifiers.Where(modifier => modifier.Value?.ToString() == "static").Count() > 0;
    }

    /// <summary>
    /// Name without generic.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The name without generic result.</returns>
    public static string NameWithoutGeneric(string name)
    {
        return SHParts.RemoveAfterFirst(name, "<");
    }
}
