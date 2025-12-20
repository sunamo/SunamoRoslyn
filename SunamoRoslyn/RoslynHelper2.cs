// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn;
public partial class RoslynHelper
{
    /// <summary>
    /// CompilationUnitSyntax is also SyntaxNode
    /// After line must be A1 = A2
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "cl"></param>
    /// <param name = "cl2"></param>
    public static T ReplaceNode<T>(SyntaxNode cl, SyntaxNode cl2, out SyntaxNode root)
        where T : SyntaxNode
    {
        bool first = true;
        T result = default;
        while (cl is SyntaxNode)
        {
            if (cl.Parent == null)
            {
                break;
            }

            cl = cl.Parent.ReplaceNode(cl, cl2);
            if (first)
            {
                result = (T)cl2;
                first = false;
            }

            cl2 = cl;
            cl = cl.Parent;
        }

        root = cl2;
        if (result == null)
        {
        }

        return result;
    }

    private static string GetParameters(ParameterListSyntax parameterList)
    {
        var c1 = parameterList.ChildNodes();
        //var c2 = parameterList.ChildNodesAndTokens();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in c1)
        {
            stringBuilder.Append(item.ToFullString() + ", ");
        }

        string r = SH.RemoveLastLetters(stringBuilder.ToString(), 2);
        return r;
    }

    public static bool IsStatic(SyntaxTokenList modifiers)
    {
        return modifiers.Where(e => e.Value.ToString() == "static").Count() > 0;
    }

    public static string NameWithoutGeneric(string name)
    {
        return SHParts.RemoveAfterFirst(name, "<");
    }
}