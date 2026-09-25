namespace SunamoRoslyn;

public partial class RoslynHelper
{
    // CompilationUnitSyntax is also a SyntaxNode.
    // After calling this method, the caller must reassign the result because old references are invalidated.
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

    public static bool IsStatic(SyntaxTokenList modifiers)
    {
        return modifiers.Where(modifier => modifier.Value?.ToString() == "static").Count() > 0;
    }

    public static string NameWithoutGeneric(string name)
    {
        return SHParts.RemoveAfterFirst(name, "<");
    }
}
