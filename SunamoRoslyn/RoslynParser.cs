namespace SunamoRoslyn;

public class RoslynParser
{
    public static bool IsCSharpCode(string input)
    {
        SyntaxTree? syntaxTree = null;
        try
        {
            syntaxTree = CSharpSyntaxTree.ParseText(input);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        var _ = syntaxTree?.GetText().ToString();
        return syntaxTree != null;
    }

    public static MethodDeclarationSyntax Method(string methodHeader)
    {
        methodHeader = methodHeader + "{}";
        var tree = CSharpSyntaxTree.ParseText(methodHeader);
        var root = tree.GetRoot();
        var childNodes = root.ChildNodes();
        return (MethodDeclarationSyntax)childNodes.First();
    }

    public
async Task<List<string>?>
GetCodeOfElementsClass(string folderFrom, string folderTo)
    {
        FS.WithEndSlash(ref folderFrom);
        FS.WithEndSlash(ref folderTo);
        var files = Directory.GetFiles(folderFrom, "*.aspx.cs", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
await
FileAsync.ReadAllTextAsync(file));
            List<string> result = new List<string>();
            SyntaxNode? syntaxNode;
            var classDeclaration = RoslynHelper.GetClass(tree.GetRoot(), out syntaxNode);
            if (classDeclaration == null || syntaxNode == null)
            {
                continue;
            }
            SyntaxAnnotation syntaxNodeAnnotation = new SyntaxAnnotation();
            syntaxNode = syntaxNode.WithAdditionalAnnotations(syntaxNodeAnnotation);
            SyntaxAnnotation classAnnotation = new SyntaxAnnotation();
            classDeclaration = classDeclaration.WithAdditionalAnnotations(classAnnotation);
            var root = tree.GetRoot();
            int count = classDeclaration.Members.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var item = classDeclaration.Members[i];
                classDeclaration = classDeclaration.RemoveNode(item, SyntaxRemoveOptions.KeepEndOfLine)!;
            }
            syntaxNode = syntaxNode.TrackNodes(classDeclaration);
            root = root.TrackNodes(syntaxNode);
            var data = syntaxNode.SyntaxTree.ToString();
            var fileTo = file.Replace(folderFrom, folderTo);
            await FileAsync.WriteAllTextAsync(fileTo, data);
        }
        return null;
    }

    private SyntaxNode FindTopParent(SyntaxNode classDeclaration)
    {
        var result = classDeclaration;
        while (result.Parent != null)
        {
            result = result.Parent;
        }
        return result;
    }

    public static ABCRoslyn GetVariablesInCsharp(CompilationUnitSyntax root, out List<string> usings)
    {
        ABCRoslyn result = new ABCRoslyn();
        usings = CSharpHelper.Usings(root);
        ClassDeclarationSyntax? classDeclaration = RoslynHelper.GetClass(root);
        if (classDeclaration == null)
        {
            return result;
        }
        var variableDeclarations = classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>();
        foreach (var variableDeclaration in variableDeclarations)
        {
            string variableName = variableDeclaration.Declaration.Type.ToString();
            variableName = SHReplace.ReplaceOnce(variableName, "global::", "");
            int lastIndex = variableName.LastIndexOf('.');
            string namespaceName, className;
            SH.GetPartsByLocation(out namespaceName, out className, variableName, lastIndex);
            usings.Add(namespaceName);
            // in key type, in value name
            result.Add(ABRoslyn.Get(className, variableDeclaration.Declaration.Variables.First().Identifier.Text));
        }
        usings = usings.Distinct().ToList();
        return result;
    }

    public static string GetAccessModifiers(SyntaxTokenList modifiers)
    {
        foreach (var item in modifiers)
        {
            switch (item.Kind())
            {
                case SyntaxKind.PublicKeyword:
                case SyntaxKind.PrivateKeyword:
                case SyntaxKind.InternalKeyword:
                case SyntaxKind.ProtectedKeyword:
                    return item.WithoutTrivia().ToFullString();
            }
        }
        return string.Empty;
    }

    public static Tuple<List<string>, List<string>> ParseVariables(object code)
    {
        SyntaxNode syntaxRoot = SyntaxNodeFromObjectOrString(code);
        var variableDeclarations = syntaxRoot.DescendantNodes().OfType<VariableDeclarationSyntax>();
        var variableAssignments = syntaxRoot.DescendantNodes().OfType<AssignmentExpressionSyntax>();
        List<string> declaredVariables = new List<string>(variableDeclarations.Count());
        List<string> assignedVariables = new List<string>(variableAssignments.Count());
        foreach (var variableDeclaration in variableDeclarations)
            declaredVariables.Add(variableDeclaration.Variables.First().Identifier.Value?.ToString() ?? string.Empty);
        foreach (var variableAssignment in variableAssignments)
            assignedVariables.Add(variableAssignment.Left.ToString());
        return new Tuple<List<string>, List<string>>(declaredVariables, assignedVariables);
    }

    public static SyntaxNode SyntaxNodeFromObjectOrString(object code)
    {
        if (code is SyntaxNode syntaxNode)
        {
            return syntaxNode;
        }
        else if (code is string codeText)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(codeText);
            return tree.GetRoot();
        }
        else
        {
            ThrowEx.NotImplementedCase("else");
            throw new InvalidOperationException("Unsupported code type");
        }
    }

    public static Dictionary<string, List<string>> GetVariablesInEveryMethod(string text)
    {
        Dictionary<string, List<string>> methodVariables = new Dictionary<string, List<string>>();
        var tree = CSharpSyntaxTree.ParseText(text);
        var root = tree.GetRoot();
        IList<MethodDeclarationSyntax> methods = root
          .DescendantNodes()
          .OfType<MethodDeclarationSyntax>().ToList();
        foreach (var method in methods)
        {
            var value = ParseVariables(method);
            methodVariables.Add(method.Identifier.Text, value.Item2);
        }
        return methodVariables;
    }
}
