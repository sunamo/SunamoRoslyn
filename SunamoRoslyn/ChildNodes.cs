namespace SunamoRoslyn;

public class ChildNodes
{
    public static IList<MethodDeclarationSyntax> Methods(SyntaxNode syntaxNode)
    {
        return syntaxNode.ChildNodes().OfType<MethodDeclarationSyntax>().ToList();
    }

    public static IList<MethodDeclarationSyntax> MethodsDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
    }

    public static IList<FieldDeclarationSyntax> FieldsDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
    }

    // VariablesDescendant returns only the variable part (e.g. int a1).
    // FieldsDescendant returns the whole field declaration (e.g. public int a1).
    public static IList<VariableDeclarationSyntax> VariablesDescendant(SyntaxNode syntaxNode)
    {
        return syntaxNode.DescendantNodes().OfType<VariableDeclarationSyntax>().ToList();
    }

    public static MethodDeclarationSyntax? Method(ClassDeclarationSyntax classDeclaration, string methodHeader)
    {
        var methodToFind = RoslynParser.Method(methodHeader);

        var foundNode = RoslynHelper.FindNode(classDeclaration, methodToFind, true);
        return foundNode as MethodDeclarationSyntax;
    }

    public static NamespaceDeclarationSyntax? Namespace(SyntaxNode syntaxNode)
    {
        if (syntaxNode is NamespaceDeclarationSyntax ns)
        {
            return ns;
        }
        return syntaxNode.ChildNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
    }

    public static ClassDeclarationSyntax? Class(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax cls)
        {
            return cls;
        }
        return syntaxNode.ChildNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
    }

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
