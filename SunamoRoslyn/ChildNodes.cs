namespace SunamoRoslyn;

/// <summary>
/// Provides methods for retrieving child and descendant syntax nodes from a Roslyn syntax tree.
/// </summary>
public class ChildNodes
{
    /// <summary>
    /// Returns direct child method declarations. If not working, try <see cref="MethodsDescendant"/>.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to search for method declarations.</param>
    /// <returns>A list of method declaration syntax nodes.</returns>
    public static IList<MethodDeclarationSyntax> Methods(SyntaxNode syntaxNode)
    {
        return syntaxNode.ChildNodes().OfType<MethodDeclarationSyntax>().ToList();
    }

    /// <summary>
    /// Returns all descendant method declarations. If not working, try <see cref="Methods"/>.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to search for descendant method declarations.</param>
    /// <returns>A list of descendant method declaration syntax nodes.</returns>
    public static IList<MethodDeclarationSyntax> MethodsDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
    }

    /// <summary>
    /// Returns all descendant field declarations.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to search for descendant field declarations.</param>
    /// <returns>A list of descendant field declaration syntax nodes.</returns>
    public static IList<FieldDeclarationSyntax> FieldsDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
    }

    /// <summary>
    /// Returns all descendant variable declarations.
    /// VariablesDescendant returns only the variable part (e.g. int a1).
    /// FieldsDescendant returns the whole field declaration (e.g. public int a1).
    /// </summary>
    /// <param name="syntaxNode">The syntax node to search for descendant variable declarations.</param>
    /// <returns>A list of descendant variable declaration syntax nodes.</returns>
    public static IList<VariableDeclarationSyntax> VariablesDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<VariableDeclarationSyntax>().ToList();
    }

    /// <summary>
    /// Finds a method declaration within a class by its header text.
    /// </summary>
    /// <param name="classDeclaration">The class declaration to search within.</param>
    /// <param name="methodHeader">The method header text to find.</param>
    /// <returns>The matching method declaration syntax node.</returns>
    public static MethodDeclarationSyntax? Method(ClassDeclarationSyntax classDeclaration, string methodHeader)
    {
        var methodToFind = RoslynParser.Method(methodHeader);

        var foundNode = RoslynHelper.FindNode(classDeclaration, methodToFind, true);
        return foundNode as MethodDeclarationSyntax;
    }

    /// <summary>
    /// Returns the namespace declaration from a syntax node.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to extract the namespace from.</param>
    /// <returns>The namespace declaration syntax node, or null if not found.</returns>
    public static NamespaceDeclarationSyntax? Namespace(SyntaxNode syntaxNode)
    {
        if (syntaxNode is NamespaceDeclarationSyntax)
        {
            return (NamespaceDeclarationSyntax)syntaxNode;
        }
        return syntaxNode.ChildNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
    }

    /// <summary>
    /// Returns the class declaration from a syntax node.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to extract the class from.</param>
    /// <returns>The class declaration syntax node, or null if not found.</returns>
    public static ClassDeclarationSyntax? Class(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax)
        {
            return (ClassDeclarationSyntax)syntaxNode;
        }
        return syntaxNode.ChildNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
    }

    /// <summary>
    /// Returns the first namespace or class declaration from a root syntax node.
    /// </summary>
    /// <param name="root">The root syntax node to search.</param>
    /// <returns>The namespace or class declaration syntax node.</returns>
    public static SyntaxNode? NamespaceOrClass(SyntaxNode root)
    {
        var ns = Namespace(root);
        if (ns != null)
        {
            return ns;
        }
        return Class(root);
    }
}
