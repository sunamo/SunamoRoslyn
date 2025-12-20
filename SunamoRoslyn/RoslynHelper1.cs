// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoRoslyn;
public partial class RoslynHelper
{
    /// <summary>
    /// Because of searching is very unreliable
    /// Into A1 I have to insert class when I search in classes. If I insert root/ns/etc, method will be return to me whole class, because its contain method
    /// </summary>
    /// <param name = "parent"></param>
    /// <param name = "child"></param>
    public static SyntaxNode FindNode(SyntaxNode parent, SyntaxNode child, bool onlyDirectSub, out int dx)
    {
        dx = -1;
#region MyRegion
        if (true)
        {
            if (onlyDirectSub)
            {
                // toto mi vratí např. jen public, nikoliv celou stránku
                //var ss = cl.ChildThatContainsPosition(cl.GetLocation().SourceSpan.Start);
                foreach (var item in parent.ChildNodes())
                {
                    // Má tu lokaci trochu dál protože obsahuje zároveň celou třídu
                    string l1 = item.GetLocation().ToString();
                    string l2 = child.GetLocation().ToString();
                    var text = child.Span;
                    var s2 = child.FullSpan;
                    var s3 = child.GetReference();
                    if (l1 == l2)
                    {
                        return item;
                    }
                }
            }
            else
            {
                return parent.FindNode(child.FullSpan, false, true).WithoutLeadingTrivia().WithoutTrailingTrivia();
            }

            return null;
        }

#endregion
        var childType = child.GetType().FullName;
        var parentType = parent.GetType().FullName;
        SyntaxNode result = null;
        if (child is MethodDeclarationSyntax && parent is ClassDeclarationSyntax)
        {
            ClassDeclarationSyntax cl = (ClassDeclarationSyntax)parent;
            MethodDeclarationSyntax method = (MethodDeclarationSyntax)child;
            foreach (var item in cl.Members)
            {
                dx++;
                if (item is MethodDeclarationSyntax)
                {
                    var method2 = (MethodDeclarationSyntax)item;
                    bool same = true;
                    if (method.Identifier.Text != method2.Identifier.Text)
                    {
                        same = false;
                    }

                    if (same)
                    {
                        if (!RoslynComparer.Modifiers(method.Modifiers, method2.Modifiers))
                        {
                            same = false;
                        }
                    }

                    if (same)
                    {
                        string p1 = GetParameters(method.ParameterList);
                        string p2 = GetParameters(method2.ParameterList);
                        if (p1 != p2)
                        {
                            same = false;
                        }
                    }

                    if (same)
                    {
                        result = method2;
                        break;
                    }
                }
            }
        }
        else if (child is BaseTypeDeclarationSyntax && parent is NamespaceDeclarationSyntax)
        {
            var ns = (NamespaceDeclarationSyntax)parent;
            var method = (BaseTypeDeclarationSyntax)child;
            foreach (BaseTypeDeclarationSyntax item in ns.Members)
            {
                dx++;
                if (method.Identifier.Value == item.Identifier.Value)
                {
                    result = method;
                    break;
                }
            }
        }
        else if (child is NamespaceDeclarationSyntax && parent is CompilationUnitSyntax)
        {
            var ns = (CompilationUnitSyntax)parent;
            var method = (NamespaceDeclarationSyntax)child;
            foreach (NamespaceDeclarationSyntax item in ns.Members)
            {
                dx++;
                string fs1 = method.Name.ToFullString();
                string fs2 = item.Name.ToFullString();
                if (fs1 == fs2)
                {
                    result = method;
                    break;
                }
            }
        }
        else if (child is ClassDeclarationSyntax && parent is CompilationUnitSyntax)
        {
            var ns = (CompilationUnitSyntax)parent;
            var method = (ClassDeclarationSyntax)child;
            foreach (ClassDeclarationSyntax item in ns.Members)
            {
                dx++;
                string fs1 = method.Identifier.ToFullString();
                string fs2 = item.Identifier.ToFullString();
                if (fs1 == fs2)
                {
                    result = method;
                    break;
                }
            }
        }
        else
        {
            ThrowEx.NotImplementedCase(SHJoinPairs.JoinPairs("Parent", parent.ToFullString(), "Child", child.ToFullString()));
        }

        return result;
    //return nsShared.FindNode(cl.FullSpan, false, true).WithoutLeadingTrivia().WithoutTrailingTrivia();
    }

    /// <summary>
    /// IUN
    /// </summary>
    /// <param name = "cl2"></param>
    /// <param name = "method"></param>
    /// <param name = "keepDirectives"></param>
    
#pragma warning disable
    public static ClassDeclarationSyntax RemoveNode(ClassDeclarationSyntax cl2, SyntaxNode method, SyntaxRemoveOptions keepDirectives)
#pragma warning restore
    {
        ThrowEx.NotImplementedMethod();
#region MyRegion
        //var children = method.ChildNodesAndTokens().ToList();
        //for (int i = children.Count() - 1; i >= 0; i--)
        //{
        //    var temp = children[i].GetType().FullName;
        //    if (!(children[i] is MethodDeclarationSyntax))
        //    {
        //        int i2 = 0;
        //    }
        //    else
        //    {
        //        children.RemoveAt(i);
        //    }
        //}
        //return null;
        //FindNode()
        //cl2.Members.
        return null;
#endregion
    }

    /// <summary>
    /// Return null if
    /// Into A2 insert first member of A1 - Namespace/Class
    /// A1 should be rather Tree/CompilationUnitSyntax than Node because of Members - Node.ChildNodes.First is usings
    /// </summary>
    /// <param name = "root"></param>
    /// <param name = "ns"></param>
    public static ClassDeclarationSyntax GetClass(SyntaxNode root2, out SyntaxNode ns)
    {
        ns = null;
        ClassDeclarationSyntax helloWorldDeclaration = null;
        //(CompilationUnitSyntax)
        var root = root2;
        //var root = (CompilationUnitSyntax)tree.GetRoot();
        // Returns usings and ns
        var childNodes = root.ChildNodes();
        if (childNodes.OfType<ClassDeclarationSyntax>().Count() > 1)
        {
            return null;
        }

        SyntaxNode firstMember = null;
        firstMember = ChildNodes.NamespaceOrClass(root);
        //firstMember = (SyntaxNode)root.ChildNodes().OfType<NamespaceDeclarationSyntax>().FirstOrNull();
        //if (firstMember == null)
        //{
        //    firstMember = root.ChildNodes().OfType<ClassDeclarationSyntax>().First();
        //}
        if (firstMember is NamespaceDeclarationSyntax)
        {
            ns = (NamespaceDeclarationSyntax)firstMember;
            int i = 0;
            var fm = ((NamespaceDeclarationSyntax)ns).Members[i++];
            while (fm.GetType() != typeof(ClassDeclarationSyntax))
            {
                fm = ((NamespaceDeclarationSyntax)ns).Members[i++];
            }

            helloWorldDeclaration = (ClassDeclarationSyntax)fm;
        }
        else if (firstMember is ClassDeclarationSyntax)
        {
            helloWorldDeclaration = (ClassDeclarationSyntax)firstMember;
        // keep ns as null
        //ns = nu;
        }
        else
        {
            ThrowEx.NotImplementedCase(firstMember);
        }

        return helloWorldDeclaration;
    }

    public static List<string> HeadersOfMethod(IList<SyntaxNode> enumerable, bool alsoModifier = true)
    {
        List<string> clMethodsSharedNew = new List<string>();
        foreach (MethodDeclarationSyntax m in enumerable)
        {
            string h = GetHeaderOfMethod(m, alsoModifier);
            clMethodsSharedNew.Add(h);
        }

        return clMethodsSharedNew;
    }

    public static SyntaxNode WithoutAllTrivia(SyntaxNode sn)
    {
        return sn.WithoutLeadingTrivia().WithoutTrailingTrivia();
    }

    public static string GetHeaderOfMethod(MethodDeclarationSyntax m, bool alsoModifier = true)
    {
        m = m.WithoutTrivia();
        string addAfter = " ";
        StringBuilder stringBuilder = new();
        if (alsoModifier)
        {
            stringBuilder.AddItem(addAfter, RoslynParser.GetAccessModifiers(m.Modifiers));
        }

        bool isStatic = IsStatic(m.Modifiers);
        if (isStatic)
        {
            stringBuilder.AddItem(addAfter, "static");
        }

        stringBuilder.AddItem(addAfter, m.ReturnType.WithoutTrivia().ToFullString());
        stringBuilder.AddItem(addAfter, m.Identifier.WithoutTrivia().Text);
        // in brackets, newline
        //string parameters = m.ParameterList.ToFullString();
        // only text
        string p2 = GetParameters(m.ParameterList);
        stringBuilder.AddItem(addAfter, "(" + p2 + ")");
        string text = stringBuilder.ToString();
        return text;
    }

    /// <summary>
    /// CompilationUnitSyntax is also SyntaxNode
    /// After line must be A1 = A2 or some RoslynHelper.Get* methods
    /// </summary>
    /// <param name = "cl"></param>
    /// <param name = "cl2"></param>
    /// <param name = "root"></param>
    public static void ReplaceNode(SyntaxNode cl, SyntaxNode cl2, out SyntaxNode root)
    {
        ReplaceNode<SyntaxNode>(cl, cl2, out root);
    }
}