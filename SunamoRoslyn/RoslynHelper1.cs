namespace SunamoRoslyn;

public partial class RoslynHelper
{
    /// <summary>
    /// Finds a syntax node within a parent that matches the given child node.
    /// When searching in classes, insert the class as the parent. If the root/namespace is inserted,
    /// the method will return the whole class because it contains the method.
    /// </summary>
    /// <param name="parent">The parent syntax node to search within.</param>
    /// <param name="child">The child syntax node to find.</param>
    /// <param name="isOnlyDirectSub">Whether to search only direct child nodes or use span-based matching.</param>
    /// <param name="foundIndex">The index of the found node within its parent's members.</param>
    /// <returns>The found syntax node, or null if not found.</returns>
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

    /// <summary>
    /// Removes a syntax node from a class declaration. Currently not implemented.
    /// </summary>
    /// <param name="classDeclaration">The class declaration to remove the node from.</param>
    /// <param name="nodeToRemove">The syntax node to remove.</param>
    /// <param name="keepDirectives">Options controlling how directives and trivia are handled during removal.</param>
    /// <returns>The modified class declaration.</returns>
    public static ClassDeclarationSyntax? RemoveNode(ClassDeclarationSyntax classDeclaration, SyntaxNode nodeToRemove, SyntaxRemoveOptions keepDirectives)
    {
        ThrowEx.NotImplementedMethod();
        return null;
    }

    /// <summary>
    /// Gets the first class declaration from a syntax tree root, also outputting the namespace node.
    /// Returns null if more than one class declaration exists at the top level.
    /// The root should be a CompilationUnitSyntax rather than a plain SyntaxNode
    /// because Members on a SyntaxNode via ChildNodes includes usings.
    /// </summary>
    /// <param name="rootNode">The root syntax node (typically CompilationUnitSyntax).</param>
    /// <param name="namespaceNode">Outputs the namespace syntax node, or null if the class is at root level.</param>
    /// <returns>The class declaration syntax node, or null if multiple classes exist.</returns>
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

    /// <summary>
    /// Returns the method header strings for a list of method declaration syntax nodes.
    /// </summary>
    /// <param name="syntaxNodes">The list of syntax nodes (must be MethodDeclarationSyntax).</param>
    /// <param name="alsoModifier">Whether to include access modifiers in the header.</param>
    /// <returns>A list of method header strings.</returns>
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

    /// <summary>
    /// Removes all leading and trailing trivia from a syntax node.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to strip trivia from.</param>
    /// <returns>The syntax node without any leading or trailing trivia.</returns>
    public static SyntaxNode WithoutAllTrivia(SyntaxNode syntaxNode)
    {
        return syntaxNode.WithoutLeadingTrivia().WithoutTrailingTrivia();
    }

    /// <summary>
    /// Builds a method header string from a method declaration syntax node,
    /// including optional access modifiers, static keyword, return type, name, and parameters.
    /// </summary>
    /// <param name="methodDeclaration">The method declaration syntax node.</param>
    /// <param name="alsoModifier">Whether to include access modifiers in the header.</param>
    /// <returns>The method header string.</returns>
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

    /// <summary>
    /// Replaces a syntax node in the tree and walks up to the root, returning the new root.
    /// After calling this method, the caller must reassign the result because the old references are invalidated.
    /// </summary>
    /// <param name="originalNode">The original syntax node to replace.</param>
    /// <param name="replacementNode">The replacement syntax node.</param>
    /// <param name="root">Outputs the new root syntax node after replacement.</param>
    public static void ReplaceNode(SyntaxNode originalNode, SyntaxNode replacementNode, out SyntaxNode root)
    {
        ReplaceNode<SyntaxNode>(originalNode, replacementNode, out root);
    }
}
