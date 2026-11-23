namespace SunamoRoslyn;

/// <summary>
/// Represents the child nodes type.
/// </summary>
public class ChildNodes
{
    /// <summary>
    /// Methods.
    /// </summary>
    /// <param name="syntaxNode">The syntax node.</param>
    /// <returns>The methods result.</returns>
    public static IList<MethodDeclarationSyntax> Methods(SyntaxNode syntaxNode)
    {
        return syntaxNode.ChildNodes().OfType<MethodDeclarationSyntax>().ToList();
    }

    /// <summary>
    /// Methods descendant.
    /// </summary>
    /// <param name="syntaxNode">The syntax node.</param>
    /// <returns>The methods descendant result.</returns>
    public static IList<MethodDeclarationSyntax> MethodsDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
    }

    /// <summary>
    /// Fields descendant.
    /// </summary>
    /// <param name="syntaxNode">The syntax node.</param>
    /// <returns>The fields descendant result.</returns>
    public static IList<FieldDeclarationSyntax> FieldsDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
    }

    // VariablesDescendant returns only the variable part (e.g. int a1).
    // FieldsDescendant returns the whole field declaration (e.g. public int a1).
    /// <summary>
    /// Variables descendant.
    /// </summary>
    /// <param name="syntaxNode">The syntax node.</param>
    /// <returns>The variables descendant result.</returns>
    public static IList<VariableDeclarationSyntax> VariablesDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<VariableDeclarationSyntax>().ToList();
    }

    /// <summary>
    /// Method.
    /// </summary>
    /// <param name="classDeclaration">The class declaration.</param>
    /// <param name="methodHeader">The method header.</param>
    /// <returns>The method result.</returns>
    public static MethodDeclarationSyntax? Method(ClassDeclarationSyntax classDeclaration, string methodHeader)
    {
        var methodToFind = RoslynParser.Method(methodHeader);

        var foundNode = RoslynHelper.FindNode(classDeclaration, methodToFind, true);
        return foundNode as MethodDeclarationSyntax;
    }

    /// <summary>
    /// Namespace.
    /// </summary>
    /// <param name="syntaxNode">The syntax node.</param>
    /// <returns>The namespace result.</returns>
    public static NamespaceDeclarationSyntax? Namespace(SyntaxNode syntaxNode)
    {
        if (syntaxNode is NamespaceDeclarationSyntax ns)
        {
            return ns;
        }
        return syntaxNode.ChildNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
    }

    /// <summary>
    /// Class.
    /// </summary>
    /// <param name="syntaxNode">The syntax node.</param>
    /// <returns>The class result.</returns>
    public static ClassDeclarationSyntax? Class(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax cls)
        {
            return cls;
        }
        return syntaxNode.ChildNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
    }

    /// <summary>
    /// Namespace or class.
    /// </summary>
    /// <param name="root">The root.</param>
    /// <returns>The namespace or class result.</returns>
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
