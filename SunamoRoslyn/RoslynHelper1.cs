namespace SunamoRoslyn;

public partial class RoslynHelper
{
    // When searching in classes, insert the class as the parent. If the root/namespace is inserted,
    // the method will return the whole class because it contains the method.
    public static SyntaxNode? FindNode(SyntaxNode parent, SyntaxNode child, bool isOnlyDirectSub, out int foundIndex)
    {
        foundIndex = -1;
        if (isOnlyDirectSub)
        {
            foreach (var item in parent.ChildNodes())
            {
                string firstLocation = item.GetLocation().ToString();
                string secondLocation = child.GetLocation().ToString();
                if (firstLocation == secondLocation)
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

    public static ClassDeclarationSyntax? RemoveNode(ClassDeclarationSyntax classDeclaration, SyntaxNode nodeToRemove, SyntaxRemoveOptions keepDirectives)
    {
        ThrowEx.NotImplementedMethod();
        return null;
    }

    // The root should be a CompilationUnitSyntax rather than a plain SyntaxNode
    // because Members on a SyntaxNode via ChildNodes includes usings.
    // Returns null if more than one class declaration exists at the top level.
    public static ClassDeclarationSyntax? GetClass(SyntaxNode rootNode, out SyntaxNode? namespaceNode)
    {
        namespaceNode = null;
        ClassDeclarationSyntax? classDeclaration = null;
        var root = rootNode;
        var childNodes = root.ChildNodes();
        if (childNodes.OfType<ClassDeclarationSyntax>().Count() > 1)
        {
            return null;
        }

        SyntaxNode? firstMember = ChildNodes.NamespaceOrClass(root);
        if (firstMember is NamespaceDeclarationSyntax)
        {
            namespaceNode = (NamespaceDeclarationSyntax)firstMember;
            int i = 0;
            var currentMember = ((NamespaceDeclarationSyntax)namespaceNode).Members[i++];
            while (currentMember.GetType() != typeof(ClassDeclarationSyntax))
            {
                currentMember = ((NamespaceDeclarationSyntax)namespaceNode).Members[i++];
            }

            classDeclaration = (ClassDeclarationSyntax)currentMember;
        }
        else if (firstMember is ClassDeclarationSyntax)
        {
            classDeclaration = (ClassDeclarationSyntax)firstMember;
        }
        else
        {
            ThrowEx.NotImplementedCase(firstMember!);
        }

        return classDeclaration;
    }

    public static List<string> HeadersOfMethod(IList<SyntaxNode> syntaxNodes, bool alsoModifier = true)
    {
        List<string> methodHeaders = new List<string>();
        foreach (MethodDeclarationSyntax methodDeclaration in syntaxNodes)
        {
            string header = GetHeaderOfMethod(methodDeclaration, alsoModifier);
            methodHeaders.Add(header);
        }

        return methodHeaders;
    }

    public static SyntaxNode WithoutAllTrivia(SyntaxNode syntaxNode)
    {
        return syntaxNode.WithoutLeadingTrivia().WithoutTrailingTrivia();
    }

    public static string GetHeaderOfMethod(MethodDeclarationSyntax methodDeclaration, bool alsoModifier = true)
    {
        methodDeclaration = methodDeclaration.WithoutTrivia();
        string separator = " ";
        StringBuilder stringBuilder = new();
        if (alsoModifier)
        {
            stringBuilder.AddItem(separator, RoslynParser.GetAccessModifiers(methodDeclaration.Modifiers));
        }

        bool isStatic = IsStatic(methodDeclaration.Modifiers);
        if (isStatic)
        {
            stringBuilder.AddItem(separator, "static");
        }

        stringBuilder.AddItem(separator, methodDeclaration.ReturnType.WithoutTrivia().ToFullString());
        stringBuilder.AddItem(separator, methodDeclaration.Identifier.WithoutTrivia().Text);
        string parametersText = GetParameters(methodDeclaration.ParameterList);
        stringBuilder.AddItem(separator, "(" + parametersText + ")");
        string headerText = stringBuilder.ToString();
        return headerText;
    }

    // After calling this method, the caller must reassign the result because the old references are invalidated.
    public static void ReplaceNode(SyntaxNode originalNode, SyntaxNode replacementNode, out SyntaxNode root)
    {
        ReplaceNode<SyntaxNode>(originalNode, replacementNode, out root);
    }
}
